using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Environment.ContainmentBreach
{
    [Export(typeof(IEnvironmentFactory))]
    [AutoTranslate]
    public class BlitzEnvironmentFactory : IEnvironmentFactory
    {
        public Type Type => typeof(BlitzEnvironment);
        public string TypeName => "BlitzEnvironment";
        public string Description { get; set; } = "Blitz3D";

        private T GetVal<T>(Dictionary<string, string> dictionary, string key, T def = default(T))
        {
            if (dictionary.TryGetValue(key, out var val))
            {
                try
                {
                    return (T) Convert.ChangeType(val, typeof(T), CultureInfo.InvariantCulture);
                }
                catch
                {
                    //
                }
            }
            return def;
        }

        public IEnvironment Deserialise(SerialisedEnvironment environment)
        {
            var gse = new BlitzEnvironment()
            {
                ID = environment.ID,
                Name = environment.Name,
                GraphicsDirectory = GetVal(environment.Properties, "GraphicsDirectory", ""),
                GameDirectory = GetVal(environment.Properties, "GameDirectory", ""),

                FgdFiles = GetVal(environment.Properties, "FgdFiles", "").Split(';').Where(x => !String.IsNullOrWhiteSpace(x)).ToList(),
                DefaultPointEntity = GetVal(environment.Properties, "DefaultPointEntity", ""),
                DefaultBrushEntity = GetVal(environment.Properties, "DefaultBrushEntity", ""),
                OverrideMapSize = GetVal(environment.Properties, "OverrideMapSize", false),
                MapSizeLow = GetVal(environment.Properties, "MapSizeLow", -4096m),
                MapSizeHigh = GetVal(environment.Properties, "MapSizeHigh", 4096m),

                DefaultTextureScale = GetVal(environment.Properties, "DefaultTextureScale", 1m),
            };
            return gse;
        }

        public SerialisedEnvironment Serialise(IEnvironment environment)
        {
            var env = (BlitzEnvironment) environment;
            var se = new SerialisedEnvironment
            {
                ID = environment.ID,
                Name = environment.Name,
                Type = TypeName,
                Properties =
                {
                    { "GameDirectory", env.GameDirectory },
                    { "GraphicsDirectory", env.GraphicsDirectory },

                    { "FgdFiles", String.Join(";", env.FgdFiles) },
                    { "DefaultPointEntity", env.DefaultPointEntity },
                    { "DefaultBrushEntity", env.DefaultBrushEntity },
                    { "OverrideMapSize", Convert.ToString(env.OverrideMapSize, CultureInfo.InvariantCulture) },
                    { "MapSizeLow", Convert.ToString(env.MapSizeLow, CultureInfo.InvariantCulture) },
                    { "MapSizeHigh", Convert.ToString(env.MapSizeHigh, CultureInfo.InvariantCulture) },

                    { "DefaultTextureScale", Convert.ToString(env.DefaultTextureScale, CultureInfo.InvariantCulture) }
                }
            };
            return se;
        }

        public IEnvironment CreateEnvironment()
        {
            return new BlitzEnvironment();
        }

        public IEnvironmentEditor CreateEditor()
        {
            return new BlitzEnvironmentEditor();
        }
    }
}