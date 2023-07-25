using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Components.Visgroup;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Data;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Map:Visgroups")]
    public class EditVisgroups : BaseCommand
    {
        [Import] private Lazy<ITranslationStringProvider> _translator;

        public override string Name { get; set; } = "Edit visgroups";
        public override string Details { get; set; } = "View and edit map visgroups";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            using (VisgroupEditForm vg = new VisgroupEditForm(document))
            {
                _translator.Value.Translate(vg);
                if (vg.ShowDialog() == DialogResult.OK)
                {
                    List<Visgroup> nv = new List<Visgroup>();
                    List<Visgroup> cv = new List<Visgroup>();
                    List<Visgroup> dv = new List<Visgroup>();

                    vg.PopulateChangeLists(document, nv, cv, dv);

                    if (nv.Any() || cv.Any() || dv.Any())
                    {
                        Transaction tns = new Transaction();

                        if (dv.Any())
                        {
                            List<long> ids = dv.Select(x => x.ID).ToList();
                            tns.Add(new RemoveMapData(document.Map.Data.Get<Visgroup>().Where(x => ids.Contains(x.ID))));
                        }

                        if (cv.Any())
                        {
                            List<long> ids = cv.Select(x => x.ID).ToList();
                            tns.Add(new RemoveMapData(document.Map.Data.Get<Visgroup>().Where(x => ids.Contains(x.ID))));
                            tns.Add(new AddMapData(cv));
                        }

                        if (nv.Any())
                        {
                            tns.Add(new AddMapData(nv));
                        }

                        await MapDocumentOperation.Perform(document, tns);
                    }
                }
            }
        }
    }
}