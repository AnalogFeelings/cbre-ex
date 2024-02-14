using CBRE.Localization;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CBRE.QuickForms.Items
{
    /// <summary>
    /// A control that shows a text box and a browse button for file selection.
    /// </summary>
    public class QuickFormBrowse : QuickFormItem
    {
        private readonly string _filter;

        public QuickFormBrowse(string tbname, string filter)
        {
            Name = tbname;
            _filter = filter;
        }

        public override List<Control> GetControls(QuickForm qf)
        {
            List<Control> controls = new List<Control>();

            Label l = new Label { Text = Name, Font = SystemFonts.MessageBoxFont, FlatStyle = FlatStyle.System };
            Location(l, qf, true);
            Size(l, qf.LabelWidth);
            TextAlign(l);
            controls.Add(l);

            TextBox t = new TextBox { Name = Name, Font = SystemFonts.MessageBoxFont };
            Anchor(t);
            Location(t, qf, false);
            int textBoxWidth = qf.ClientSize.Width - (QuickForm.ItemPadding * 2);
            textBoxWidth -= qf.LabelWidth;
            textBoxWidth -= 60; // Button width
            textBoxWidth -= QuickForm.ItemPadding * 2;
            int h = QuickForm.ItemHeight;
            t.Size = new Size(textBoxWidth, h);
            controls.Add(t);

            Button b = new Button { Text = Local.LocalString("browse"), Width = 60, Font = SystemFonts.MessageBoxFont, FlatStyle = FlatStyle.System, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            Location(b, qf, false);
            b.Location = new Point(qf.ClientSize.Width - QuickForm.ItemPadding - b.Width, b.Location.Y);
            b.Click += (s, e) => ShowBrowseDialog(t);
            controls.Add(b);

            return controls;
        }

        private void ShowBrowseDialog(Control textBox)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = _filter, FileName = textBox.Text })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = ofd.FileName;
                }
            }
        }
    }
}