using System;
using System.Collections.Generic;
using System.Linq;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Editing.Components.Compile.Specification
{
    public class CompileSpecification
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Engine { get; set; }
        public List<CompileTool> Tools { get; private set; }
        public List<CompilePreset> Presets { get; private set; }

        public CompileSpecification()
        {
            Tools = new List<CompileTool>();
            Presets = new List<CompilePreset>();
        }

        public CompileTool GetTool(string name)
        {
            return Tools.FirstOrDefault(x => String.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static CompileSpecification Parse(SerialisedObject gs)
        {
            CompileSpecification spec = new CompileSpecification
            {
                ID = gs.Get("ID", ""),
                Name = gs.Get("Name", ""),
                Engine = gs.Get("Engine", "")
            };
            IEnumerable<SerialisedObject> tools = gs.Children.Where(x => x.Name == "Tool");
            spec.Tools.AddRange(tools.Select(CompileTool.Parse));
            IEnumerable<SerialisedObject> presets = gs.Children.Where(x => x.Name == "Preset");
            spec.Presets.AddRange(presets.Select(CompilePreset.Parse));
            return spec;
        }

        public override string ToString()
        {
            return Name;
        }

        public string GetDefaultParameters(string name)
        {
            CompileTool tool = GetTool(name);
            return tool == null
                ? ""
                : String.Join(" ", tool.Parameters.Select(x => x.GetDefaultArgumentString()).Where(x => !String.IsNullOrWhiteSpace(x)));
        }

        public bool GetDefaultRun(string name)
        {
            CompileTool tool = GetTool(name);
            return tool != null && tool.Enabled;
        }
    }
}
