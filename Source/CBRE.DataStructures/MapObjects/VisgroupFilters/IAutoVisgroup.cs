using CBRE.DataStructures.GameData;
using CBRE.Localization;
using System;
using System.Linq;

namespace CBRE.DataStructures.MapObjects.VisgroupFilters
{

    /*
     * These are all the auto visgroups I could find in Hammer 4, plus a bit more:
     *   - = Source only
     *   + = Source and GoldSource
     *   * = GoldSource only
     * Entities
     *   + Brush Entities -> entity type
     *   + Point Entities -> entity type
     *   + Triggers -> entity classes
     *   + Lights -> entity classes
     *   + Nodes -> entity classes
     *   - NPCs -> entity classes
     * Tool Brushes
     *   - Occluders -> entity class
     *   - Areaportals -> entity class
     *   - Area Portal -> face texture
     *   - Fog -> face texture
     *   + Hint -> face texture
     *   - Occluder -> face texture
     *   + Origin -> face texture
     *   + Skip -> face texture
     *   + Trigger -> face texture
     *   * Bevel -> face texture
     * Custom -> FGD @AutoVisGroups
     * World Geometry -> all non-entity solids
     *   - Nodraw -> face texture
     *   * Null -> face texture
     *   - Displacements -> face displacement
     *   + Sky -> face texture
     *   - Black -> face texture
     *   + Water -> face texture metadata
     * World Details
     *   - Func Detail -> entity class
     *   - Props -> entity classes
     * Clips
     *   - Control -> face texture
     *   - NPC -> face texture
     *   - Player -> face texture
     *   + Clip -> face texture
     * Block
     *   - LOS -> face texture
     *   - Bullets -> face texture
     *   - Light -> face texture
     * Invisible
     *   - Invisible -> face texture
     *   - Ladder -> face texture
     *   
     * For GoldSource / milestone 1:
     * Entities
     *   + Brush Entities -> entity type
     *   + Point Entities -> entity type
     *   + Triggers -> entity classes
     *   + Lights -> entity classes
     *   + Nodes -> entity classes
     * Tool Brushes
     *   + Hint -> face texture
     *   + Origin -> face texture
     *   + Skip -> face texture
     *   + Trigger -> face texture
     *   * Bevel -> face texture
     *   + Clip -> face texture
     * World Geometry -> all non-entity solids
     *   * Null -> face texture
     *   + Sky -> face texture
     *   + Water -> face texture metadata
     *   
     */

    public interface IVisgroupFilter
    {
        string Group { get; }
        string Name { get; }
        bool IsMatch(MapObject mapObject);
    }

    public class BrushEntitiesVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.entities"); } }
        public string Name { get { return Local.LocalString("visgroup.entities.brush"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Entity && ((Entity)x).GameData != null && ((Entity)x).GameData.ClassType == ClassType.Solid;
        }
    }

    public class PointEntitiesVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.entities"); } }
        public string Name { get { return Local.LocalString("visgroup.entities.point"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Entity && ((Entity)x).GameData != null && ((Entity)x).GameData.ClassType != ClassType.Solid;
        }
    }

    public class TriggersVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.entities"); } }
        public string Name { get { return Local.LocalString("visgroup.entities.triggers"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Entity && x.GetEntityData().Name.StartsWith("trigger_");
        }
    }

    public class LightsVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.entities"); } }
        public string Name { get { return Local.LocalString("visgroup.entities.lights"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Entity && x.GetEntityData().Name.StartsWith("light");
        }
    }

    public class NodesVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.entities"); } }
        public string Name { get { return Local.LocalString("visgroup.entities.nodes"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Entity && x.GetEntityData().Name.Contains("_node");
        }
    }

    public class HintVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.tool_brushes"); } }
        public string Name { get { return Local.LocalString("visgroup.tool_brushes.hint"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Solid && ((Solid)x).Faces.Any(y => String.Equals(y.Texture.Name, "hint", StringComparison.OrdinalIgnoreCase));
        }
    }

    public class OriginVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.tool_brushes"); } }
        public string Name { get { return Local.LocalString("visgroup.tool_brushes.origin"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Solid && ((Solid)x).Faces.Any(y => String.Equals(y.Texture.Name, "origin", StringComparison.OrdinalIgnoreCase));
        }
    }

    public class SkipVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.tool_brushes"); } }
        public string Name { get { return Local.LocalString("visgroup.tool_brushes.skip"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Solid && ((Solid)x).Faces.Any(y => String.Equals(y.Texture.Name, "skip", StringComparison.OrdinalIgnoreCase));
        }
    }

    public class TriggerVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.tool_brushes"); } }
        public string Name { get { return Local.LocalString("visgroup.tool_brushes.trigger"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Solid && ((Solid)x).Faces.Any(y => String.Equals(y.Texture.Name, "aaatrigger", StringComparison.OrdinalIgnoreCase));
        }
    }

    public class BevelVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.tool_brushes"); } }
        public string Name { get { return Local.LocalString("visgroup.tool_brushes.bevel"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Solid && ((Solid)x).Faces.Any(y => String.Equals(y.Texture.Name, "bevel", StringComparison.OrdinalIgnoreCase));
        }
    }

    public class BrushesVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.world_geometry"); } }
        public string Name { get { return Local.LocalString("visgroup.world_geometry.brushes"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Solid && x.FindClosestParent(y => y is Entity) == null;
        }
    }

    public class NullVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.world_geometry"); } }
        public string Name { get { return Local.LocalString("visgroup.world_geometry.null"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Solid && x.FindClosestParent(y => y is Entity) == null && ((Solid)x).Faces.Any(y => String.Equals(y.Texture.Name, "null", StringComparison.OrdinalIgnoreCase));
        }
    }

    public class SkyVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.world_geometry"); } }
        public string Name { get { return Local.LocalString("visgroup.world_geometry.sky"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Solid && x.FindClosestParent(y => y is Entity) == null && ((Solid)x).Faces.Any(y => String.Equals(y.Texture.Name, "sky", StringComparison.OrdinalIgnoreCase));
        }
    }

    public class WaterVisgroupFilter : IVisgroupFilter
    {
        public string Group { get { return Local.LocalString("visgroup.world_geometry"); } }
        public string Name { get { return Local.LocalString("visgroup.world_geometry.water"); } }
        public bool IsMatch(MapObject x)
        {
            return x is Solid && x.FindClosestParent(y => y is Entity) == null && ((Solid)x).Faces.Any(y => y.Texture.Name.StartsWith("!"));
        }
    }
}
