using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.Common.Transport;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Primitives.MapObjects
{
    /// <summary>
    /// A collection of faces
    /// </summary>
    public class Solid : BaseMapObject
    {
        public IEnumerable<Face> Faces => Data.Get<Face>();
        public ObjectColor Color => Data.GetOne<ObjectColor>();

        public Solid(long id) : base(id)
        {
        }

        public Solid(SerialisedObject obj) : base(obj)
        {
        }

        [Export(typeof(IMapElementFormatter))]
        public class SolidFormatter : StandardMapElementFormatter<Solid> { }

        protected override Box GetBoundingBox()
        {
            List<Face> faces = Faces.ToList();
            return faces.Any(x => x.Vertices.Count > 0) ? new Box(faces.SelectMany(x => x.Vertices)) : Box.Empty;
        }
        
        public override IEnumerable<Polygon> GetPolygons()
        {
            return Faces.Select(x => x.ToPolygon());
        }

        public Polyhedron ToPolyhedron()
        {
            return new Polyhedron(GetPolygons());
        }

        protected override string SerialisedName => "Solid";

        public override IEnumerable<IMapObject> Decompose(IEnumerable<Type> allowedTypes)
        {
            yield return this;
        }
    }
}