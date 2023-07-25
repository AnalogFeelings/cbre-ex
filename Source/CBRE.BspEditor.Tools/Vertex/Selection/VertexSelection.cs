using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations;
using CBRE.BspEditor.Modification.Operations.Data;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Threading;

namespace CBRE.BspEditor.Tools.Vertex.Selection
{
    public class VertexSelection : IEnumerable<VertexSolid>
    {
        private readonly object _lock = new object();
        private readonly ISet<VertexSolid> _selectedSolids;

        public VertexSelection()
        {
            _selectedSolids = new ThreadSafeSet<VertexSolid>();
        }

        public async Task Clear(MapDocument document)
        {
            Transaction tran = new Transaction();

            lock (_lock)
            {
                if (_selectedSolids.Any())
                {
                    List<VertexSolid> toDeselect = _selectedSolids.ToList();
                    tran.Add(new TrivialOperation(
                        d => toDeselect.ForEach(x => x.Real.Data.Remove(o => o is VertexHidden)),
                        c => c.UpdateRange(toDeselect.Select(x => x.Real))
                    ));

                    _selectedSolids.Clear();
                }
            }

            if (!tran.IsEmpty) await MapDocumentOperation.Perform(document, tran);
        }

        public async Task Update(MapDocument document)
        {
            if (document == null) return;

            Transaction tran = new Transaction();

            lock (_lock)
            {
                HashSet<Solid> selection = new HashSet<Solid>(document.Selection.OfType<Solid>());
                List<Solid> toSelect = selection.Except(_selectedSolids.Select(x => x.Real)).ToList();
                List<Solid> toDeselect = _selectedSolids.Select(x => x.Real).Except(selection).ToList();

                if (toSelect.Any())
                {
                    tran.Add(new TrivialOperation(
                        d => toSelect.ForEach(x => x.Data.Add(new VertexHidden())),
                        c => c.UpdateRange(toSelect)
                    ));
                }

                if (toDeselect.Any())
                {
                    tran.Add(new TrivialOperation(
                        d => toDeselect.ForEach(x => x.Data.Remove(o => o is VertexHidden)),
                        c => c.UpdateRange(toDeselect)
                    ));
                }

                IEnumerable<VertexSolid> rem = _selectedSolids.Where(s => toDeselect.Contains(s.Real));
                _selectedSolids.ExceptWith(rem);
                _selectedSolids.UnionWith(toSelect.Select(s => new VertexSolid(s)));
            }

            if (!tran.IsEmpty) await MapDocumentOperation.Perform(document, tran);
        }

        public async Task Commit(MapDocument document)
        {
            Transaction tran = new Transaction();

            lock (_lock)
            {
                foreach (VertexSolid solid in _selectedSolids.Where(x => x.IsDirty))
                {
                    tran.Add(new RemoveMapObjectData(solid.Real.ID, solid.Real.Faces));
                    tran.Add(new AddMapObjectData(solid.Real.ID, solid.Copy.Faces.Select(x => x.ToFace(document.Map.NumberGenerator))));
                    solid.Reset();
                }
            }

            if (!tran.IsEmpty)
            {
                await MapDocumentOperation.Perform(document, tran);
            }
        }

        public async Task Reset(MapDocument document)
        {
            lock (_lock)
            {
                foreach (VertexSolid ss in _selectedSolids)
                {
                    ss.Reset();
                }
            }
        }

        public IEnumerator<VertexSolid> GetEnumerator()
        {
            lock (_lock)
            {
                return _selectedSolids.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
