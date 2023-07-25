using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Grid;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjectData;

namespace CBRE.BspEditor.Providers.Processors
{
    /// <summary>
    /// Add some known defaults to the bsp after loading
    /// </summary>
    [Export(typeof(IBspSourceProcessor))]
    public class AddDefaults : IBspSourceProcessor
    {
        private readonly SquareGridFactory _squareGridFactory;

        [ImportingConstructor]
        public AddDefaults([Import] SquareGridFactory squareGridFactory)
        {
            _squareGridFactory = squareGridFactory;
        }

        public string OrderHint => "A";

        public async Task AfterLoad(MapDocument document)
        {
            if (!document.Map.Data.Any(x => x is GridData))
            {
                IGrid grid = await _squareGridFactory.Create(document.Environment);
                document.Map.Data.Add(new GridData(grid));
            }

            DataStructures.GameData.GameData gd = await document.Environment.GetGameData();
            document.Map.Root.Data.Replace(new PointEntityGameDataBoundingBoxProvider(gd));
            document.Map.Root.Invalidate();
        }

        public Task BeforeSave(MapDocument document)
        {
            return Task.FromResult(0);
        }
    }
}