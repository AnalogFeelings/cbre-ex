using CBRE.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CBRE.DataStructures.GameData
{
    public class GameData
    {
        public int MapSizeLow { get; set; }
        public int MapSizeHigh { get; set; }
        public List<GameDataObject> Classes { get; private set; }
        public List<string> Includes { get; private set; }
        public List<string> MaterialExclusions { get; private set; }
        public List<AutoVisgroupSection> AutoVisgroups { get; private set; }

        public List<string> CustomEntityErrors { get; private set; }

        public GameData()
        {
            MapSizeHigh = 16384;
            MapSizeLow = -16384;
            Classes = new List<GameDataObject>();

            CustomEntityErrors = new List<string>();

            IEnumerable<string> jsonFiles = Directory.EnumerateFiles("Entities", "*.json");
            foreach (string jsonFile in jsonFiles)
            {
                string jsonFilename = Path.GetFileName(jsonFile);
                CustomEntity customEntity;

                try
                {
                    string jsonContent = File.ReadAllText(jsonFile);
                    customEntity = JsonConvert.DeserializeObject<CustomEntity>(jsonContent);

                    if (customEntity == null)
                    {
                        CustomEntityErrors.Add(Local.LocalString("error.data.deserialize_into_entity", jsonFilename));
                        continue;
                    }
                }
                catch (Exception)
                {
                    CustomEntityErrors.Add(Local.LocalString("error.data.deserialize_into_entity", jsonFilename));
                    continue;
                }

                if (string.IsNullOrEmpty(customEntity.Name))
                {
                    CustomEntityErrors.Add(Local.LocalString("error.data.no_valid_name", jsonFilename));

                    continue;
                }
                if (Classes.Any(x => x.Name == customEntity.Name))
                {
                    CustomEntityErrors.Add(Local.LocalString("error.data.already_exist", jsonFilename, customEntity.Name));

                    continue;
                }

                GameDataObject gameDataObj = new GameDataObject(customEntity.Name, customEntity.Description, ClassType.Point, true);
                foreach (CustomEntityProperty customProperty in customEntity.Properties)
                {
                    if (string.IsNullOrEmpty(customProperty.Name))
                    {
                        CustomEntityErrors.Add(Local.LocalString("error.data.property_no_valid_name", jsonFilename));

                        continue;
                    }
                    if (!Enum.TryParse(customProperty.Type, out VariableType varType))
                    {
                        CustomEntityErrors.Add(Local.LocalString("error.data.property_cant_read_type", jsonFilename, customProperty.Name));

                        continue;
                    }

                    Property actualProperty = new Property(customProperty.Name, varType)
                    {
                        ShortDescription = customProperty.SmartEditName,
                        DefaultValue = customProperty.DefaultValue,
                        Description = customProperty.HelpText
                    };

                    gameDataObj.Properties.Add(actualProperty);
                }

                if(!string.IsNullOrWhiteSpace(customEntity.Sprite)) gameDataObj.Behaviours.Add(new Behaviour("sprite", customEntity.Sprite));
                if (customEntity.UseModelRendering)
                {
                    if (!customEntity.Properties.Any(x => x.Name == "file"))
                    {
                        CustomEntityErrors.Add(Local.LocalString("error.data.file_property_missing", jsonFilename));
                    }
                    else
                    {
                        gameDataObj.Behaviours.Add(new Behaviour("useModels"));
                    }
                }

                Classes.Add(gameDataObj);
            }

            GameDataObject lightDataObj = new GameDataObject("light", Local.LocalString("data.light"), ClassType.Point);
            lightDataObj.Properties.Add(new Property("color", VariableType.Color255) { ShortDescription = Local.LocalString("data.light.color"), DefaultValue = "255 255 255" });
            lightDataObj.Properties.Add(new Property("intensity", VariableType.Float) { ShortDescription = Local.LocalString("data.light.intensity"), DefaultValue = "1.0" });
            lightDataObj.Properties.Add(new Property("range", VariableType.Float) { ShortDescription = Local.LocalString("data.light.range"), DefaultValue = "1.0" });
            lightDataObj.Properties.Add(new Property("hassprite", VariableType.Bool) { ShortDescription = Local.LocalString("data.light.hassprite"), DefaultValue = "Yes" });
            lightDataObj.Behaviours.Add(new Behaviour("sprite", "sprites/lightbulb"));
            Classes.Add(lightDataObj);

            GameDataObject spotlightDataObj = new GameDataObject("spotlight", Local.LocalString("data.spotlight"), ClassType.Point);
            spotlightDataObj.Properties.Add(new Property("color", VariableType.Color255) { ShortDescription = Local.LocalString("data.light.color"), DefaultValue = "255 255 255" });
            spotlightDataObj.Properties.Add(new Property("intensity", VariableType.Float) { ShortDescription = Local.LocalString("data.spotlight.intensity"), DefaultValue = "1.0" });
            spotlightDataObj.Properties.Add(new Property("range", VariableType.Float) { ShortDescription = Local.LocalString("data.spotlight.range"), DefaultValue = "1.0" });
            spotlightDataObj.Properties.Add(new Property("hassprite", VariableType.Bool) { ShortDescription = Local.LocalString("data.spotlight.hassprite"), DefaultValue = "Yes" });
            spotlightDataObj.Properties.Add(new Property("innerconeangle", VariableType.Float) { ShortDescription = Local.LocalString("data.spotlight.innerconeangle"), DefaultValue = "45" });
            spotlightDataObj.Properties.Add(new Property("outerconeangle", VariableType.Float) { ShortDescription = Local.LocalString("data.spotlight.outerconeangle"), DefaultValue = "90" });
            spotlightDataObj.Properties.Add(new Property("angles", VariableType.Vector) { ShortDescription = Local.LocalString("data.model.angles"), DefaultValue = "0 0 0" });
            spotlightDataObj.Behaviours.Add(new Behaviour("sprite", "sprites/spotlight"));
            Classes.Add(spotlightDataObj);

            GameDataObject waypointDataObj = new GameDataObject("waypoint", Local.LocalString("data.waypoint"), ClassType.Point);
            waypointDataObj.Behaviours.Add(new Behaviour("sprite", "sprites/waypoint"));
            Classes.Add(waypointDataObj);

            GameDataObject soundEmitterDataObj = new GameDataObject("soundemitter", Local.LocalString("data.soundemitter"), ClassType.Point);
            soundEmitterDataObj.Properties.Add(new Property("sound", VariableType.Integer) { ShortDescription = Local.LocalString("data.soundemitter.sound"), DefaultValue = "1" });
            soundEmitterDataObj.Behaviours.Add(new Behaviour("sprite", "sprites/speaker"));
            Classes.Add(soundEmitterDataObj);

            GameDataObject modelDataObj = new GameDataObject("model", Local.LocalString("data.model"), ClassType.Point);
            modelDataObj.Properties.Add(new Property("file", VariableType.String) { ShortDescription = Local.LocalString("data.model.file"), DefaultValue = "" });
            modelDataObj.Properties.Add(new Property("angles", VariableType.Vector) { ShortDescription = Local.LocalString("data.model.angles"), DefaultValue = "0 0 0" });
            modelDataObj.Properties.Add(new Property("scale", VariableType.Vector) { ShortDescription = Local.LocalString("data.model.scale"), DefaultValue = "1 1 1" });
            modelDataObj.Behaviours.Add(new Behaviour("sprite", "sprites/model"));
            modelDataObj.Behaviours.Add(new Behaviour("useModels"));
            Classes.Add(modelDataObj);

            GameDataObject screenDataObj = new GameDataObject("screen", Local.LocalString("data.screen"), ClassType.Point);
            screenDataObj.Properties.Add(new Property("imgpath", VariableType.String) { ShortDescription = Local.LocalString("data.screen.imgpath"), DefaultValue = "" });
            screenDataObj.Behaviours.Add(new Behaviour("sprite", "sprites/screen"));
            Classes.Add(screenDataObj);

            GameDataObject noShadowObj = new GameDataObject("noshadow", Local.LocalString("data.noshadow"), ClassType.Solid);
            Classes.Add(noShadowObj);

            Property p = new Property("position", VariableType.Vector) { ShortDescription = Local.LocalString("data.position"), DefaultValue = "0 0 0" };
            foreach (GameDataObject gdo in Classes)
            {
                if (gdo.ClassType != ClassType.Solid)
                {
                    gdo.Properties.Add(p);
                }
            }

            Includes = new List<string>();
            MaterialExclusions = new List<string>();
            AutoVisgroups = new List<AutoVisgroupSection>();
        }

        public void CreateDependencies()
        {
            List<string> resolved = new List<string>();
            List<GameDataObject> unresolved = new List<GameDataObject>(Classes);
            while (unresolved.Any())
            {
                List<GameDataObject> resolve = unresolved.Where(x => x.BaseClasses.All(resolved.Contains)).ToList();
                if (!resolve.Any()) throw new Exception(Local.LocalString("exception.circular_dependencies", String.Join(", ", unresolved.Select(x => x.Name))));
                resolve.ForEach(x => x.Inherit(Classes.Where(y => x.BaseClasses.Contains(y.Name))));
                unresolved.RemoveAll(resolve.Contains);
                resolved.AddRange(resolve.Select(x => x.Name));
            }
        }

        public void RemoveDuplicates()
        {
            foreach (IGrouping<string, GameDataObject> g in Classes.Where(x => x.ClassType != ClassType.Base).GroupBy(x => x.Name.ToLowerInvariant()).Where(g => g.Count() > 1).ToList())
            {
                foreach (GameDataObject obj in g.Skip(1)) Classes.Remove(obj);
            }
        }
    }
}
