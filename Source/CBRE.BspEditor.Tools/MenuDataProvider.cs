using System.Collections.Generic;
using System.ComponentModel.Composition;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Tools
{
    [AutoTranslate]
    [Export(typeof(IMenuMetadataProvider))]
    public class MenuDataProvider : IMenuMetadataProvider
    {
        public string Map { get; set; } = "Map";

        public IEnumerable<MenuSection> GetMenuSections()
        {
            yield return new MenuSection("Map", Map, "F");
        }

        public IEnumerable<MenuGroup> GetMenuGroups()
        {
            yield return new MenuGroup("Map", "", "Grid", "B");
            yield return new MenuGroup("Map", "", "GridTypes", "C");
            yield return new MenuGroup("Map", "", "Grouping", "F");
            yield return new MenuGroup("Map", "", "Texture", "H");
            yield return new MenuGroup("Map", "", "Info", "J");

            yield return new MenuGroup("Tools", "", "Cordon", "F");
            yield return new MenuGroup("Tools", "", "Texture", "I");
        }
    }
}