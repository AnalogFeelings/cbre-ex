using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Documents;
using CBRE.Shell.Registers;

namespace CBRE.Shell.Commands
{
    /// <summary>
    /// Internal: Export a document to a path
    /// </summary>
    [Export(typeof(ICommand))]
    [CommandID("Internal:ExportDocument")]
    [InternalCommand]
    public class ExportDocument : ICommand
    {
        private readonly Lazy<DocumentRegister> _documentRegister;

        public string Name { get; set; } = "Export";
        public string Details { get; set; } = "Export";

        [ImportingConstructor]
        public ExportDocument(
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
            IDocument doc = parameters.Get<IDocument>("Document");
            string path = parameters.Get<string>("Path");
            string hint = parameters.Get("LoaderHint", "");

            await _documentRegister.Value.ExportDocument(doc, path, hint);
        }
    }
}