using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Compile;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Components.Compile;
using CBRE.BspEditor.Editing.Components.Compile.Profiles;
using CBRE.BspEditor.Editing.Components.Compile.Specification;
using CBRE.BspEditor.Editing.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;
using CBRE.Shell;

namespace CBRE.BspEditor.Editing.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("File", "", "Build", "B")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Run))]
    [CommandID("BspEditor:File:Compile")]
    [DefaultHotkey("F9")]
    public class OpenCompileWindow : BaseCommand
    {
        private readonly Lazy<CompileSpecificationRegister> _compileSpecificationRegister;
        private readonly Lazy<BuildProfileRegister> _buildProfileRegister;

        [ImportingConstructor]
        public OpenCompileWindow(
            [Import] Lazy<CompileSpecificationRegister> compileSpecificationRegister,
            [Import] Lazy<BuildProfileRegister> buildProfileRegister
        )
        {
            _compileSpecificationRegister = compileSpecificationRegister;
            _buildProfileRegister = buildProfileRegister;
        }

        public override string Name { get; set; } = "Run...";
        public override string Details { get; set; } = "Open the compile dialog";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            CompileSpecification spec = _compileSpecificationRegister.Value.GetCompileSpecificationsForEngine(document.Environment.Engine).FirstOrDefault();
            if (spec == null) return;

            using (CompileDialog cd = new CompileDialog(spec, _buildProfileRegister.Value))
            {
                if (await cd.ShowDialogAsync() == DialogResult.OK)
                {
                    System.Collections.Generic.IEnumerable<BatchArgument> arguments = cd.SelectedBatchArguments;
                    Batch batch = await document.Environment.CreateBatch(arguments, new BatchOptions());
                    if (batch == null) return;
                    
                    await batch.Run(document);
                }
            }
        }
    }
}