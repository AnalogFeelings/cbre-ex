using CBRE.DataStructures.GameData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace CBRE.Providers.GameData
{
    [Export("Blitz", typeof(IGameDataProvider))]
    public class BlitzGameDataProvider : IGameDataProvider
    {
        public DataStructures.GameData.GameData GetGameDataFromFiles(IEnumerable<string> files)
        {
            DataStructures.GameData.GameData gameData = new DataStructures.GameData.GameData();

            gameData.MapSizeHigh = 16384;
            gameData.MapSizeLow = -16384;

            // TODO: Add error reporting.
            foreach(string file in files.Where(IsValidForFile))
            {
                string fileName = Path.GetFileName(file);
                CustomEntity customEntity;

                try
                {
                    string jsonContent = File.ReadAllText(file);

                    customEntity = JsonConvert.DeserializeObject<CustomEntity>(jsonContent);

                    if (customEntity == null) continue;
                }
                catch (Exception)
                {
                    continue;
                }

                if(string.IsNullOrWhiteSpace(customEntity.Name)) continue;
                if(gameData.Classes.Any(x => x.Name == customEntity.Name)) continue;

                GameDataObject dataObject = new GameDataObject(customEntity.Name, customEntity.Description, ClassType.Point, true);

                foreach(CustomEntityProperty property in customEntity.Properties)
                {
                    if(string.IsNullOrWhiteSpace(property.Name)) continue;
                    if (!Enum.TryParse(property.Type, out VariableType type)) continue;

                    Property realProperty = new Property(property.Name, type)
                    {
                        ShortDescription = property.SmartEditName,
                        DefaultValue = property.DefaultValue,
                        Description = property.HelpText
                    };

                    dataObject.Properties.Add(realProperty);
                }

                if(!string.IsNullOrWhiteSpace(customEntity.Sprite)) 
                    dataObject.Behaviours.Add(new Behaviour("sprite", customEntity.Sprite));

                if(customEntity.UseModelRendering)
                {
                    if(!customEntity.Properties.Any(x => x.Name == "file"))
                    {

                    }
                    else
                    {
                        dataObject.Behaviours.Add(new Behaviour("usemodels"));
                    }
                }

                gameData.Classes.Add(dataObject);
            }

            GameDataObject lightDataObj = new GameDataObject("light", "Point light source.", ClassType.Point);
            lightDataObj.Properties.Add(new Property("color", VariableType.Color255) { ShortDescription = "Color", DefaultValue = "255 255 255" });
            lightDataObj.Properties.Add(new Property("intensity", VariableType.Float) { ShortDescription = "Intensity", DefaultValue = "1.0" });
            lightDataObj.Properties.Add(new Property("range", VariableType.Float) { ShortDescription = "Range", DefaultValue = "1.0" });
            lightDataObj.Properties.Add(new Property("hassprite", VariableType.Bool) { ShortDescription = "Has sprite", DefaultValue = "Yes" });
            lightDataObj.Behaviours.Add(new Behaviour("sprite", "ent_light.png"));
            gameData.Classes.Add(lightDataObj);

            GameDataObject spotlightDataObj = new GameDataObject("spotlight", "Self-explanatory.", ClassType.Point);
            spotlightDataObj.Properties.Add(new Property("color", VariableType.Color255) { ShortDescription = "Color", DefaultValue = "255 255 255" });
            spotlightDataObj.Properties.Add(new Property("intensity", VariableType.Float) { ShortDescription = "Intensity", DefaultValue = "1.0" });
            spotlightDataObj.Properties.Add(new Property("range", VariableType.Float) { ShortDescription = "Range", DefaultValue = "1.0" });
            spotlightDataObj.Properties.Add(new Property("hassprite", VariableType.Bool) { ShortDescription = "Has sprite", DefaultValue = "Yes" });
            spotlightDataObj.Properties.Add(new Property("innerconeangle", VariableType.Float) { ShortDescription = "Inner cone angle", DefaultValue = "45" });
            spotlightDataObj.Properties.Add(new Property("outerconeangle", VariableType.Float) { ShortDescription = "Outer cone angle", DefaultValue = "90" });
            spotlightDataObj.Properties.Add(new Property("angles", VariableType.Vector) { ShortDescription = "Rotation", DefaultValue = "0 0 0" });
            spotlightDataObj.Behaviours.Add(new Behaviour("sprite", "ent_spot.png"));
            gameData.Classes.Add(spotlightDataObj);

            GameDataObject noShadowObj = new GameDataObject("noshadow", "Disables shadow casting for this brush.", ClassType.Solid);
            gameData.Classes.Add(noShadowObj);

            Property p = new Property("position", VariableType.Vector) { ShortDescription = "Position", DefaultValue = "0 0 0" };
            foreach (GameDataObject dataObject in gameData.Classes)
            {
                if (dataObject.ClassType != ClassType.Solid)
                {
                    dataObject.Properties.Add(p);
                }
            }

            return gameData;
        }

        public bool IsValidForFile(string filename)
        {
            return File.Exists(filename) && filename.EndsWith(".json");
        }
    }
}
