using System.Reflection;

namespace CBRE.Common.Shell.Settings
{
    /// <summary>
    /// Common extensions for settings stores
    /// </summary>
    public static class SettingStoreExtensions
    {
        /// <summary>
        /// Store fields and properties from an instance marked with <see cref="SettingAttribute" /> into the store.
        /// </summary>
        /// <param name="store">The settings store</param>
        /// <param name="instance">The instance to store</param>
        public static void StoreInstance(this ISettingsStore store, object instance)
        {
            if (instance == null) return;
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            foreach (MemberInfo mem in instance.GetType().GetMembers(flags))
            {
                SettingAttribute sa = mem.GetCustomAttribute<SettingAttribute>();
                if (sa == null) continue;

                string name = sa.SettingName ?? mem.Name;

                PropertyInfo prop = mem as PropertyInfo;
                FieldInfo field = mem as FieldInfo;

                if (prop != null) store.Set(name, prop.GetValue(instance));
                else if (field != null) store.Set(name, field.GetValue(instance));
            }
        }

        /// <summary>
        /// Load values from the store into fields and properties in an instance marked with <see cref="SettingAttribute" />.
        /// </summary>
        /// <param name="store">The settings store</param>
        /// <param name="instance">The instance to load</param>
        public static void LoadInstance(this ISettingsStore store, object instance)
        {
            if (instance == null) return;
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            foreach (MemberInfo mem in instance.GetType().GetMembers(flags))
            {
                SettingAttribute sa = mem.GetCustomAttribute<SettingAttribute>();
                if (sa == null) continue;

                string name = sa.SettingName ?? mem.Name;

                PropertyInfo prop = mem as PropertyInfo;
                FieldInfo field = mem as FieldInfo;

                if (prop != null) prop.SetValue(instance, store.Get(prop.PropertyType, name, prop.GetValue(instance)));
                else if (field != null) field.SetValue(instance, store.Get(field.FieldType, name, field.GetValue(instance)));
            }
        }
    }
}