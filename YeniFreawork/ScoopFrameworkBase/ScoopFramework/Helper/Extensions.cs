using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc.Html;
using ScoopFramework.Entity;
using System.Web.Script.Serialization;
using System.Net;
using System.Security.Permissions;
using System.Collections.Generic;
using System.ComponentModel;
using ScoopFramework.Model;
using System.Web.Mvc;

namespace ScoopFramework.Helper
{
    public struct Validations
    {
        public static ValidationUI UserName(bool? required = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9._-]+",
                data_error = "Yanlızca sayı ve türkçe karakter içermeyen harf girişi gerçekleştirebilirsiniz.. ( 6 - 30 Karakter )",
                minlength = 6,
                maxlength = 30
            };
        }

        public static ValidationUI Text09(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9]+",
                data_error = "Lütfen Özel karakter kullanmadan ve birleşik giriş yapınız..",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI Text09Space(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ]+",
                //data_pattern_error = "Lütfen Özel karakter kullanmadan giriş yapınız...",
                data_error = "Lütfen Özel karakter kullanmadan giriş yapınız...",
                minlength = minLength,
                maxlength = maxLength

            };
        }

        public static ValidationUI TextEverywhere(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöıâÂ:;|\\-_/.,&*'?+()’<>= ]+",
                data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI MalzemeAdi(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöıâÂ\\-/*]+",
                data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI Unvan(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöı.,]+",
                data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextTurkce(bool? required = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-ZÇŞĞÜÖİâÂçşğüöı]+",
                data_error = "Lütfen Yalnızca Alfabetik Giriş Yapınız.."
            };
        }

        public static ValidationUI TextTurkceSpace(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-ZÇŞĞÜÖâÂİçşğüöı .]+",
                data_error = "Lütfen Yalnızca Alfabetik Giriş Yapınız..",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextTurkce09(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9ÇŞĞÜÖİçşğüâÂöı]+",
                data_pattern_error = "Lütfen Özel karakter kullanmadan birleşik giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextTurkceSpace09(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöıâÂ]+",
                //data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                data_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI Text09Double(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöıâÂ,.-:;]+",
                //data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                data_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI IP(bool? required = null, string error = "Geçerli bir adres girilmedi.")
        {
            return new ValidationUI
            {
                pattern = "\\b\\d{1,3}.\\b\\d{1,3}.\\b\\d{1,3}.\\b\\d{1,3}",
                data_error = error,
                required = (required == true),
                maxlength = 15
            };
        }

        public static ValidationUI NumberOnly(bool? required = null, Int32? minLength = 0, Int32? maxLength = 12, string error = "Sadece sayı girişi gerçekleştiriniz..")
        {
            return new ValidationUI
            {
                pattern = "[0-9]+",
                data_error = error,
                required = (required == true),
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI Range(Int32? Min, Int32? Max, bool? required = null)
        {
            return new ValidationUI
            {
                type = "number",
                min = Min,
                max = Max,
                required = (required == true),
            };
        }

        public static ValidationUI TelefonNo(bool? required = null)
        {
            return new ValidationUI
            {
                maxlength = 13,
                minlength = 11,
                pattern = "([+][0-9])?(0)[0-9]{10}",
                data_error = "Telefon Numarası Sayısal Karakterlerden Oluşmalıdır. ( 0 ile beraber )",
                required = (required == true),
            };
        }

        public static ValidationUI TelefonNo2(bool? required = null)
        {
            return new ValidationUI
            {
                maxlength = 13,
                minlength = 10,
                pattern = "([+][0-9])?[0-9]{10}",
                data_error = "Telefon Numarası Sayısal Karakterlerden Oluşmalıdır.(0 olmadan)",
                required = (required == true),
            };
        }

        public static ValidationUI VergiNo(bool? required = null)
        {
            return new ValidationUI
            {
                minlength = 10,
                maxlength = 11,
                pattern = "[0-9]+",
                data_error = "Vergi Numarası En Az 10 En Fazla 11 Sayısal Karakterden Oluşmalıdır.",
                required = (required == true),
            };
        }

        public static ValidationUI IhaleNo(bool? required = null)
        {
            return new ValidationUI
            {
                minlength = 10,
                maxlength = 11,
                pattern = "[0-9]*[.][0-9]*[.][0-9]*",
                data_error = "İhale Numarası En Az 10 En Fazla 11 Sayısal Karakterden Oluşmalıdır.",
                required = (required == true),
            };
        }

        public static ValidationUI TCKimlik = new ValidationUI
        {
            maxlength = 11,
            minlength = 11,
            pattern = "[0-9]+",
            data_error = "Kimlik Numarası Sayısal Karakterlerden Oluşmalıdır. ( 11 Karakter )",
            required = true,
        };

        public static ValidationUI Required = new ValidationUI { required = true, };

        public static ValidationUI Yuzde(bool? required = null)
        {
            return new ValidationUI
            {
                type = "number",
                pattern = "(^(?:100|[1-9]?[0-9])$)",
                maxlength = 3,
                data_error = "Lütfen 0 ile 100 arasında bir değer giriniz.",
                data_pattern_error = "Lütfen 0 ile 100 arasında bir değer giriniz.",
                required = (required == true)
            };
        }

        public static ValidationUI Number(bool? required = null, Int32? minLength = 0, Int32? maxLength = 12)
        {
            return new ValidationUI
            {
                pattern = "[-+]?[0-9]*(?:.|,)?[0-9]+",
                data_error = "Geçerli bir sayı girilmedi.",
                required = (required == true),
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI EMail(bool? required = null)
        {
            return new ValidationUI
            {
                type = "email",
                data_error = "Geçerli bir mail adresi girilmedi",
                maxlength = 100,
                minlength = 5,
                required = (required == true)
            };
        }

        public static ValidationUI Password(bool? required = null)
        {
            return new ValidationUI
            {
                autocomplete = "off",
                type = "password",
                maxlength = 14,
                minlength = 6,
                pattern = "[0-9a-zA-Z.,*!-_&]+",
                data_pattern_error = "Geçerli bir şifre girilmedi. ( 6 - 14 Karakter )",
                required = (required == true)
            };
        }

        public static ValidationUI Password2(bool? required = null)
        {
            return new ValidationUI
            {
                autocomplete = "off",
                type = "password",
                maxlength = 100,
                minlength = 0,
                pattern = "[0-9a-zA-Z.,*!-_&]+",
                data_pattern_error = "Geçerli bir şifre girilmedi. ",
                required = (required == true)
            };
        }

        public static ValidationUI PasswordMatch(string Match, bool? required = null)
        {
            return new ValidationUI
            {
                data_error = "Malesef Şifreler eşleşmiyor...",
                data_pattern_error = "Malesef Şifreler eşleşmiyor...",
                data_match = Match,
                required = (required == true),
                type = Password(required).type,
                pattern = Password(required).pattern,
                maxlength = Password(required).maxlength,
                minlength = Password(required).minlength,
                autocomplete = Password(required).autocomplete,
            };
        }

        public static ValidationUI Adres(bool? required = null, int? minLength = 10)
        {
            return new ValidationUI
            {
                pattern = "[0-9a-zA-âÂZÇŞĞÜÖİçşğüöı \\-/.,;:]+",
                maxlength = 255,
                minlength = minLength == null ? 10 : minLength,
                required = (required == true)
            };
        }

        public static ValidationUI URL(bool? required = null)
        {
            return new ValidationUI
            {
                maxlength = 255,
                type = "url",
                required = (required == true),
            };
        }

        public static ValidationUI MeasurementLinearity(bool? required = null, Int32? minLength = 0, Int32? maxLength = 100)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[0-9%+-]+",
                data_pattern_error = "Lütfen Sayı %,+,- gibi Alanları Tercih Ediniz..",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public class ValidationUI
        {
            public string pattern { get; set; }
            public Int32? maxlength { get; set; }
            public Int32? minlength { get; set; }
            public string type { get; set; }
            public bool? required { get; set; }
            public string data_match { get; set; }
            public string data_error { get; set; }
            public string data_required_error { get { return "Lütfen Bu Alanı Doldurun.."; } }
            public string data_pattern_error { get; set; }
            public string autocomplete { get; set; }    //      on  -  off
            public Int32? min { get; set; }
            public Int32? max { get; set; }
        }

    }
    public enum GridSelectorType
    {
        [Description("radio")]
        Radio = 0,
        [Description("checkbox")]
        Checkbox = 1
    }

    public static class BaseModel
    {
        private static string GetAttributesString(IDictionary<string, object> attributes)
        {
            var res = String.Empty;

            if (attributes == null)
                return res;

            foreach (var attr in attributes)
            {
                res += " " + attr.Key.ToString().ToLower() + "=\"";

                res += attr.Value.GetType().IsAssignableFrom(typeof(bool)) ? attr.Key.ToString().ToLower() : attr.Value;

                res += "\"";
            }

            return res;

        }

        private static IDictionary<string, object> ReplaceAttributes(ref IDictionary<string, object> Keys, Validations.ValidationUI param)
        {

            foreach (var pr in param.GetType().GetProperties().Where(a => a.GetValue(param) != null).ToList())
            {

                var nm = pr.Name.ToString().Replace("_", "-");
                var val = pr.GetValue(param).GetType().IsAssignableFrom(typeof(bool))
                    ? nm
                    : pr.GetValue(param);

                if (Keys[nm] != null)
                {
                    Keys[nm] = val;
                }
                else
                {
                    Keys.Add(nm, val);
                }

            }

            return Keys;

        }

        public static string AppendAttribute(this string item, string Key, object Value)
        {

            var pat = Key + "=\"(.*?)\"";

            if (Regex.Match(item, pat).Success)
            {
                item = Regex.Replace(item, "" + Key + "=\"(.*?)\"", Key += "=\"" + Value + "\"", RegexOptions.IgnoreCase);
            }
            else
            {
                if (item.StartsWith("<input"))
                {
                    item = item.Insert(6, " " + Key + "=\"" + Value + "\"");
                }
                else if (item.StartsWith("<div"))
                {
                    item = item.Insert(4, " " + Key + "=\"" + Value + "\"");
                }
                else if (item.StartsWith("<textarea"))
                {
                    item = item.Insert(9, " " + Key + "=\"" + Value + "\"");
                }
                else if (item.StartsWith("<select"))
                {
                    item = item.Insert(7, " " + Key + "=\"" + Value + "\"");
                }

            }

            return item;

        }

        public static string AppendAttributes(this string item, Validations.ValidationUI validate)
        {
            if (validate == null)
                return item;

            var attr = validate.GetType().GetProperties().Where(a => a.GetValue(validate) != null)
                .ToDictionary(
                k => k.Name.ToString().Replace("_", "-").ToLower(),
                v => v.GetValue(validate).GetType().IsAssignableFrom(typeof(bool)) ? v.Name.ToString().Replace("_", "-").ToLower() : v.GetValue(validate));

            return item.AppendAttributes(attr);

        }

        public static string AppendAttributes(this string item, IDictionary<string, object> attributes)
        {

            if (attributes == null)
                return item;

            foreach (var attr in attributes)
            {
                item = item.AppendAttribute(attr.Key.ToString().ToLower(), attr.Value);
            };

            return item;

        }

        public static string AppendAttributesValidation(this string item, Validations.ValidationUI param)
        {

            if (param == null)
                return item;


            foreach (var pr in param.GetType().GetProperties().Where(a => a.GetValue(param) != null).ToList())
            {

                var key = pr.Name.ToString().Replace("_", "-");
                var val = pr.GetValue(param).GetType().IsAssignableFrom(typeof(bool))
                    ? key
                    : pr.GetValue(param);

                item = item.AppendAttribute(key, val);

            }

            return item;

        }

        public static string AsAttribute(this string item, string attribute)
        {
            return " " + attribute + "=\"" + item + "\"";
        }

        public static string AsAttribute(this bool item, string attribute)
        {
            return " " + attribute + "=\"" + item.ToString().ToLower() + "\"";
        }

        //  Validations To Kendo

        //public static DatePickerBuilder Validate(this DatePickerBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.DatePicker)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}


        //public static DropDownListBuilder Validate(this DropDownListBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.DropDownList)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}


        public static MvcHtmlString Validate(this MvcHtmlString item, Validations.ValidationUI param)
        {

            var hstr = item.ToHtmlString();

            foreach (var pr in param.GetType().GetProperties().Where(a => a.GetValue(param) != null).ToList())
            {

                var nm = pr.Name.ToString().Replace("_", "-");
                var pat = nm + "=\"(.*?)\"";

                var _val = pr.GetValue(param).GetType().IsAssignableFrom(typeof(bool)) && (bool)pr.GetValue(param) == true
                    ? nm
                    : pr.GetValue(param);

                if (pr.GetValue(param).GetType().IsAssignableFrom(typeof(bool)) && (bool)pr.GetValue(param) == false)
                    continue;

                hstr = AppendAttribute(hstr, nm, _val);

            }

            return new MvcHtmlString(hstr);

        }

        //public static MvcHtmlString DynamicValidation(this MvcHtmlString item, string elems)
        //{

        //    var hstr = item.ToHtmlString();

        //    hstr = Regex.Replace(hstr, "required=\"(.*?)\"", String.Empty).Replace("  ", " ");

        //    elems += ",#" + Regex.Match(hstr, "id=\"(.*?)\"", RegexOptions.IgnoreCase).Value.Replace("id=\"", "").Replace("\"", "");
        //    var ifQuery = "";
        //    foreach (var elem in elems.Split(','))
        //        ifQuery += "+ $('" + elem.Trim() + "').val()";
        //    ifQuery = ifQuery.Substring(2);

        //    hstr += "\n" + "<script type=\"text-javascript\">" + "\n"
        //        + "$(function () {" + "\n"
        //        + "$(document).on('change', '" + elems + "', function(){" + "\n"
        //        + "var run = (" + ifQuery + ") != null && (" + ifQuery + ") != '';" + "\n"
        //        + "if(run == true) { "
        //        + "$('" + elems + "').attr('required','required');"
        //        + " } else { "
        //        + "$('" + elems + "').removeAttr('required');"
        //        + " }"
        //        + "}" + "\n"
        //        + "$('" + elems + "')"
        //        + "})" + "\n"
        //        + "</script>";

        //    return new MvcHtmlString(hstr);

        //}

        //public static AutoCompleteBuilder Validate(this AutoCompleteBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.AutoComplete)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static CheckBoxBuilder Validate(this CheckBoxBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.CheckBox)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static ColorPickerBuilder Validate(this ColorPickerBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.ColorPicker)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static ComboBoxBuilder Validate(this ComboBoxBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.ComboBox)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static NumericTextBoxBuilder<long> Validate(this NumericTextBoxBuilder<long> item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.NumericTextBox<long>)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static NumericTextBoxBuilder<int> Validate(this NumericTextBoxBuilder<int> item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.NumericTextBox<int>)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static NumericTextBoxBuilder<short> Validate(this NumericTextBoxBuilder<short> item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.NumericTextBox<short>)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static NumericTextBoxBuilder<double> Validate(this NumericTextBoxBuilder<double> item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.NumericTextBox<double>)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static DateTimePickerBuilder Validate(this DateTimePickerBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.DateTimePicker)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static EditorBuilder Validate(this EditorBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.Editor)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static MaskedTextBoxBuilder Validate(this MaskedTextBoxBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.MaskedTextBox)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static MultiSelectBuilder Validate(this MultiSelectBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.MultiSelect)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static RadioButtonBuilder Validate(this RadioButtonBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.RadioButton)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static RangeSliderBuilder<double> Validate(this RangeSliderBuilder<double> item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.RangeSlider<double>)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static RecurrenceEditorBuilder Validate(this RecurrenceEditorBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.RecurrenceEditor)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static SliderBuilder<double> Validate(this SliderBuilder<double> item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.Slider<double>)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static TimePickerBuilder Validate(this TimePickerBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.TimePicker)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static TimezoneEditorBuilder Validate(this TimezoneEditorBuilder item, Validations.ValidationUI param)
        //{
        //    var hAttr = ((Kendo.Mvc.UI.TimezoneEditor)item).HtmlAttributes;
        //    ReplaceAttributes(ref hAttr, param);
        //    return item;
        //}

        //public static GridBoundColumnBuilder<T> GridSelector<T>(this GridBoundColumnBuilder<T> item, GridSelectorType type) where T : class
        //{

        //    item.Column.Title = String.Empty;
        //    item.Column.ClientTemplate = "<input type=\"" + type.ToDescription() + "\" data-event=\"GridSelector\" />";
        //    item.Column.Width = "45px";
        //    item.Column.Sortable = false;
        //    item.Column.Filterable = false;
        //    item.Column.HtmlAttributes.Add("style", "max-width: 45px !important;");
        //    item.Column.HeaderHtmlAttributes.Add("style", "max-width: 45px !important;");

        //    return item;

        //}

        //public static GridBoundColumnBuilder<T> DataColumn<T>(this GridBoundColumnBuilder<T> item, Expression<Func<T, object>> expression) where T : class
        //{

        //    MemberExpression body = expression.Body as MemberExpression;
        //    if (body == null)
        //    {
        //        UnaryExpression ubody = (UnaryExpression)expression.Body;
        //        body = ubody.Operand as MemberExpression;
        //    }

        //    item.Column.ClientTemplate = item.Column.ClientTemplate.AppendAttribute("data-" + body.Member.Name, "#=data." + body.Member.Name + "#");

        //    return item;
        //}

    }
    [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
    public static class Extensions
    {
        /// <summary>
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ToTimeStamp(this long timeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(Convert.ToInt64(timeStamp)).ToLocalTime();
        }

        public static string ToJson(this object param)
        {
            return new JavaScriptSerializer().Serialize(param);
        }

        public static CultureInfo Culture(string lang = "tr-TR")
        {
            return CultureInfo.GetCultureInfo(lang);
        }
        public static List<Guid> ToGuidList(this IEnumerable<string> strList)
        {

            var res = new List<Guid>();

            foreach (var item in strList)
            {
                if (item.IsValidGuid())
                {
                    res.Add(new Guid(item));
                }
            }

            return res;

        }
        public static string ToDescription(this Enum item)
        {
            MemberInfo[] memberInfos = item.GetType().GetMembers(BindingFlags.Public | BindingFlags.Static);

            var res = memberInfos.Where(a => a.Name == item.ToString()).FirstOrDefault();

            if (res != null)
            {
                return res.GetCustomAttributesData().Select(a => a.ConstructorArguments).FirstOrDefault().FirstOrDefault().Value.ToString();
            }
            else
            {
                return item.ToString();
            }

        }
        public static List<object> ToArrayList(this string strList)
        {
            return strList.Split(',').Cast<object>().ToList();
        }

        //public static List<TT> ToDropListDown<TT>(this TT[] item, string placeholder = "Seçim Yapınız")
        //{
        //    dynamic p = new { Text = placeholder, Value = DBNull.Value };
        //    List<TT> drop = new List<TT> { p };
        //    drop.AddRange(item);
        //    return drop;
        //}

        public static List<TT> ToDropListDown<TT>(this IEnumerable<TT> item, string placeholder = "Seçim Yapınız")
        {
            var typeDATA = typeof(TT).GetProperties();
            List<TT> drop = new List<TT>();
            var entity = (TT)Activator.CreateInstance(typeof(TT));
            var propertiesInfo = entity.GetType().GetProperties();

            foreach (var propertyInfo in propertiesInfo)
            {
                if (propertyInfo.Name.ToLowerInvariant().Trim() == "text")
                    propertyInfo.SetValue(entity, placeholder, null);
                if (propertyInfo.Name.ToLowerInvariant().Trim() == "value")
                    propertyInfo.SetValue(entity, null);
            }
            drop.Add(entity);
            drop.AddRange(item);
            return drop.ToList();
        }

        public static List<SelectListItem> ToDropListDown(this SelectListItem[] item)
        {
            List<SelectListItem> drop = new List<SelectListItem> { new SelectListItem() { Text = "Seçim Yapınız", Value = null } };
            drop.AddRange(item);
            return drop;
        }
        public static List<SelectListItem> ToSelectAll(this List<SelectListItem> item, string placeholder = "Select All")
        {
            List<SelectListItem> drop = new List<SelectListItem> { new SelectListItem() { Text = placeholder, Value = Guid.Empty.ToString() } };
            drop.AddRange(item);
            return drop;
        }
        public static List<SelectListItem> ToSelectAll(this SelectListItem[] item, string placeholder = "Select All")
        {
            List<SelectListItem> drop = new List<SelectListItem> { new SelectListItem() { Text = placeholder, Value = Guid.Empty.ToString() } };
            drop.AddRange(item);
            return drop;
        }


        public static List<T> ToConvertDateTableForList<T>(this DataTable dataTable)
        {
            var listItem = new List<T>();
            if (dataTable.Rows.Count > 0)
            {
                var tClass = typeof(T);
                var pClass = tClass.GetProperties();
                var dc = dataTable.Columns.Cast<DataColumn>().ToList();
                foreach (DataRow item in dataTable.Rows)
                {
                    var cn = (T)Activator.CreateInstance(tClass);
                    foreach (var pc in pClass)
                    {
                        try
                        {
                            var d = dc.Find(c => c.ColumnName == pc.Name);
                            if (d != null)
                            {
                                if (item[pc.Name] == DBNull.Value && pc.PropertyType.GenericTypeArguments.Length > 0)
                                {
                                    pc.SetValue(cn, null, null);
                                }
                                else if (item[pc.Name] == DBNull.Value)
                                {
                                    pc.SetValue(cn, NewHelpers.GetDefault(d.DataType), null);
                                }
                                else
                                {
                                    pc.SetValue(cn, Convert.ChangeType(item[pc.Name], d.DataType), null);
                                }
                            }

                        }
                        catch (System.Exception ex)
                        {
                            throw new System.Exception(ex.Message);
                        }
                    }
                    listItem.Add(cn);
                }
            }
            return listItem;
        }
        public static DataTable ToConvertToDataTable<T>(this IList<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    AttributeCollection attributes = TypeDescriptor.GetAttributes(prop);
                    if (!attributes[typeof(ReadOnlyAttribute)].Equals(ReadOnlyAttribute.Yes))
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }
            return table;
        }
        public static object ToNumericFormat(this object param)
        {
            var truCultureInfo = CultureInfo.CreateSpecificCulture("tr-TR");
            return string.Format(truCultureInfo, "{0:N0}", param);
        }
        public static object ToMoneyFormat(this object param)
        {
            var truCultureInfo = CultureInfo.CreateSpecificCulture("tr-TR");
            return string.Format(truCultureInfo, "{0:N2}", param);
        }
        public static object ToMoneyIconFormat(this object param)
        {
            var truCultureInfo = CultureInfo.CreateSpecificCulture("tr-TR");
            return string.Format(truCultureInfo, "{0:C}", param);
        }
        public static string GetIdCode(string firstLetter = null)
        {
            var d = DateTime.Now;
            return $"{firstLetter}{d.Year.ToString() + d.Month + d.Day + d.Hour + d.Minute + d.Second + d.Millisecond}";
        }
        public static bool IsValidGuid(this string str)
        {
            Guid guid;

            if (String.IsNullOrEmpty(str)) { return false; }

            return Guid.TryParse(str, out guid);
        }
        public static bool IsValidGuidRegex(this string expression)
        {
            if (expression != null)
            {
                Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

                return guidRegEx.IsMatch(expression);
            }
            return false;
        }
        public static double ToDekar(this double param)
        {
            return Math.Round((param / 1000.0), 3);
        }
        public static double ToDouble(this int item)
        {
            double TryParse;

            if (double.TryParse(item.ToString().Replace(".", ","), out TryParse) == true)
            {
                return Convert.ToDouble(item);
            }

            return default(double);
        }
        public static string ToPhoneMaskReplace(this string item)
        {
            return item.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Trim();
        }
        public static Int32 ToInt32(this double item)
        {
            Int32 TryParse;

            if (Int32.TryParse(item.ToString(), out TryParse) == true)
            {
                return Convert.ToInt32(item);
            }

            return default(Int32);
        }
        public static Int32 ToInt32(this float item)
        {
            Int32 TryParse;

            if (Int32.TryParse(item.ToString(), out TryParse) == true)
            {
                return Convert.ToInt32(item);
            }

            return default(Int32);
        }
        public static int? ToInt32(this string item)
        {

            Int32 tryParse;

            if (Int32.TryParse(item, out tryParse))
            {
                return Convert.ToInt32(item);
            }

            return 0;
        }
        public static bool IsTypeNullable(this Type type) => Nullable.GetUnderlyingType(type) != null;
        public static bool IsNumber(this string s, bool floatpoint)
        {
            int i;
            double d;
            string withoutWhiteSpace = s.RemoveSpaces();
            if (floatpoint)
                return double.TryParse(withoutWhiteSpace, NumberStyles.Any,
                    Thread.CurrentThread.CurrentUICulture, out d);
            else
                return int.TryParse(withoutWhiteSpace, out i);
        }
        public static string RemoveSpaces(this string s)
        {
            return s.Replace(" ", "");
        }
        public static string Nl2Br(this string s)
        {
            return s.Replace("\r\n", "<br />").Replace("\n", "<br />");
        }
        public static string StripHtml(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;
            return WebUtility.HtmlDecode(Regex.Replace(html, @"<[^>]*>", string.Empty));
        }
        public static string Join(this IEnumerable<object> array, string seperator)
        {
            if (array == null)
                return "";
            return string.Join(seperator, array.ToArray());
        }
        public static string Join(this object[] array, string seperator)
        {
            if (array == null)
                return "";
            return string.Join(seperator, array);
        }
        public static double? ToDouble(this string item)
        {
            double TryParse;

            if (item.ToArray().Count(a => a == ',') + item.ToArray().Count(a => a == '.') > 1)
            {
                return null;
            }

            if (item.All(Char.IsDigit) && double.TryParse(item, out TryParse))
            {
                return TryParse;
            }

            if (item.IndexOf(",") > -1)
            {
                if (double.TryParse(item.Replace(",", "."), out TryParse))
                {
                    return TryParse;
                }
            }

            if (double.TryParse(item, out TryParse))
            {
                return TryParse;
            }

            return null;

        }
        public static T EntityDataCopyNoBaseEntity<T, T2>(this T entity, T2 copyData)
        {
            //foreach (var prop in copyData.GetType().GetProperties().Where(prop => prop != null && prop.Name != "id" && prop.Name != "Id" && prop.Name != "ID"))
            foreach (var prop in copyData.GetType().GetProperties())
            {
                var value = prop.GetValue(copyData, null);
                if (value == null) continue;

                var isThere = entity.GetType().GetProperties().Where(x => x.Name == prop.Name).FirstOrDefault();
                if (isThere != null)
                {
                    //if (prop.PropertyType.GenericTypeArguments.Length > 0 && prop.PropertyType.GenericTypeArguments[0].Name == "Guid")
                    if (prop.PropertyType.GenericTypeArguments.Length > 0)
                    {
                        isThere.SetValue(entity, Convert.ChangeType(value, prop.PropertyType.GenericTypeArguments[0], CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        isThere.SetValue(entity, Convert.ChangeType(value, prop.PropertyType, CultureInfo.InvariantCulture));
                        //isThere.SetValue(entity, Convert.ChangeType(value, value.GetType()));
                    }
                }

            }
            return entity;
        }
        public static T EntityDataCopy<T, T2>(this T entity, T2 copyData)
        {
            //foreach (var prop in copyData.GetType().GetProperties().Where(prop => prop != null && prop.Name != "id" && prop.Name != "Id" && prop.Name != "ID"))
            foreach (var prop in copyData.GetType().GetProperties().Where(prop => !new BaseEntity().GetType().GetProperties().Select(x => x.Name).Contains(prop.Name)))
            {
                var value = prop.GetValue(copyData, null);
                if (value == null) continue;

                var isThere = entity.GetType().GetProperties().Where(x => x.Name == prop.Name).FirstOrDefault();
                if (isThere != null)
                {
                    //if (prop.PropertyType.GenericTypeArguments.Length > 0 && prop.PropertyType.GenericTypeArguments[0].Name == "Guid")
                    if (prop.PropertyType.GenericTypeArguments.Length > 0)
                    {
                        isThere.SetValue(entity, Convert.ChangeType(value, prop.PropertyType.GenericTypeArguments[0], CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        isThere.SetValue(entity, Convert.ChangeType(value, prop.PropertyType, CultureInfo.InvariantCulture));
                        //isThere.SetValue(entity, Convert.ChangeType(value, value.GetType()));
                    }
                }

            }
            return entity;
        }
        public static string ToSeoUrl(this string name)
        {
            var phrase = string.Format("{0}", name);
            var str = RemoveAccent(phrase).ToLower();
            str = RemoveAccent(str).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            str = str.Replace(".", "-");
            return str;
        }
        public static string MD5Sifrele(this string item)
        {

            // MD5CryptoServiceProvider sınıfının bir örneğini oluşturduk.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //Parametre olarak gelen veriyi byte dizisine dönüştürdük.
            byte[] dizi = Encoding.UTF8.GetBytes(item);
            //dizinin hash'ini hesaplattık.
            dizi = md5.ComputeHash(dizi);
            //Hashlenmiş verileri depolamak için StringBuilder nesnesi oluşturduk.
            StringBuilder sb = new StringBuilder();
            //Her byte'i dizi içerisinden alarak string türüne dönüştürdük.

            foreach (byte ba in dizi)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            //hexadecimal(onaltılık) stringi geri döndürdük.
            return sb.ToString();
        }
        private static string RemoveAccent(string text)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return Encoding.ASCII.GetString(bytes);
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public static Image ScaleImage(this Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        public static string IsNewFileName(this string param)
        {
            return $"{DateTime.Now.ToString(new CultureInfo("tr")).Replace(" ", "").Replace(",", "").Replace(":", "").Replace(".", "").Replace("/", "")}_{param}";
        }
        public static string DateFormatShort(bool? kendoGrid = false)
        {
            var vl = DateTimeFormatInfo.CurrentInfo.ShortDatePattern.Replace("d", "dd").Replace("ddd", "dd"); ;

            if (kendoGrid == true)
            {
                return "{0:" + vl + "}";
            }

            return vl;
        }
        public static string DateFormatFull(bool? kendoGrid = false)
        {
            var vl = DateFormatShort() + " " + DateTimeFormatInfo.CurrentInfo.ShortTimePattern;

            if (kendoGrid == true)
            {
                return "{0:" + vl + "}";
            }

            return vl;
        }
        #region MVC Extensions
        public static MvcHtmlString PartialFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.PartialFor(typeof(TValue).Name, expression);
        }
        public static MvcHtmlString PartialFor<TModel, TValue>(this HtmlHelper<TModel> html, string partialViewName, Expression<Func<TModel, TValue>> expression)
        {
            var containingModel = html.ViewData.Model;
            var model = expression.Compile()(containingModel);

            var oldTemplateInfo = html.ViewData.TemplateInfo;
            var newViewData = new ViewDataDictionary(html.ViewData)
            {
                TemplateInfo = new TemplateInfo
                {
                    FormattedModelValue = oldTemplateInfo.FormattedModelValue,
                }
            };

            var newPrefix = ExpressionHelper.GetExpressionText(expression);
            if (oldTemplateInfo.HtmlFieldPrefix.Length > 0)
            {
                newPrefix = oldTemplateInfo.HtmlFieldPrefix + "." + newPrefix;
            }
            newViewData.TemplateInfo.HtmlFieldPrefix = newPrefix;

            return html.Partial(partialViewName, model, newViewData);
        }
        #endregion
        public static string ToDateTime(this DateTime item, string culture = "tr-TR")
        {
            return item.ToString("g", new CultureInfo(culture));
        }
        public static string GetName(this Type item)
        {

            var elem = item.UnderlyingSystemType.GetGenericArguments().Select(a => a.Name).FirstOrDefault();
            if (String.IsNullOrEmpty(elem))
            {
                elem = item.Name.Replace("[]", "");
            }
            return elem;
        }
        public static T AppendObjectToOther<T, T2>(this T baseObject, T2 beCopyObject)
        {
            foreach (var beCopyProp in beCopyObject.GetType().GetProperties().Where(prop => !new BaseEntity().GetType().GetProperties().Select(x => x.Name).Contains(prop.Name)))
            {
                var value = beCopyProp.GetValue(beCopyObject, null);
                var baseObjectProp = baseObject.GetType().GetProperties().Where(x => x.Name == beCopyProp.Name).FirstOrDefault();

                if (!baseObjectProp.GetType().IsAssignableFrom(beCopyProp.GetType()))
                {
                    continue;
                }

                if (baseObjectProp != null)
                {
                    if (beCopyProp.PropertyType.GenericTypeArguments.Length > 0)
                    {
                        baseObjectProp.SetValue(baseObject, value);
                    }
                    else
                    {
                        baseObjectProp.SetValue(baseObject, value);
                    }
                }

            }
            return baseObject;
        }

        public static bool IsHtmlFragment(string value)
        {
            return Regex.IsMatch(value, @"</?(p|div)>");
        }

        /// <summary>
        /// Remove tags from a html string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveTags(string value)
        {
            if (value != null)
            {
                value = CleanHtmlComments(value);
                value = CleanHtmlBehaviour(value);
                value = Regex.Replace(value, @"</[^>]+?>", " ");
                value = Regex.Replace(value, @"<[^>]+?>", "");
                value = value.Trim();
            }
            return value;
        }

        /// <summary>
        /// Clean script and styles html tags and content
        /// </summary>
        /// <returns></returns>
        public static string CleanHtmlBehaviour(string value)
        {
            value = Regex.Replace(value, "(<style.+?</style>)|(<script.+?</script>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return value;
        }

        /// <summary>
        /// Replace the html commens (also html ifs of msword).
        /// </summary>
        public static string CleanHtmlComments(string value)
        {
            //Remove disallowed html tags.
            value = Regex.Replace(value, "<!--.+?-->", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return value;
        }

        /// <summary>
        /// Adds rel=nofollow to html anchors
        /// </summary>
        public static string HtmlLinkAddNoFollow(string value)
        {
            return Regex.Replace(value, "<a[^>]+href=\"?'?(?!#[\\w-]+)([^'\">]+)\"?'?[^>]*>(.*?)</a>", "<a href=\"$1\" rel=\"nofollow\" target=\"_blank\">$2</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
    }

    public static class DictionaryExtensions
    {
        public static T Get<T>(this IDictionary<string, object> dictionary, string key)
        {
            if (key == null)
                throw new ArgumentNullException("key", "Görnderilen Key Parametresi null");

            return (T)dictionary[key];
        }
        public static bool TryGet<T>(this IDictionary<string, object> dictionary, string key, out T value)
        {
            if (key == null)
                throw new ArgumentNullException("key", "Görnderilen Key Parametresi null");

            object result;
            if (dictionary.TryGetValue(key, out result) && result is T)
            {
                value = (T)result;
                return true;
            }
            value = default(T);
            return false;
        }
        public static void Add(this IDictionary<string, object> dictionary, string key, object value)
        {
            if (key == null)
                throw new ArgumentNullException("key", "Görnderilen Key Parametresi null");

            if (value == null)
                throw new ArgumentNullException("value", "Görnderilen Value Parametresi null");

            if (dictionary.All(x => x.Key != key))
                dictionary.Add(new KeyValuePair<string, object>(key, value));
            else
                throw new ArgumentNullException("key", "Dictionary içinde böyle bir Key mevcuttur");
        }
        public static void Set(this IDictionary<string, object> dictionary, string key, object value)
        {
            if (key == null)
                throw new ArgumentNullException("key", "Görnderilen Key Parametresi null");

            if (value == null)
                throw new ArgumentNullException("value", "Görnderilen Value Parametresi null");

            dictionary[key] = value;
        }
        public static void Remove(this IDictionary<string, object> dictionary, string key)
        {
            if (key == null)
                throw new ArgumentNullException("key", "Görnderilen Key Parametresi null");

            dictionary.Remove(key);
        }
    }
}