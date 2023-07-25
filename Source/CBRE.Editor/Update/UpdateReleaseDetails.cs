using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CBRE.Editor.Update
{
    public class UpdateReleaseDetails
    {
        public string Tag { get; }
        public string Name { get; }
        public string Changelog { get; }
        public string FileName { get; }
        public string DownloadUrl { get; }

        public bool Exists => Tag != null;

        public UpdateReleaseDetails(string jsonString)
        {
            JArray obj = JsonConvert.DeserializeObject(jsonString) as JArray;
            if (obj == null || obj.Count < 1) return;

            JObject rel = obj[0] as JObject;
            JArray assets = rel?.GetValue("assets") as JArray;
            if (assets == null || assets.Count < 1) return;

            JObject exeAsset = assets.FirstOrDefault(x => x is JObject o && o.GetValue("name").ToString().EndsWith(".exe")) as JObject;
            if (exeAsset == null) return;

            Tag = rel.GetValue("tag_name").ToString();
            Name = rel.GetValue("name").ToString();
            Changelog = rel.GetValue("body").ToString();
            FileName = exeAsset.GetValue("name").ToString();
            DownloadUrl = exeAsset.GetValue("url").ToString();
        }
    }
}
