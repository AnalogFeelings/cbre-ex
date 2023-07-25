using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Editing.Commands.Pointfile
{
    public class Pointfile : IMapData
    {
        public bool AffectsRendering => true;

        public List<Line> Lines { get; set; }

        private Pointfile()
        {
            Lines = new List<Line>();
        }

        public Pointfile(SerialisedObject obj)
        {
            Lines = new List<Line>();
            foreach (SerialisedObject l in obj.Children.Where(x => x.Name == "Line"))
            {
                Lines.Add(new Line(
                    l.Get<Vector3>("Start"),
                    l.Get<Vector3>("End")
                ));
            }
        }

        public static Pointfile Parse(IEnumerable<string> lines)
        {
            Pointfile pf = new Pointfile();
            List<string> list = lines.ToList();
            if (!list.Any()) return pf;

            // Format detection: look at one line
            // .lin format: coordinate - coordinate
            // .pts format: coordinate
            string[] detect = list[0].Split(' ');
            bool lin = detect.Length == 7;
            bool pts = detect.Length == 3;
            if (!lin && !pts) throw new Exception("Invalid pointfile format.");

            Vector3? previous = null;
            foreach (string line in list)
            {
                string[] split = line.Split(' ');
                Vector3 point = NumericsExtensions.Parse(split[0], split[1], split[2], NumberStyles.Float, CultureInfo.InvariantCulture);
                if (lin)
                {
                    Vector3 point2 = NumericsExtensions.Parse(split[4], split[5], split[6], NumberStyles.Float, CultureInfo.InvariantCulture);
                    pf.Lines.Add(new Line(point2, point));
                }
                else // pts
                {
                    if (previous.HasValue) pf.Lines.Add(new Line(previous.Value, point));
                    previous = point;
                }
            }

            return pf;
        }

        [Export(typeof(IMapElementFormatter))]
        public class PointfileFormatter : StandardMapElementFormatter<Pointfile> { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Lines", Lines);
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public IMapElement Clone()
        {
            return new Pointfile()
            {
                Lines = Lines.Select(x => new Line(x.Start, x.End)).ToList()
            };
        }

        public SerialisedObject ToSerialisedObject()
        {
            SerialisedObject so = new SerialisedObject("Pointfile");
            foreach (Line line in Lines)
            {
                SerialisedObject l = new SerialisedObject("Line");
                l.Set("Start", line.Start);
                l.Set("End", line.End);
                so.Children.Add(l);
            }
            return so;
        }
    }
}
