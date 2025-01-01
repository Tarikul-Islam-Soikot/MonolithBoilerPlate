using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MonolithBoilerPlate.Common.Extensions
{
    public static class EnumExtension
    {
        public static List<EnumProperty> GetKeyValues(Type enumType)
        {
            var data = new List<EnumProperty>();

            if (!enumType.IsEnum)
                return data;

            
            foreach (var e in Enum.GetValues(enumType))
            {
                var val = GetDescription(enumType, e.ToString());
                data.Add(new EnumProperty
                {
                    Id = Convert.ToInt32(e),
                    Value = val ?? string.Join(" ", Regex.Split(e.ToString(), @"([A-Z]?[a-z]+)").Where(str => !string.IsNullOrEmpty(str)))
                });
            }
            return data;
        }

        public static string GetDescription(Type enumType, string name)
        {
            var fieldInfo = enumType.GetField(name);
            if (fieldInfo != null)
            {
                var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                return descriptionAttribute?.Description;
            }
            return null;
        }

        public static string GetEnumDescription(this Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                if (fi == null) return "Enum type not Found";

                DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                if (attributes != null && attributes.Any())
                {
                    return attributes[0].Description;
                }

                return string.Join(" ", Regex.Split(value.ToString(), @"([A-Z]?[a-z]+)").Where(str => !string.IsNullOrEmpty(str)));
            }
            catch (Exception)
            {
                return "Enum type not Found";
            }
        }

        public static int ParseEnum<T>(string key)
        {
            return (int)Enum.Parse(typeof(T), key, true);
        }

        public class EnumProperty
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

        public static IEnumerable<string> GetDescriptions(Type type)
        {
            var descs = new List<string>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds)
                {
                    descs.Add(fd.Description);
                }
            }
            return descs;
        }

        public static List<int> GetValuesFromDescription<T>(string description)
        {
            var Ids = new List<int>();
            var type = typeof(T);

            if (!type.IsEnum)
                throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                if (field.Name.ToLower() == "value__")
                    continue;

                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description.ToLower().Contains(description.ToLower()))
                    {
                        var name = (T)field.GetValue(null);
                        var Id = Convert.ToInt32(name);
                        Ids.Add(Id);
                    }
                }
                else
                {
                    var fieldName = string.Join(" ", Regex.Split(field.Name.ToString(), @"([A-Z]?[a-z]+)").Where(str => !string.IsNullOrEmpty(str)));
                    if (fieldName.ToLower().Contains(description.ToLower()))
                    {
                        var name = (T)field.GetValue(null);
                        var Id = Convert.ToInt32(name);
                        Ids.Add(Id);
                    }
                }
            }
            return Ids;
        }

        public static List<EnumProperty> GetEnumKeyValueList(this List<Enum> values)
        {
            var data = new List<EnumProperty>();

            foreach (var e in values)
            {
                data.Add(new EnumProperty
                {
                    Id = Convert.ToInt32(e),
                    Value = GetEnumDescription(e)
                });
            }

            return data;
        }

        public static string ToEnumString<T>(this string enumValue)
        {
            try
            {
                var value = Convert.ToInt32(enumValue);
                return Enum.GetName(typeof(T), value);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
