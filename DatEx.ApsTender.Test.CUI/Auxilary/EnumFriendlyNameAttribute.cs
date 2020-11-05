using System;
using System.Reflection;

namespace DatEx.ApsTender.Test.CUI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FriendlyNameAttribute : Attribute
    {
        public String Value { get; set; }

        public FriendlyNameAttribute(String value)
        {
            Value = value;
        }
    }

    public static class Ext_EnumFriendlyName
    {
        public static string GetFriendlyName(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    FriendlyNameAttribute attr = Attribute.GetCustomAttribute(field, typeof(FriendlyNameAttribute)) as FriendlyNameAttribute;
                    if (attr != null) return attr.Value;
                }
            }
            return null;
        }
    }
}
