using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Editing.Components.Compile.Specification
{
    [Export(typeof(ICompileSpecificationProvider))]
    public class ApplicationEnvironmentSpecificationProvider : ICompileSpecificationProvider
    {
        [Import] private SerialisedObjectFormatter _parser;

        public async Task<IEnumerable<CompileSpecification>> GetSpecifications()
        {
            List<CompileSpecification> specs = new List<CompileSpecification>();

            string specFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Specifications");
            if (!Directory.Exists(specFolder)) return specs;

            foreach (string file in Directory.GetFiles(specFolder, "*.vdf"))
            {
                try
                {
                    using (FileStream f = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        List<SerialisedObject> gs = _parser.Deserialize(f).ToList();
                        specs.AddRange(gs.Where(x => x.Name == "Specification").Select(CompileSpecification.Parse));
                    }
                }
                catch
                {
                    // Not a valid GS
                }
            }

            return specs;
        }
    }
}