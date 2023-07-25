using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Context;

namespace CBRE.Shell.Controls
{
    public abstract partial class TextSidebarPanel : UserControl, ISidebarComponent
    {
        public abstract string Title { get; }
        public abstract string Text { get; }
        public object Control => this;

        protected TextSidebarPanel()
        {
            InitializeComponent();
            UpdateText();
        }

        public abstract bool IsInContext(IContext context);

        protected void UpdateText()
        {
            string text = Text ?? "";
            HelpTextBox.ResetFont();
            string rtf = ConvertSimpleMarkdownToRtf(text);
            HelpTextBox.Rtf = rtf;
            System.Drawing.Size size = TextRenderer.MeasureText(HelpTextBox.Text, HelpTextBox.Font, HelpTextBox.Size, TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
            Height = size.Height + HelpTextBox.Margin.Vertical + HelpTextBox.Lines.Length * 5;
        }

        /// <summary>
        /// Converts simple markdown into RTF.
        /// Simple markdown is a very limited subset of markdown. It supports:
        /// - Lists, delimited with -
        /// - Bold, delimited with *
        /// - Paragraphs/new lines
        /// </summary>
        /// <param name="simpleMarkdown"></param>
        private string ConvertSimpleMarkdownToRtf(string simpleMarkdown)
        {
            /*
             * {\rtf1\utf8\f0\pard
             *   This is some {\b bold} text.\par
             * }";
             */
            string escaped = simpleMarkdown
                .Replace("\\", "\\\\")
                .Replace("{", "\\{")
                .Replace("}", "\\}");

            StringBuilder sb = new StringBuilder();
            foreach (char c in escaped)
            {
                if (c > 127) sb.AppendFormat(@"\u{0}?", (int) c);
                else if (c == '\\') sb.Append("\\\\");
                else if (c == '{') sb.Append("\\{");
                else if (c == '}') sb.Append("\\}");
                else sb.Append(c);
            }

            string bolded = Regex.Replace(sb.ToString(), @"\*(?:\b(?=\w)|(?=\\))(.*?)\b(?!\w)\*", @"{\b $1}");
            string bulleted = Regex.Replace(bolded, @"^\s*-\s+", @" \bullet  ", RegexOptions.Multiline);
            string paragraphs = Regex.Replace(bulleted, @"(\r?\n){2,}", "\\par\\par ");
            string lines = Regex.Replace(paragraphs, @"(\r?\n)+", "\\par ");

            return @"{\rtf1\ansi\f0\pard\sa60 " + lines + " }";
        }
    }
}
