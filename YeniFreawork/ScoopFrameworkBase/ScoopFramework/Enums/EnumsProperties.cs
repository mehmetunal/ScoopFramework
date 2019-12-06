using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ScoopFreanwork
{
    public class Item 
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string EnumKey { get; set; }
    }
    public class EnumsProperties
    {
        public static IEnumerable<Item> EnumToArrayValues<T>()
        {
            foreach (var item in System.Enum.GetValues(typeof(T)))
            {
                var deger = (int)item;
                yield return new Item { Key = deger.ToString(), Value = GetDescriptionFromEnumValue((System.Enum)item), EnumKey = item.ToString() };
            }
        }
        public static string GetDescriptionFromEnumValue(System.Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }
        public static IEnumerable<Item> EnumToArray<T>()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in System.Enum.GetValues(typeof(T)))
            {
                var deger = item.ToString().Replace("_", "");
                yield return new Item { Key = deger.ToString(), Value = GetDescriptionFromEnumValue((System.Enum)item) };
            }
        }
    }
}
