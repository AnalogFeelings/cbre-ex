using CBRE.Localization;
using CBRE.Settings;
using System;
using System.ComponentModel;

namespace CBRE.Editor.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            if (value is DoubleClick3DAction action)
            {
                switch (action)
                {
                    case DoubleClick3DAction.Nothing:
                        return Local.LocalString("setting.interact.double_click_action.nothing");
                    case DoubleClick3DAction.ObjectProperties:
                        return Local.LocalString("setting.interact.double_click_action.object_properties");
                    default:
                        return Local.LocalString("setting.interact.double_click_action.texture_tool");
                }
            }
            else if (value is AntiAliasingOption option)
            {
                switch (option)
                {
                    case AntiAliasingOption.None:
                        return Local.LocalString("setting.render.antialising.no");
                    case AntiAliasingOption.TwoSamples:
                        return Local.LocalString("setting.render.antialising.two");
                    case AntiAliasingOption.FourSamples:
                        return Local.LocalString("setting.render.antialising.four");
                    case AntiAliasingOption.EightSamples:
                        return Local.LocalString("setting.render.antialising.eight");
                    default:
                        return Local.LocalString("setting.render.antialising.sixteen");
                }
            }
            else
            {
                System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return attributes.Length > 0 ? attributes[0].Description : value.ToString();
            }
        }

        public static T FromDescription<T>(string description) where T : struct, IConvertible
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException(Local.LocalString("exception.not_enumerated_type"));
            }

            foreach (T item in Enum.GetValues(type))
            {
                System.Reflection.FieldInfo fi = type.GetField(item.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string desc = attributes.Length > 0 ? attributes[0].Description : item.ToString();
                if (desc == description) return item;
            }
            return default(T);
        }
    }
}
