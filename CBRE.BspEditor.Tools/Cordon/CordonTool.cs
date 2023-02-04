using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Threading.Tasks;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Tools.Draggable;
using CBRE.BspEditor.Tools.Properties;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Hotkeys;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Tools.Cordon
{
    [Export(typeof(ITool))]
    [OrderHint("R")]
    [DefaultHotkey("Shift+K")]
    public class CordonTool : BaseDraggableTool
    {
        private readonly CordonBoxDraggableState _cordonBox;

        public CordonTool()
        {
            _cordonBox = new CordonBoxDraggableState(this);
            _cordonBox.BoxColour = Color.Red;
            _cordonBox.FillColour = Color.FromArgb(/*View.SelectionBoxBackgroundOpacity*/ 64, Color.LightGray);
            _cordonBox.State.Changed += CordonBoxChanged;
            States.Add(_cordonBox);

            Usage = ToolUsage.View2D;
        }

        public override Image GetIcon()
        {
            return Resources.Tool_Cordon;
        }

        public override string GetName()
        {
            return "CordonTool";
        }

        public override async Task ToolSelected()
        {
            _cordonBox.Update();
            await base.ToolSelected();
        }

        private void CordonBoxChanged(object sender, EventArgs e)
        {
            // Only commit changes after the resize has finished
            if (_cordonBox.State.Action != BoxAction.Drawn) return;

            var document = GetDocument();
            if (document == null) return;

            var bounds = new Box(_cordonBox.State.Start, _cordonBox.State.End);
            var cb = new CordonBounds
            {
                Box = bounds,
                Enabled = document.Map.Data.GetOne<CordonBounds>()?.Enabled == true
            };
            MapDocumentOperation.Perform(document, new TrivialOperation(x => x.Map.Data.Replace(cb), x => x.Update(cb)));
        }
    }
}
