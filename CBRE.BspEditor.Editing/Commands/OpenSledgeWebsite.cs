using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Help", "", "Links", "D")]
    [CommandID("BspEditor:Links:SledgeWebsite")]
    public class OpenSledgeWebsite : ICommand
    {
        public string Name { get; set; } = "Sledge Website";
        public string Details { get; set; } = "Go to the Sledge website";

        public bool IsInContext(IContext context)
        {
            return true;
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            System.Diagnostics.Process.Start("http://sledge-editor.com/");
        }
    }
}