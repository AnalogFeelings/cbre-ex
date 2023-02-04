using System.Threading.Tasks;
using CBRE.BspEditor.Documents;

namespace CBRE.BspEditor.Providers.Processors
{
    public interface IBspSourceProcessor
    {
        string OrderHint { get; }
        Task AfterLoad(MapDocument document);
        Task BeforeSave(MapDocument document);
    }
}
