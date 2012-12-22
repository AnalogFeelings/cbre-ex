﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Sledge.DataStructures.Geometric;
using Sledge.DataStructures.Transformations;

namespace Sledge.DataStructures.MapObjects
{
    public abstract class MapObject
    {
        public string ClassName { get; set; }
        public List<int> Visgroups { get; set; }
        public List<MapObject> Children { get; set; }
        public MapObject Parent { get; set; }
        public Color Colour { get; set; }
        public bool IsSelected { get; set; }
        public Box BoundingBox { get; set; }

        protected MapObject()
        {
            Visgroups = new List<int>();
            Children = new List<MapObject>();
        }

        /// <summary>
        /// Creates an exact copy of this object
        /// </summary>
        public abstract MapObject Clone();

        /// <summary>
        /// Copies all the values of the provided object into this one
        /// </summary>
        public abstract void Unclone(MapObject o);

        protected void CloneBase(MapObject o)
        {
            o.ClassName = ClassName;
            o.Visgroups.AddRange(Visgroups);
            o.Parent = Parent;
            o.Colour = Colour;
            o.IsSelected = IsSelected;
            o.BoundingBox = BoundingBox.Clone();
            foreach (var c in Children.Select(x => x.Clone()))
            {
                c.Parent = o;
                o.Children.Add(c);
            }
        }

        protected void UncloneBase(MapObject o)
        {
            Visgroups.Clear();
            Children.Clear();

            ClassName = o.ClassName;
            Visgroups.AddRange(o.Visgroups);
            Parent = o.Parent;
            Colour = o.Colour;
            IsSelected = o.IsSelected;
            BoundingBox = o.BoundingBox.Clone();
            foreach (var c in o.Children.Select(x => x.Clone()))
            {
                c.Parent = this;
                Children.Add(c);
            }
        }

        public void RemoveDescendant(MapObject remove)
        {
            if (Children.Contains(remove))
            {
                Children.Remove(remove);
            }
            else
            {
                Children.ForEach(x => x.RemoveDescendant(remove));
            }
        }

        public virtual void UpdateBoundingBox(bool cascadeToParent = true)
        {
            if (cascadeToParent && Parent != null)
            {
                Parent.UpdateBoundingBox();
            }
        }

        public virtual void Transform(IUnitTransformation transform)
        {
            Children.ForEach(c => c.Transform(transform));
            UpdateBoundingBox(false);
        }

        public virtual Coordinate GetIntersectionPoint(Line line)
        {
            return null;
        }

        /// <summary>
        /// Searches upwards for the last parent that is not null and is not
        /// an instance of <code>Sledge.DataStructures.MapObjects.World</code>.
        /// </summary>
        /// <param name="o">The starting node</param>
        /// <returns>The highest parent that is not a worldspawn instance</returns>
        public static MapObject GetTopmostNonRootParent(MapObject o)
        {
            while (o.Parent != null && !(o.Parent is World))
            {
                o = o.Parent;
            }
            return o;
        }

        /// <summary>
        /// Searches upwards for the root node as an instance of
        /// <code>Sledge.DataStructures.MapObjects.World</code>.
        /// </summary>
        /// <param name="o">The starting node</param>
        /// <returns>The root world node, or null if it doesn't exist.</returns>
        public static World GetRoot(MapObject o)
        {
            while (o != null && !(o is World))
            {
                o = o.Parent;
            }
            return o as World;
        }

        /// <summary>
        /// Flattens the tree underneath this node.
        /// </summary>
        /// <returns>A list containing all the descendants of this node (including this node)</returns>
        public IEnumerable<MapObject> GetAllNodes()
        {
            return Children.SelectMany(x => x.GetAllNodes()).Union(new[] {this});
        }

        /// <summary>
        /// Flattens the tree and selects the nodes that match the test.
        /// </summary>
        /// <param name="test">The test function</param>
        /// <returns>A list of all the descendants that match the test (including this node)</returns>
        public IEnumerable<MapObject> GetAllNodesMatching(Func<MapObject, bool> test)
        {
            return GetAllNodes().Where(test);
        }

        /// <summary>
        /// Get all the nodes starting from this node that intersect with a box.
        /// </summary>
        /// <param name="box">The intersection box</param>
        /// <returns>A list of all the descendants that intersect with the box.</returns>
        public IEnumerable<MapObject> GetAllNodesIntersectingWith(Box box)
        {
            var list = new List<MapObject>();
            if (!(this is World))
            {
                if (BoundingBox == null || !BoundingBox.IntersectsWith(box)) return list;
                if (this is Solid || this is Entity) list.Add(this);
            }
            list.AddRange(Children.SelectMany(x => x.GetAllNodesIntersectingWith(box)));
            return list;
        }

        /// <summary>
        /// Get all the nodes starting from this node that intersect with a line.
        /// </summary>
        /// <param name="line">The intersection line</param>
        /// <returns>A list of all the descendants that intersect with the line.</returns>
        public IEnumerable<MapObject> GetAllNodesIntersectingWith(Line line)
        {
            var list = new List<MapObject>();
            if (!(this is World))
            {
                if (BoundingBox == null || !BoundingBox.IntersectsWith(line)) return list;
                if (this is Solid || this is Entity) list.Add(this);
            }
            list.AddRange(Children.SelectMany(x => x.GetAllNodesIntersectingWith(line)));
            return list;
        }

        /// <summary>
        /// Get all the nodes starting from this node that are entirely contained within a box.
        /// </summary>
        /// <param name="box">The containing box</param>
        /// <returns>A list of all the descendants that are contained within the box.</returns>
        public IEnumerable<MapObject> GetAllNodesContainedWithin(Box box)
        {
            var list = new List<MapObject>();
            if (!(this is World))
            {
                if (BoundingBox == null || !BoundingBox.ContainedWithin(box)) return list;
                if (this is Solid || this is Entity) list.Add(this);
            }
            list.AddRange(Children.SelectMany(x => x.GetAllNodesContainedWithin(box)));
            return list;
        }

        /// <summary>
        /// Get all the solid nodes starting from this node where the
        /// edges of the solid intersect with the provided box.
        /// </summary>
        /// <param name="box">The intersection box</param>
        /// <returns>A list of all the solid descendants where the edges of the solid intersect with the box.</returns>
        public IEnumerable<MapObject> GetAllNodesIntersecting2DLineTest(Box box)
        {
            var list = new List<MapObject>();
            if (!(this is World))
            {
                if (BoundingBox == null || !BoundingBox.IntersectsWith(box)) return list;
                if (this is Solid && ((Solid)this).Faces.Any(f => f.IntersectsWithLine(box)))
                {
                    list.Add(this);
                }
            }
            list.AddRange(Children.SelectMany(x => x.GetAllNodesIntersecting2DLineTest(box)));
            return list;
        }
    }
}