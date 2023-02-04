using System.ComponentModel.Composition;
using System.Drawing;
using System.Threading.Tasks;
using CBRE.BspEditor.Environment;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Grid
{
    /// <summary>
    /// A grid that does nothing
    /// </summary>
    [AutoTranslate]
    [Export(typeof(IGridFactory))]
    public class NoGridFactory : IGridFactory
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Image Icon => null;

        public async Task<IGrid> Create(IEnvironment environment)
        {
            return new NoGrid();
        }

        public bool IsInstance(IGrid grid)
        {
            return grid is NoGrid;
        }
    }
}