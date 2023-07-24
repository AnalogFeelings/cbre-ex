using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Environment.Blitz
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

                TextureDirectories = GetVal(environment.Properties, "TextureDirectories", "").Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
                ModelDirectories = GetVal(environment.Properties, "ModelDirectories", "").Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
                DefaultPointEntity = GetVal(environment.Properties, "DefaultPointEntity", ""),
                DefaultBrushEntity = GetVal(environment.Properties, "DefaultBrushEntity", ""),

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
                    { "TextureDirectories", string.Join(";", env.TextureDirectories) },
                    { "ModelDirectories", string.Join(";", env.ModelDirectories) },
                    { "DefaultPointEntity", env.DefaultPointEntity },
                    { "DefaultBrushEntity", env.DefaultBrushEntity },

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