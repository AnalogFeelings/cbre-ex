using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.BspEditor.Controls
{
    public sealed class TableSplitControl : TableLayoutPanel
    {
        private int _inH;
        private int _inV;

        private bool _resizing;

        public int MinimumViewSize { get; set; }
        private int MaximumViewSize => 100 - MinimumViewSize;

        private TableSplitConfiguration _configuration;

        public TableSplitConfiguration Configuration
        {
            get => _configuration;
            set
            {
                if (!value.IsValid()) return;
                _configuration = value;
                ResetLayout();
            }
        }

        public IEnumerable<float> RowSizes
        {
            get => RowStyles.OfType<RowStyle>().Select(x => x.Height);
            set
            {
                List<float> vals = value.ToList();
                while (vals.Count < RowStyles.Count) vals.Add(RowStyles[vals.Count].Height);
                float total = vals.Aggregate(0f, (a, b) => a + b);
                vals = vals.Select(x => x / total * 100).ToList();
                for (int i = 0; i < vals.Count; i++)
                {
                    if (i < RowStyles.Count) RowStyles[i].Height = vals[i];
                }
            }
        }

        public IEnumerable<float> ColumnSizes
        {
            get => ColumnStyles.OfType<ColumnStyle>().Select(x => x.Width);
            set
            {
                List<float> vals = value.ToList();
                while (vals.Count < ColumnStyles.Count) vals.Add(ColumnStyles[vals.Count].Width);
                float total = vals.Aggregate(0f, (a, b) => a + b);
                vals = vals.Select(x => x / total * 100).ToList();
                for (int i = 0; i < vals.Count; i++)
                {
                    if (i < ColumnStyles.Count) ColumnStyles[i].Width = vals[i];
                }
            }
        }

        private void ResetLayout()
        {
            SuspendLayout();

            // Remove any controls that aren't in the layout anymore
            List<Rectangle> recs = _configuration.Rectangles.ToList();
            foreach (Control cc in Controls.OfType<Control>().ToList())
            {
                TableLayoutPanelCellPosition pos = GetPositionFromControl(cc);
                if (recs.Any(x => x.Y == pos.Row && x.X == pos.Column)) continue;

                Controls.Remove(cc);
                cc.Dispose();
            }

            // Set the new layout
            RowCount = _configuration.Rows;
            ColumnCount = _configuration.Columns;
            ColumnStyles.Clear();
            RowStyles.Clear();
            for (int i = 0; i < ColumnCount; i++) ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (int)(100m / ColumnCount)));
            for (int i = 0; i < RowCount; i++) RowStyles.Add(new RowStyle(SizeType.Percent, (int)(100m / RowCount)));

            // Make sure there's at least an empty control in every cell
            foreach (Rectangle rec in recs)
            {
                int i = rec.X;
                int j = rec.Y;
                Control c = GetControlFromPosition(i, j);
                if (c == null) Controls.Add(c = new Panel {BackColor = SystemColors.ControlDark, Dock = DockStyle.Fill});
                SetRow(c, rec.Y);
                SetColumn(c, rec.X);
                SetRowSpan(c, rec.Height);
                SetColumnSpan(c, rec.Width);
            }

            ResumeLayout();
            ResetViews();
        }

        public TableSplitControl()
        {
            MinimumViewSize = 2;
            _resizing = false;
            _inH = _inV = -1;
            _configuration = TableSplitConfiguration.Default();
            ResetLayout();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            TableLayoutPanelCellPosition r = GetPositionFromControl(e.Control);
            Rectangle rec = _configuration.Rectangles.FirstOrDefault(t => t.X <= r.Column && t.X + t.Width > r.Column && t.Y <= r.Row && t.Y + t.Height > r.Row);
            if (!rec.IsEmpty)
            {
                SetRow(e.Control, rec.Y);
                SetColumn(e.Control, rec.X);
                SetRowSpan(e.Control, rec.Height);
                SetColumnSpan(e.Control, rec.Width);
            }
            base.OnControlAdded(e);
        }

        public void ReplaceControl(Control oldControl, Control newControl)
        {
            int col = GetColumn(oldControl),
                row = GetRow(oldControl),
                csp = GetColumnSpan(oldControl),
                rsp = GetRowSpan(oldControl);
            Controls.Add(newControl, col, row);
            Controls.Remove(oldControl);
            SetColumnSpan(newControl, csp);
            SetRowSpan(newControl, rsp);
        }

        public void ResetViews()
        {
            int c = (int) Math.Floor(100m / _configuration.Columns);
            int r = (int)Math.Floor(100m / _configuration.Rows);
            for (int i = 0; i < ColumnCount; i++) ColumnStyles[i].Width = i == 0 ? 100 - (c * (ColumnCount - 1)) : c;
            for (int i = 0; i < RowCount; i++) RowStyles[i].Height = i == 0 ? 100 - (r * (RowCount - 1)) : r;
        }

        public void FocusOn(Control ctrl)
        {
            if (ctrl == null || !Controls.Contains(ctrl)) return;
            int row = GetRow(ctrl);
            int col = GetColumn(ctrl);
            FocusOn(row, col);
        }

        public void FocusOn(int rowIndex, int columnIndex)
        {
            if (rowIndex < 0 || rowIndex > 1 || columnIndex < 0 || columnIndex > 1) return;
            RememberFocus();
            ColumnStyles[columnIndex].Width = MaximumViewSize;
            ColumnStyles[(columnIndex + 1) % 2].Width = MinimumViewSize;
            RowStyles[rowIndex].Height = MaximumViewSize;
            RowStyles[(rowIndex + 1) % 2].Height = MinimumViewSize;
        }

        private void RememberFocus()
        {
            _memoryWidth = new float[ColumnStyles.Count];
            _memoryHeight = new float[RowStyles.Count];
            for (int i = 0; i < ColumnStyles.Count; i++)
            {
                _memoryWidth[i] = ColumnStyles[i].Width;
            }
            for (int i = 0; i < RowStyles.Count; i++)
            {
                _memoryHeight[i] = RowStyles[i].Height;
            }
        }

        private void ForgetFocus()
        {
            _memoryWidth = _memoryHeight = null;
        }

        public void Unfocus()
        {
            for (int i = 0; i < ColumnStyles.Count; i++)
            {
                ColumnStyles[i].Width = _memoryWidth[i];
            }
            for (int i = 0; i < RowStyles.Count; i++)
            {
                RowStyles[i].Height = _memoryHeight[i];
            }
            ForgetFocus();
        }

        public bool IsFocusing()
        {
            return _memoryWidth != null;
        }

        private float[] _memoryWidth;
        private float[] _memoryHeight;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_resizing)
            {
                if (_inH >= 0 && Width > 0)
                {
                    ForgetFocus();
                    float mp = e.Y / (float)Height * 100;
                    SetHorizontalSplitPosition(_inH, mp);
                }
                if (_inV >= 0 && Height > 0)
                {
                    ForgetFocus();
                    float mp = e.X / (float)Width * 100;
                    SetVerticalSplitPosition(_inV, mp);
                }
            }
            else
            {
                int[] cw = GetColumnWidths();
                int[] rh = GetRowHeights();
                _inH = _inV = -1;
                int hval = 0, vval = 0;

                //todo: rowspan checks

                for (int i = 0; i < rh.Length - 1; i++)
                {
                    hval += rh[i];
                    int top = hval - Margin.Bottom;
                    int bottom = hval + Margin.Top;
                    if (e.X <= Margin.Left || e.X >= Width - Margin.Right || e.Y <= top || e.Y >= bottom) continue;
                    _inH = i;
                    break;
                }

                for (int i = 0; i < cw.Length - 1; i++)
                {
                    vval += cw[i];
                    int left = vval - Margin.Right;
                    int right = vval + Margin.Left;
                    if (e.Y <= Margin.Top || e.Y >= Height - Margin.Bottom || e.X <= left || e.X >= right) continue;
                    _inV = i;
                    break;
                }

                if (_inH >= 0 && _inV >= 0) Cursor = Cursors.SizeAll;
                else if (_inV >= 0) Cursor = Cursors.SizeWE;
                else if (_inH >= 0) Cursor = Cursors.SizeNS;
                else Cursor = Cursors.Default;
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!_resizing) Cursor = Cursors.Default;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_inV >= 0 || _inH >= 0) _resizing = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _resizing = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (_inH >= 0 && _inH < RowCount - 1)
            {
                ForgetFocus();
                SetHorizontalSplitPosition(_inH, (_inH + 1f) / RowCount * 100);
            }
            if (_inV >= 0)
            {
                ForgetFocus();
                SetVerticalSplitPosition(_inV, (_inV + 1f) / ColumnCount * 100);
            }
        }

        private void SetVerticalSplitPosition(int index, float percentage)
        {
            percentage = Math.Min(100, Math.Max(0, percentage));
            if (ColumnCount == 0 || index < 0 || index >= ColumnCount - 1 || Width <= 0) return;

            List<float> widths = ColumnStyles.OfType<ColumnStyle>().Select(x => x.Width).ToList();
            float currentPercent = widths.GetRange(0, index + 1).Sum();
            if (percentage < currentPercent)
            {
                // <--
                float diff = currentPercent - percentage;
                for (int i = index; i >= 0 && diff > 0; i--)
                {
                    float w = widths[i];
                    float nw = Math.Max(MinimumViewSize, w - diff);
                    widths[i] = nw;
                    widths[index + 1] += (w - nw);
                    diff -= (w - nw);
                }
            }
            else if (percentage > currentPercent)
            {
                // -->
                float diff = percentage - currentPercent;
                for (int i = index + 1; i < widths.Count && diff > 0; i++)
                {
                    float w = widths[i];
                    float nw = Math.Max(MinimumViewSize, w - diff);
                    widths[i] = nw;
                    widths[index] += (w - nw);
                    diff -= (w - nw);
                }
            }
            for (int i = 0; i < ColumnCount; i++)
            {
                widths[i] = (float)Math.Round(widths[i] * 10) / 10;
                ColumnStyles[i].Width = widths[i];
            }
        }

        private void SetHorizontalSplitPosition(int index, float percentage)
        {
            percentage = Math.Min(100, Math.Max(0, percentage));
            if (RowCount == 0 || index < 0 || index >= RowCount - 1 || Height <= 0) return;

            List<float> heights = RowStyles.OfType<RowStyle>().Select(x => x.Height).ToList();
            float currentPercent = heights.GetRange(0, index + 1).Sum();
            if (percentage < currentPercent)
            {
                // <--
                float diff = currentPercent - percentage;
                for (int i = index; i >= 0 && diff > 0; i--)
                {
                    float h = heights[i];
                    float nh = Math.Max(MinimumViewSize, h - diff);
                    heights[i] = nh;
                    heights[index + 1] += (h - nh);
                    diff -= (h - nh);
                }
            }
            else if (percentage > currentPercent)
            {
                // -->
                float diff = percentage - currentPercent;
                for (int i = index + 1; i < heights.Count && diff > 0; i++)
                {
                    float h = heights[i];
                    float nh = Math.Max(MinimumViewSize, h - diff);
                    heights[i] = nh;
                    heights[index] += (h - nh);
                    diff -= (h - nh);
                }
            }
            for (int i = 0; i < RowCount; i++)
            {
                heights[i] = (float)Math.Round(heights[i] * 10) / 10;
                RowStyles[i].Height = heights[i];
            }
        }
    }
}
