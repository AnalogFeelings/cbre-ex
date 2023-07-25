using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;
using CBRE.DataStructures.Geometric;
using CBRE.QuickForms;

namespace CBRE.BspEditor.Editing.Commands.View
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:View:GoToLocation")]
    [MenuItem("View", "", "GoTo", "H")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_GoToCoordinates))]
    public class GoToLocation : BaseCommand
    {
        public override string Name { get; set; } = "Go to location";
        public override string Details { get; set; } = "Center views on a specific set of coordinates.";

        public string Title { get; set; }
        public string OK { get; set; }
        public string Cancel { get; set; }

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            using (QuickForm qf = new QuickForm(Title) { UseShortcutKeys = true }.TextBox("X", "X", "0").TextBox("Y", "Y", "0").TextBox("Z", "Z", "0").OkCancel(OK, Cancel))
            {
                qf.ClientSize = new Size(180, qf.ClientSize.Height);

                if (await qf.ShowDialogAsync() != DialogResult.OK) return;

                if (!decimal.TryParse(qf.String("X"), out decimal x)) return;
                if (!decimal.TryParse(qf.String("Y"), out decimal y)) return;
                if (!decimal.TryParse(qf.String("Z"), out decimal z)) return;

                Vector3 coordinate = new Vector3((float) x, (float) y, (float) z);

                Box box = new Box(coordinate - (Vector3.One * 10), coordinate + (Vector3.One * 10));

                await Task.WhenAll(
                    Oy.Publish("MapDocument:Viewport:Focus3D", box),
                    Oy.Publish("MapDocument:Viewport:Focus2D", box)
                );
            }
        }
    }
}