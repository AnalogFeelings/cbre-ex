using CBRE.DataStructures.GameData;
using CBRE.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Property = CBRE.DataStructures.MapObjects.Property;

namespace CBRE.Editor.UI.ObjectProperties
{
    internal class TableValue
    {
        public string Class { get; set; }
        public string OriginalKey { get; set; }
        public string NewKey { get; set; }
        public string Value { get; set; }
        public bool IsModified { get; set; }
        public bool IsAdded { get; set; }
        public bool IsRemoved { get; set; }

        public Color GetColour()
        {
            if (IsAdded) return Color.LightBlue;
            if (IsRemoved) return Color.LightPink;
            if (IsModified) return Color.LightGreen;
            return Color.Transparent;
        }

        public string DisplayText(GameData gd)
        {
            GameDataObject cls = gd.Classes.FirstOrDefault(x => x.Name == Class);
            DataStructures.GameData.Property prop = cls == null ? null : cls.Properties.FirstOrDefault(x => x.Name == NewKey);
            return prop == null ? NewKey : prop.DisplayText();
        }

        public string DisplayValue(GameData gd)
        {
            GameDataObject cls = gd.Classes.FirstOrDefault(x => x.Name == Class);
            DataStructures.GameData.Property prop = cls == null ? null : cls.Properties.FirstOrDefault(x => x.Name == OriginalKey && x.VariableType == VariableType.Choices);
            Option opt = prop == null ? null : prop.Options.FirstOrDefault(x => x.Key == Value);
            return opt == null ? Value : opt.Description;
        }

        public static List<TableValue> Create(GameData gd, string className, List<Property> props)
        {
            List<TableValue> list = new List<TableValue>();
            GameDataObject cls = gd.Classes.FirstOrDefault(x => x.Name == className);
            List<DataStructures.GameData.Property> gameDataProps = cls != null ? cls.Properties : new List<DataStructures.GameData.Property>();
            foreach (IGrouping<string, DataStructures.GameData.Property> gdProps in gameDataProps.Where(x => x.Name != "spawnflags").GroupBy(x => x.Name))
            {
                DataStructures.GameData.Property gdProp = gdProps.First();
                List<string> vals = props.Where(x => x.Key == gdProp.Name).Select(x => x.Value).Distinct().ToList();
                string value = vals.Count == 0 ? gdProp.DefaultValue : (vals.Count == 1 ? vals.First() : "<multiple values>" + String.Join(", ", vals));
                list.Add(new TableValue { Class = className, OriginalKey = gdProp.Name, NewKey = gdProp.Name, Value = value });
            }
            foreach (IGrouping<string, Property> group in props.Where(x => gameDataProps.All(y => x.Key != y.Name)).GroupBy(x => x.Key))
            {
                List<string> vals = @group.Select(x => x.Value).Distinct().ToList();
                string value = vals.Count == 1 ? vals.First() : Local.LocalString("object_properties.multiple_values", String.Join(", ", vals));
                list.Add(new TableValue { Class = className, OriginalKey = @group.Key, NewKey = @group.Key, Value = value });
            }
            return list;
        }
    }
}