using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;

namespace CBRE.BspEditor.Providers.Processors
{
    /// <summary>
    /// Ensure that objects marked as "is selected" are added to the selection
    /// </summary>
    [Export(typeof(IBspSourceProcessor))]
    public class HandleSelection : IBspSourceProcessor
    {
        public string OrderHint => "B";

        public Task AfterLoad(MapDocument document)
        {
            document.Selection.Update(document);
            return Task.FromResult(0);
        }

        public Task BeforeSave(MapDocument document)
        {
            return Task.FromResult(0);
        }
    }
}