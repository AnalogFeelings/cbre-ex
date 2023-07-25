using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Shell.Hooks;

namespace CBRE.BspEditor.Modification.ChangeHandling
{
    [Export(typeof(IInitialiseHook))]
    public class ChangeHandler : IInitialiseHook
    {
        private readonly IMapDocumentChangeHandler[] _changeHandlers;

        [ImportingConstructor]
        public ChangeHandler(
            [ImportMany] IMapDocumentChangeHandler[] changeHandlers
        )
        {
            _changeHandlers = changeHandlers;
        }

        public Task OnInitialise()
        {
            Oy.Subscribe<Change>("MapDocument:Changed:Early", Changed);
            Oy.Subscribe<MapDocument>("Document:Opened", Opened);
            return Task.CompletedTask;
        }

        private Task Opened(MapDocument doc)
        {
            Change ch = new Change(doc);
            ch.AddRange(doc.Map.Root.FindAll());
            foreach (Primitives.MapData.IMapData d in doc.Map.Data) ch.Update(d);
            return Changed(ch);
        }

        private async Task Changed(Change change)
        {
            foreach (IMapDocumentChangeHandler ch in _changeHandlers.OrderBy(x => x.OrderHint))
            {
                await ch.Changed(change);
            }
        }
    }
}