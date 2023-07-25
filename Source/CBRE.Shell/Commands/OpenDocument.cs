using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Shell.Registers;

namespace CBRE.Shell.Commands
{
    /// <summary>
    /// Internal: Load a document from a path
    /// </summary>
    [Export(typeof(ICommand))]
    [CommandID("Internal:OpenDocument")]
    [InternalCommand]
    public class OpenDocument : ICommand
    {
        private readonly Lazy<DocumentRegister> _documentRegister;

        public string Name { get; set; } = "Load";
        public string Details { get; set; } = "Load";

        [ImportingConstructor]
        public OpenDocument(
            [Import] Lazy<DocumentRegister> documentRegister
        )
        {
            _documentRegister = documentRegister;
        }

        public bool IsInContext(IContext context)
        {
            return true;
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            string path = parameters.Get<string>("Path");
            string hint = parameters.Get("LoaderHint", "");

            await _documentRegister.Value.OpenDocument(path, hint);
        }
    }
}