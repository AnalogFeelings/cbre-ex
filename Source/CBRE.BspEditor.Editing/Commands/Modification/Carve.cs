using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Properties;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Tree;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands.Modification
{
    /// <summary>
    /// For all solids (carvee) that intersect with the selected solids (carver),
    /// the carvee will be clipped against all planes of the carver, retaining the front solids.
    /// This technique is typically not recommended as it can create inefficient results.
    /// Using the clip tool is encouraged instead.
    /// </summary>
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Tools", "", "Evil", "B")]
    [CommandID("BspEditor:Tools:Carve")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Carve))]
    [DefaultHotkey("Ctrl+Shift+C")]
    public class Carve : BaseCommand
    {
        public override string Name { get; set; } = "Carve";
        public override string Details { get; set; } = "Carve";

        protected override bool IsInContext(IContext context, MapDocument document)
        {
            return base.IsInContext(context, document)
                && document.Selection.Any(x => x is Solid);
        }

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            // Get the carving objects (whatever's selected)
            List<Solid> carvers = document.Selection.OfType<Solid>().ToList();
            if (!carvers.Any()) return;

            // Get the carve set. This set doesn't have to explicitly intersect with a carver,
            // it's only a list of candidates. The carve operation does a more precise filter.
            IEnumerable<Solid> carvees = document.Map.Root
                .Find(x => x is Solid && carvers.Any(c => x.BoundingBox.IntersectsWith(c.BoundingBox)))
                .OfType<Solid>()
                .Where(x => !carvers.Contains(x));

            // Perform the carve
            Transaction tns = CarveObjects(document, carvers, carvees);
            if (tns.IsEmpty) return;

            // Commit changes
            await MapDocumentOperation.Perform(document, tns);
        }

        /// <summary>
        /// For each solid in <see cref="carvers"/>, split intersecting solids in <see cref="carvees"/>
        /// by that solid. A carvee may be carved multiple times by the carver set.
        /// </summary>
        /// <param name="document">The document that's being edited</param>
        /// <param name="carvers">The list of objects to carve with</param>
        /// <param name="carvees">The list of objects that will be carved</param>
        /// <returns>An operation that will commit the changes.</returns>
        private static Transaction CarveObjects(MapDocument document, IEnumerable<Solid> carvers, IEnumerable<Solid> carvees)
        {
            // Create a copy of the carvee list which will be modified as the carve happens
            List<Solid> carveSet = carvees.ToList();

            // Create a carve data to track changes before they are committed
            CarveData data = new CarveData(carveSet);

            foreach (Solid carver in carvers)
            {
                List<Solid> added = new List<Solid>();
                List<Solid> removed = new List<Solid>();

                // Carve all the candidates in the carve set
                // Since we're carving with only one object at this point, we can do all the carve set in one go
                // and do the add/remove operations at the end. This stops the carve result from potentially being re-carved somehow.
                foreach (Solid carvee in carveSet)
                {
                    List<Solid> result = PerformCarve(document, carver, carvee, data);
                    if (result == null) continue;
                    
                    added.AddRange(result);
                    removed.Add(carvee);
                }

                // Update the carve set with the new changes
                carveSet.RemoveAll(removed.Contains);
                carveSet.AddRange(added);
            }

            // Return the result
            return data.Transaction;
        }

        /// <summary>
        /// Carve one solid into another and output the results into the provided lists.
        /// </summary>
        /// <param name="document">The document that's being edited</param>
        /// <param name="carver">The solid to carve with</param>
        /// <param name="carvee">The solid to carve into</param>
        /// <param name="data">The current carve data</param>
        /// <returns>The list of added solids if the carve was successful, null otherwise</returns>
        private static List<Solid> PerformCarve(MapDocument document, Solid carver, Solid carvee, CarveData data)
        {
            bool split = false;
            Solid solid = carvee; // this reference will change as the carve continues
            List<Solid> list = new List<Solid>();

            foreach (DataStructures.Geometric.Plane plane in carver.Faces.Select(x => x.Plane))
            {
                // Split solid by plane
                Solid back, front;
                try
                {
                    if (!solid.Split(document.Map.NumberGenerator, plane, out back, out front))
                    {
                        // If the solid is completely in front of the plane, then the solids don't overlap
                        if (back == null && front != null) return null;
                    }
                }
                catch
                {
                    // We're not too fussy about over-complicated carving, just get out if we've broken it.
                    break;
                }
                split = true;

                if (front != null)
                {
                    // Retain the front solid
                    if (solid.IsSelected) front.IsSelected = true;
                    list.Add(front);
                }

                // If the back isn't valid, exit the carve as it cannot continue
                if (back == null || !back.IsValid()) break;

                // Use the back solid as the new clipping target
                if (solid.IsSelected) back.IsSelected = true;
                solid = back;
            }

            if (!split) return null;

            // Update the carve data
            foreach (Solid s in list) data.Attach(carvee, s);
            data.Detatch(carvee);

            return list;
        }

        /// <summary>
        /// A data class used to store object parents before they are committed
        /// to the map.
        /// </summary>
        private class CarveData
        {
            private Dictionary<Solid, long> ObjectsAndParents { get; }
            public Transaction Transaction { get; }

            public CarveData(IEnumerable<Solid> solids)
            {
                ObjectsAndParents = solids.ToDictionary(x => x, x => x.Hierarchy.Parent.ID);
                Transaction = new Transaction();
            }

            public void Attach(Solid originalSolid, Solid solid)
            {
                long parent = ObjectsAndParents[originalSolid];
                ObjectsAndParents[solid] = parent;
                Transaction.Add(new Attach(parent, solid));
            }

            public void Detatch(Solid solid)
            {
                long parent = ObjectsAndParents[solid];
                Transaction.Add(new Detatch(parent, solid));
            }
        }
    }
}
