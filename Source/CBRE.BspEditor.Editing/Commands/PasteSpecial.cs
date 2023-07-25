using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Components;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Components;
using CBRE.BspEditor.Editing.Properties;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Tree;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Editing.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Edit", "", "Clipboard", "H")]
    [CommandID("BspEditor:Edit:PasteSpecial")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_PasteSpecial))]
    [DefaultHotkey("Ctrl+Shift+V")]
    public class PasteSpecial : BaseCommand
    {
        private readonly Lazy<ClipboardManager> _clipboard;
        private readonly Lazy<ITranslationStringProvider> _translator;

        public override string Name { get; set; } = "Paste Special...";
        public override string Details { get; set; } = "Paste multiple copies";

        [ImportingConstructor]
        public PasteSpecial(
            [Import] Lazy<ClipboardManager> clipboard,
            [Import] Lazy<ITranslationStringProvider> translator
        )
        {
            _clipboard = clipboard;
            _translator = translator;
        }

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            if (_clipboard.Value.CanPaste())
            {
                List<IMapObject> content = _clipboard.Value.GetPastedContent(document).ToList();
                if (!content.Any()) return;

                using (PasteSpecialDialog psd = new PasteSpecialDialog(new Box(content.Select(x => x.BoundingBox))))
                {
                    _translator.Value.Translate(psd);
                    if (psd.ShowDialog() == DialogResult.OK)
                    {
                        List<IMapObject> objs = GetPastedContent(document, content, psd);
                        if (objs.Any())
                        {
                            Attach op = new Attach(document.Map.Root.ID, objs);
                            await MapDocumentOperation.Perform(document, op);
                        }
                    }
                }
            }
        }

        private List<IMapObject> GetPastedContent(MapDocument document, List<IMapObject> objectsToPaste, PasteSpecialDialog dialog)
        {
            Vector3 origin = GetPasteOrigin(document, dialog.StartPoint, objectsToPaste);
            List<IMapObject> objects = new List<IMapObject>();
            PasteSpecialDialog.PasteSpecialGrouping grouping = dialog.Grouping;
            bool makeEntitesUnique = dialog.MakeEntitiesUnique;
            int numCopies = dialog.NumberOfCopies;
            Vector3 offset = dialog.AccumulativeOffset;
            Vector3 rotation = dialog.AccumulativeRotation;

            if (objectsToPaste.Count == 1)
            {
                // Only one object - no need to group.
                grouping = PasteSpecialDialog.PasteSpecialGrouping.None;
            }

            Group allGroup = null;
            if (grouping == PasteSpecialDialog.PasteSpecialGrouping.All)
            {
                // Use one group for all copies
                allGroup = new Group(document.Map.NumberGenerator.Next("MapObject"));
                // Add the group to the tree
                objects.Add(allGroup);
            }

            // Get a list of all entity names if needed
            List<string> names = new List<string>();
            if (makeEntitesUnique)
            {
                names = document.Map.Root.Find(x => x is Entity)
                    .Select(x => x.Data.GetOne<EntityData>())
                    .Where(x => x != null && x.Properties.ContainsKey("targetname"))
                    .Select(x => x.Properties["targetname"])
                    .ToList();
            }

            // Start at i = 1 so the original isn't duped with no offets
            for (int i = 1; i <= numCopies; i++)
            {
                Vector3 copyOrigin = origin + (offset * i);
                Vector3 copyRotation = rotation * i;
                List<IMapObject> copy = CreateCopy(document.Map.NumberGenerator, copyOrigin, copyRotation, names, objectsToPaste, makeEntitesUnique, dialog.PrefixEntityNames, dialog.EntityNamePrefix).ToList();
                IEnumerable<IMapObject> grouped = GroupCopy(document.Map.NumberGenerator, allGroup, copy, grouping);
                objects.AddRange(grouped);
            }

            return objects;
        }

        private Vector3 GetPasteOrigin(MapDocument document, PasteSpecialDialog.PasteSpecialStartPoint startPoint, List<IMapObject> objectsToPaste)
        {
            // Find the starting point of the paste
            Vector3 origin;
            switch (startPoint)
            {
                case PasteSpecialDialog.PasteSpecialStartPoint.CenterOriginal:
                    // Use the original origin
                    Box box = new Box(objectsToPaste.Select(x => x.BoundingBox));
                    origin = box.Center;
                    break;
                case PasteSpecialDialog.PasteSpecialStartPoint.CenterSelection:
                    // Use the selection origin
                    origin = document.Selection.GetSelectionBoundingBox().Center;
                    break;
                default:
                    // Use the map origin
                    origin = Vector3.Zero;
                    break;
            }
            return origin;
        }

        private IEnumerable<IMapObject> CreateCopy(UniqueNumberGenerator gen, Vector3 origin, Vector3 rotation, List<string> names, List<IMapObject> objectsToPaste, bool makeEntitesUnique, bool prefixEntityNames, string entityNamePrefix)
        {
            Box box = new Box(objectsToPaste.Select(x => x.BoundingBox));

            Vector3 rads = rotation * (float) Math.PI / 180;
            Matrix4x4 mov = Matrix4x4.CreateTranslation(-box.Center); // Move to zero
            Matrix4x4 rot = Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(rads.Y, rads.X, rads.Z)); // Do rotation
            Matrix4x4 fin = Matrix4x4.CreateTranslation(origin); // Move to final origin
            Matrix4x4 transform = mov * rot * fin;

            foreach (IMapObject mo in objectsToPaste)
            {
                // Copy, transform and fix entity names
                IMapObject copy = (IMapObject) mo.Copy(gen);

                // Transform the object
                copy.Transform(transform);

                // Paste special will always texture lock (always uniform too, only translation and rotation possible)
                foreach (ITextured t in copy.Data.OfType<ITextured>()) t.Texture.TransformUniform(transform);

                FixEntityNames(copy, names, makeEntitesUnique, prefixEntityNames, entityNamePrefix);
                yield return copy;
            }
        }

        private void FixEntityNames(IMapObject obj, List<string> names, bool makeEntitesUnique, bool prefixEntityNames, string entityNamePrefix)
        {
            if (!makeEntitesUnique && !prefixEntityNames) return;

            IEnumerable<Entity> ents = obj.Find(x => x is Entity).OfType<Entity>().Where(x => x.EntityData != null);
            foreach (Entity entity in ents)
            {
                // Find the targetname property
                if (!entity.EntityData.Properties.ContainsKey("targetname")) continue;
                string prop = entity.EntityData.Properties["targetname"];

                // Skip unnamed entities
                if (string.IsNullOrWhiteSpace(prop)) continue;

                // Add the prefix before the unique check
                if (prefixEntityNames)
                {
                    prop = entityNamePrefix + prop;
                }

                // Make the name unique
                if (makeEntitesUnique)
                {
                    string name = prop;

                    // Find a unique new name for the entity
                    string newName = name;
                    int counter = 1;
                    while (names.Contains(newName))
                    {
                        newName = name + "_" + counter;
                        counter++;
                    }

                    // Set the new name and add it into the list
                    entity.EntityData.Properties["targetname"] = newName;
                    names.Add(newName);
                }
            }
        }

        private IEnumerable<IMapObject> GroupCopy(UniqueNumberGenerator gen, IMapObject allGroup, List<IMapObject> copy, PasteSpecialDialog.PasteSpecialGrouping grouping)
        {
            switch (grouping)
            {
                case PasteSpecialDialog.PasteSpecialGrouping.None:
                    // No grouping - add directly to tree
                    return copy;
                case PasteSpecialDialog.PasteSpecialGrouping.Individual:
                    // Use one group per copy
                    Group group = new Group(gen.Next("MapObject"));
                    copy.ForEach(x => x.Hierarchy.Parent = group);
                    return new List<IMapObject> { group };
                case PasteSpecialDialog.PasteSpecialGrouping.All:
                    // Use one group for all copies
                    copy.ForEach(x => x.Hierarchy.Parent = allGroup);
                    return new IMapObject[0];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}