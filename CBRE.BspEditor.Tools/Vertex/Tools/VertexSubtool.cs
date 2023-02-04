using System.ComponentModel.Composition;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.BspEditor.Tools.Draggable;
using CBRE.BspEditor.Tools.Vertex.Selection;

namespace CBRE.BspEditor.Tools.Vertex.Tools
{
    public abstract class VertexSubtool : BaseDraggableTool
    {
        protected VertexSubtool()
        {
            Active = false;
            Title = GetName() ?? GetType().Name;
        }

        public abstract string OrderHint { get; }
        public VertexSelection Selection { get; set; }
        [Import] public VertexTool Parent { get; set; }
        
        public override Image GetIcon() => null;
        public abstract Task SelectionChanged();
        public abstract Control Control { get; }
        public string Title { get; set; }

        protected void Invalidate()
        {
            Parent.Invalidate();
        }
        
        public abstract void Update();
    }
}
