using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Environment;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Shell.Documents;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Providers
{
    [Export(typeof(IBspSourceProvider))]
    public class NativeBspSourceProvider : IBspSourceProvider
    {
        private readonly SerialisedObjectFormatter _formatter;
        private readonly MapElementFactory _factory;

        private static readonly IEnumerable<Type> SupportedTypes = new List<Type>
        {
            // Supports everything.
            typeof(IMapObject),
            typeof(IMapObjectData),
            typeof(IMapData),
        };

        public IEnumerable<Type> SupportedDataTypes => SupportedTypes;

        [ImportingConstructor]
        public NativeBspSourceProvider([Import] Lazy<SerialisedObjectFormatter> formatter, [Import] Lazy<MapElementFactory> factory)
        {
            _formatter = formatter.Value;
            _factory = factory.Value;
        }

        public IEnumerable<FileExtensionInfo> SupportedFileExtensions { get; } = new[]
        {
            new FileExtensionInfo("CBRE-EX map format", ".cxmf"), 
        };

        public async Task<BspFileLoadResult> Load(Stream stream, IEnvironment environment)
        {
            return await Task.Factory.StartNew(() =>
            {
                BspFileLoadResult result = new BspFileLoadResult();

                Map map = new Map();
                IEnumerable<SerialisedObject> so = _formatter.Deserialize(stream);
                foreach (SerialisedObject o in so)
                {
                    if (o.Name == nameof(Root))
                    {
                        map.Root.Unclone((Root) _factory.Deserialise(o));
                    }
                    else
                    {
                        map.Data.Add((IMapData) _factory.Deserialise(o));
                    }
                }
                map.Root.DescendantsChanged();

                result.Map = map;
                return result;
            });
        }
        
        public Task Save(Stream stream, Map map)
        {
            return Task.Factory.StartNew(() =>
            {
                List<SerialisedObject> list = new List<SerialisedObject>
                {
                    _factory.Serialise(map.Root)
                };
                list.AddRange(map.Data.Select(_factory.Serialise).Where(x => x != null));
                _formatter.Serialize(stream, list);
            });
        }
    }
}