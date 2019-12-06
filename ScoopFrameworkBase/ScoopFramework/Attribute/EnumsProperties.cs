using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ScoopFramework.Attribute
{
    public static class EnumsProperties
    {
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


        public static IEnumerable<Item> EnumToArrayValues<T>()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in System.Enum.GetValues(typeof(T)))
            {

                var deger = (int)item;

                yield return new Item { Key = deger.ToString(), Value = GetDescriptionFromEnumValue((System.Enum)item) };


            }
        }


        public static string EnumToArrayValuesSearch<T>(int? param)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in System.Enum.GetValues(typeof(T)))
            {

                var deger = (int)item;
                if (param == deger)
                {
                    return GetDescriptionFromEnumValue((System.Enum)item);
                }
            }
            return string.Empty;
        }


        public static IEnumerable<AttriubteItem> EnumToArrayGenericValues<T>()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in System.Enum.GetValues(typeof(T)))
            {

                var deger = (int)item;

                yield return new AttriubteItem { Key = deger.ToString(), Value = GetStringGenericValue((System.Enum)item) };


            }
        }


        public static BreadCrump_AuthorityAction GetStringGenericValue(System.Enum value)
        {
            BreadCrump_AuthorityAction attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(BreadCrump_AuthorityAction), false)
                .SingleOrDefault() as BreadCrump_AuthorityAction;
            return attribute;
        }
    }
    //public class ComboItem
    //{
    //    public IEnumerable<Item> item { get; set; }

    //}
    public class Item
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AttriubteItem
    {
        public string Key { get; set; }
        public BreadCrump_AuthorityAction Value { get; set; }
    }
    //public class Title : System.Attribute
    //{
    //    private string _value;
    //    public Title(string value)
    //    {
    //        _value = value;
    //    }
    //    public string Value
    //    {
    //        get { return _value; }
    //    }
    //}

    //public class SubValue : System.Attribute
    //{
    //    private int _value;
    //    public SubValue(int value)
    //    {
    //        _value = value;
    //    }
    //    public int Value
    //    {
    //        get { return _value; }
    //    }
    //}

    public class BreadCrump_AuthorityAction : System.Attribute
    {
        private string _authorityActionText;
        private string _breadCrumpText;
        private int _parentActionCode;
        public bool _showOnAuthorityActions { get; set; }

        public BreadCrump_AuthorityAction(
            string breadCrumpText,
            string authorityActionText,
            int parentActionCode = 0,
            bool showOnAuthorityActions = false)
        {
            _authorityActionText = authorityActionText;
            _breadCrumpText = breadCrumpText;
            _parentActionCode = parentActionCode;
            _showOnAuthorityActions = showOnAuthorityActions;
        }

        public string BreadCrumpText { get { return _breadCrumpText; } }
        public string AuthorityActionText { get { return _authorityActionText; } }
        public int ParentActionCode { get { return _parentActionCode; } }
        public bool ShowOnAuthorityActions { get { return _showOnAuthorityActions; } }

    }


}
