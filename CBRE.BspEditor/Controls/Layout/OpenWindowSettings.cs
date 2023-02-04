using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Components;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;
using CBRE.Shell;

namespace CBRE.BspEditor.Controls.Layout
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Window:WindowSettings")]
    [MenuItem("Window", "", "Layout", "D")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_WindowSettings))]
    [AllowToolbar(false)]
    public class OpenWindowSettings : BaseCommand
    {
        private readonly Lazy<MapDocumentControlHost> _host;
        private readonly Lazy<ITranslationStringProvider> _translator;

        [ImportingConstructor]
        public OpenWindowSettings(
            [Import] Lazy<MapDocumentControlHost> host,
            [Import] Lazy<ITranslationStringProvider> translator
        )
        {
            _translator = translator;
            _host = host;
        }

        public override string Name { get; set; } = "Modify layout...";
        public override string Details { get; set; } = "Open the window layout settings dialog";
        
        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            var lsw = new LayoutSettings(_host.Value.GetConfigurations());
            _translator.Value.Translate(lsw);
            
            if (await lsw.ShowDialogAsync() == DialogResult.OK)
            {
                var configs = lsw.Configurations;
                _host.Value.SetConfigurations(configs);
            }
        }
    }
}