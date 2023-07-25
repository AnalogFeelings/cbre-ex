using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CBRE.Shell.Controls
{
    // https://stackoverflow.com/a/24087828
    public class DropdownButton : Button
    {
        [DefaultValue(null)]
        public ContextMenuStrip Menu { get; set; }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (Menu == null || mevent.Button != MouseButtons.Left) return;

            Point menuLocation = new Point(0, Height);
            Menu.Show(this, menuLocation);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (Menu == null) return;

            int arrowX = ClientRectangle.Width - 14;
            int arrowY = ClientRectangle.Height / 2 - 1;

            Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
            Point[] arrows = { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
            pevent.Graphics.FillPolygon(brush, arrows);
        }
    }
}
