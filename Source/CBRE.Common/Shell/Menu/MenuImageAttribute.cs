using System;
using System.Drawing;
using System.Reflection;

namespace CBRE.Common.Shell.Menu
{
    /// <summary>
    /// An attribute that can be attached to a class to indicate the class' menu item.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MenuImageAttribute : Attribute
    {
        public Type ResourceType { get; set; }
        public string ResourceName { get; set; }
        public Image Image => GetResource();

        public MenuImageAttribute(Type resourceType, string resourceName)
        {
            ResourceType = resourceType;
            ResourceName = resourceName;
        }

        private Image GetResource()
        {
            PropertyInfo prop = ResourceType?.GetProperty(ResourceName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            if (prop == null || !typeof(Image).IsAssignableFrom(prop.PropertyType)) return null;
            return prop.GetValue(null) as Image;
        }
    }
}