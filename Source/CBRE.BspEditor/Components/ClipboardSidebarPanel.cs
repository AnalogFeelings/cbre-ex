using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Documents;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Context;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Components
{
#if DEBUG_EXTRA
    [AutoTranslate]
    [Export(typeof(ISidebarComponent))]
#endif
    [OrderHint("K")]
    public partial class ClipboardSidebarPanel : UserControl, ISidebarComponent
    {
        [Import] private Lazy<ClipboardManager> _clipboard;

        public string Title { get; set; } = "Clipboard";
        public object Control => this;

        public ClipboardSidebarPanel()
        {
            InitializeComponent();

            Oy.Subscribe<ClipboardManager>("BspEditor:ClipboardChanged", ClipboardChanged);
        }

        private Task ClipboardChanged(ClipboardManager arg)
        {
            UpdateList();
            return Task.CompletedTask;
        }

        private void UpdateList()
        {
            ClipboardList.BeginUpdate();
            ClipboardList.Items.Clear();

            foreach (ClipboardManager.ClipboardEntry val in _clipboard.Value.GetClipboardRing().Reverse())
            {
                ClipboardList.Items.Add(val);
            }
            if (ClipboardList.Items.Count > 0) ClipboardList.SelectedIndex = 0;

            ClipboardList.EndUpdate();
        }

        public bool IsInContext(IContext context)
        {
            return context.TryGet("ActiveDocument", out MapDocument _);
        }
    }
}
