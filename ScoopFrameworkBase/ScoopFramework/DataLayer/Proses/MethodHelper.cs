using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using ScoopFramework.Attribute;
using ScoopFramework.DataLayer.Library;
using ScoopFramework.Domain;
using ScoopFramework.Extension;
using ScoopFramework.Model;
using ScoopFramework.UrlController;
using ScoopFramework.UserLogin;

namespace ScoopFramework.DataLayer.Proses
{
    public class MethodHelper
    {
        public object ModelCreate(HttpRequestBase request)
        {
            try
            {
                var tablo = request["xtb"] != null ? request["xtb"].Base64Decode() : null;
                var result = SetValuetoClass<object>(tablo).FirstOrDefault();

                var paramValues = request.Form.AllKeys.Select(query => new ParamValue()
                {
                    Key = query.StartsWith("_") ? query.Substring(query.LastIndexOf("_", StringComparison.Ordinal) + 1) : query,
                    Value = request[query].StartsWith("?") ? request[query].Substring(request[query].LastIndexOf("_", StringComparison.Ordinal) + 1) : request[query]
                }).ToList();

                if (paramValues.Count() == 0)
                {
                    foreach (var rRequst in result.GetType().GetProperties())
                    {
                        if (request[rRequst.Name] != null && request[rRequst.Name] != String.Empty)
                        {
                            paramValues.Add(new ParamValue()
                            {
                                Key = rRequst.Name,
                                Value = request[rRequst.Name],
                            });
                        }
                    }
                }



                if (request.Files != null && request.Files.Count > 0)
                {
                    if (result != null)
                    {
                        var imagesProp = result.GetType().GetProperties()
                            .Where(p => p.GetCustomAttributes(typeof(ImageTypeAttribute), true).Length > 0)
                            .ToList();
                        if (imagesProp.Count > 0)
                        {
                            for (int i = 0; i < request.Files.Count; i++)
                            {
                                paramValues.Add(new ParamValue() { Key = (imagesProp.Count() == 1 ? imagesProp[0].Name : imagesProp[i].Name), ImageValue = request.Files[i] });
                            }
                        }
                    }
                }

                var entity = SetPropertyValue(result, paramValues);
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static IEnumerable<T> SetValuetoClass<T>(string classname)
        {

            List<T> res = new List<T>();
            //var ids = id.Split(',').ToList();
            var cls = GetDbClassFromString(classname);

            //foreach (var _id in ids)
            //{
            dynamic newClass = (dynamic)Activator.CreateInstance(cls);

            //foreach (var p in newClass.GetType().GetProperties())
            //{
            //    if (p.Name == "id" || p.Name == "Id")
            //    {
            //        p.SetValue(newClass, Guid.NewGuid());
            //    }
            //}

            res.Add(newClass);

            //}

            return res;

        }
        public static Type GetDbClassFromString(string className)
        {
            var classlar = GetClassesInNamespace("ScoopFramework", "ScoopFramework.DBModel");
            if (!classlar.Where(x => x.Name == className).Any())
            {
                classlar = GetClassesInNamespace("ScoopFramework", "ScoopFramework.DbBusiness.DB");
            }
            else if (!classlar.Where(x => x.Name == className).Any())
            {
                classlar = GetClassesInNamespace("Totan", "Totan.Models");
            }
            return classlar.FirstOrDefault(a => a.Name.ToLower() == className.ToLower());

        }
        private static List<Type> GetClassesInNamespace(string dll, string nameSpace)
        {
            if (string.IsNullOrEmpty(nameSpace))
            {
                return new List<Type>();
            }
            try
            {
                var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.ManifestModule.Name == String.Format("{0}.dll", dll)).ToList();
                foreach (var assembly in assemblys)
                {
                    var _type = assembly.GetTypes().Where(x => x.IsClass && x.Namespace == nameSpace).ToList();
                    if (_type.Count > 0)
                    {
                        return _type;
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex is ReflectionTypeLoadException)
                {
                    //AppDomainCreated.PreLoadDeployedAssemblies();

                    /*YÜklenmeyen dll lerin burda yüklenmesi gerekir.*/

                    var typeLoadException = ex as ReflectionTypeLoadException;
                    try
                    {
                        var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.ManifestModule.Name == String.Format("{0}.dll", dll)).ToList();
                        foreach (var assembly in assemblys)
                        {
                            var _type = assembly.GetTypes().Where(x => x.IsClass && x.Namespace == nameSpace).ToList();
                            if (_type.Count > 0)
                            {
                                return _type;
                            }
                        }
                        var loaderExceptions = typeLoadException.LoaderExceptions;
                    }
                    catch { }
                }
            }
            return new List<Type>(); ;

        }
        public T SetPropertyValue<T>(T entity, T formProperty)
        {
            var update = false;

            foreach (var propertyInfo in entity.GetType().GetProperties())
            {
                var pkProperty = propertyInfo.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (pkProperty.Any())
                {
                    var valuepk =
                    ((IEnumerable<ParamValue>)formProperty).Where(x => x.Key.Equals(propertyInfo.Name)).Select(c => c.Value).FirstOrDefault();

                    if (valuepk != null && !string.IsNullOrEmpty(valuepk.ToString()) && !valuepk.Equals(propertyInfo.GetValue(entity, null).GetType().GetDefault().ToString()))
                    {
                        update = true;
                    }
                }

                if (GetUrlSeo(entity, propertyInfo)) continue;

                if (UserUpdate(entity, propertyInfo, update)) continue;

                if (UpdateDate(entity, propertyInfo, update)) continue;

                PropertySet(entity, formProperty, propertyInfo, update);

                if (GetControlDataColumn(entity, propertyInfo))
                {
                    throw new Exception("Böyle Bir Kayı Sistemimizde Mevcuttur.", new Exception());
                }
            }
            return entity;
        }


        private static void PropertySet<T>(T entity, T formProperty, PropertyInfo propertyInfo, bool update)
        {
            foreach (var property in ((IEnumerable<ParamValue>)formProperty).Where(_v => _v.Key == propertyInfo.Name))
            {
                if (PropertyImageSet(entity, property, propertyInfo)) continue;

                if (UserCreated(entity, propertyInfo, update)) continue;

                if (PropertyReplace(entity, property, propertyInfo)) continue;

                if (PropertyEtiketTable(entity, property, propertyInfo)) continue;

                PropertyValueSet(entity, property, propertyInfo);
            }
        }

        private static bool PropertyEtiketTable<T>(T entity, ParamValue property, PropertyInfo propertyInfo)
        {
            try
            {
                var paramValues = new List<ParamValue>();

                //.Where(x => x.GetCustomAttributes(typeof (PrimaryKeyAttribute), true)
                //.FirstOrDefault();
                var attr = propertyInfo.GetCustomAttributes(typeof(EtiketAttribute), true);
                var attrTable = propertyInfo.GetCustomAttributes(typeof(ForingnKeyTable), true);
                var attrsuccess = attr.Length > 0 && attrTable.Length > 0;
                if (attrsuccess)
                {
                    object primarkey = null;

                    foreach (var item in entity.GetType().GetProperties())
                    {
                        var pkAttr = item.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                        if (pkAttr.Length > 0)
                        {
                            primarkey = item.GetValue(entity, null) == null ? item.GetType().GetDefault() : item.GetValue(entity, null);

                        }
                    }

                    var values = property.Value.ToString().Split(',');
                    var foringnKeyTable = (ForingnKeyTable)attrTable.Select(x => x).FirstOrDefault();
                    if (foringnKeyTable != null)
                    {
                        try
                        {
                            var tablo = foringnKeyTable.tableName;
                            var classtablo = SetValuetoClass<object>(tablo).FirstOrDefault();
                            foreach (var item in values)
                            {
                                if (classtablo != null)
                                {
                                    foreach (var rRequst in classtablo.GetType().GetProperties())
                                    {
                                        if (rRequst.GetCustomAttributes(typeof(EtiketAttribute), true).Length > 0)
                                        {
                                            rRequst.SetValue(classtablo, Convert.ChangeType(item, rRequst.PropertyType), null);
                                        }

                                        if (rRequst.GetCustomAttributes(typeof(ForingnKeyAttribute), true).Length > 0)
                                        {
                                            rRequst.SetValue(classtablo, Convert.ChangeType(primarkey, rRequst.PropertyType), null);
                                        }

                                        if (rRequst.GetCustomAttributes(typeof(EtiketSeoAttribute), true).Length > 0)
                                        {
                                            var urlSeo = new UrlSeoController();
                                            var value = urlSeo.GenerateSlug(item);
                                            rRequst.SetValue(classtablo, Convert.ChangeType(value, rRequst.PropertyType), null);
                                        }
                                    }
                                    var result = DataProcessing<object>.Saves(classtablo);
                                    if (!result.success)
                                    {
                                        return false;
                                    }
                                }
                            }
                            return true;
                        }
                        catch (Exception ex) { throw; }
                    }
                }
            }
            catch (Exception ex) { throw; }
            return false;
        }
        private static bool PropertyReplace<T>(T entity, ParamValue property, PropertyInfo propertyInfo)
        {
            var attr = propertyInfo.GetCustomAttributes(typeof(Replace), true);
            var userUpdate = attr.Length > 0;
            if (userUpdate)
            {
                var attrReplace = (Replace)attr.Select(x => x).FirstOrDefault();
                if (attrReplace != null)
                {
                    var replc = attrReplace.name;
                    var value = property.Value.ToString().Replace(replc, "");
                    propertyInfo.SetValue(entity, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                }
                return true;
            }
            return false;
        }

        private static bool PropertyImageSet<T>(T entity, ParamValue property, PropertyInfo propertyInfo)
        {
            if (property.ImageValue != null && property.ImageValue.ContentLength > 0)
            {
                if (propertyInfo.GetCustomAttributes(typeof(ImageTypeAttribute), true).Length > 0)
                {
                    var imgName = Guid.NewGuid() + "_" + property.ImageValue.FileName;

                    var fileName = Path.GetFileName(imgName);
                    var folder = Path.Combine(HttpContext.Current.Server.MapPath("~//upload/site-img"));

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                        UploadImagePath(entity, property, propertyInfo, imgName, fileName);
                    }
                    else
                    {
                        UploadImagePath(entity, property, propertyInfo, imgName, fileName);
                    }

                    return true;
                }
            }
            return false;
        }
        private static void UploadImagePath<T>(T entity, ParamValue property, PropertyInfo propertyInfo, string imgName, string fileName)
        {
            var path = Path.Combine(HttpContext.Current.Server.MapPath("~//upload/site-img")) + "/" + imgName;
            if (!Directory.Exists(path))
            {
                property.ImageValue.SaveAs(path);
                propertyInfo.SetValue(entity, Convert.ChangeType(fileName, propertyInfo.PropertyType), null);
            }
        }
        private static void PropertyValueSet<T>(T entity, ParamValue property, PropertyInfo propertyInfo)
        {
            try
            {
                if (propertyInfo.PropertyType == typeof(System.Guid))
                {
                    propertyInfo.SetValue(entity, new Guid(property.Value.ToString()), null);
                    return;
                }
                if (propertyInfo.PropertyType == typeof(System.Boolean))
                {
                    if (property.Value.Equals("true,false"))
                    {
                        propertyInfo.SetValue(entity, true, null);
                        return;
                    }
                    propertyInfo.SetValue(entity, Convert.ToBoolean(property.Value), null);
                    return;
                }

                if (property.Value != null && !string.IsNullOrEmpty(property.Value.ToString()))
                {
                    propertyInfo.SetValue(entity, Convert.ChangeType(property.Value, propertyInfo.PropertyType), null);
                }
            }
            catch { }
        }
        private static bool UserCreated<T>(T entity, PropertyInfo propertyInfo, bool update)
        {
            var userCreated = propertyInfo.GetCustomAttributes(typeof(UserCreatedAttribute), true).Length > 0;
            if (userCreated && !update)
            {
                var userSession = (PageSecurity)HttpContext.Current.Session["userStatus"];
                propertyInfo.SetValue(entity, Convert.ChangeType(userSession == null ? 0 : userSession.userid, propertyInfo.PropertyType), null);
                return true;
            }
            return false;
        }
        private static bool UpdateDate<T>(T entity, PropertyInfo propertyInfo, bool update)
        {
            var dateUpdate = propertyInfo.GetCustomAttributes(typeof(DateUpdateAttribute), true).Length > 0;
            if (dateUpdate && update)
            {
                propertyInfo.SetValue(entity, Convert.ChangeType(DateTime.Now, propertyInfo.PropertyType), null);
                return true;
            }
            return false;
        }
        private static bool UserUpdate<T>(T entity, PropertyInfo propertyInfo, bool update)
        {
            var userUpdate = propertyInfo.GetCustomAttributes(typeof(UserUpdateAttribute), true).Length > 0;
            if (userUpdate && update)
            {
                var userSession = (PageSecurity)HttpContext.Current.Session["userStatus"];
                propertyInfo.SetValue(entity, Convert.ChangeType(userSession == null ? 0 : userSession.userid, propertyInfo.PropertyType), null);
                return true;
            }
            return false;
        }
        private static bool GetUrlSeo<T>(T entity, PropertyInfo propertyInfo)
        {
            var seo = propertyInfo.GetCustomAttributes(typeof(SeoAttribute), true).Length > 0;
            if (seo)
            {
                var urlSeo = new UrlSeoController();
                var seoName =
                    entity.GetType()
                        .GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(ProcessedSeoName), true).Length > 0);
                if (seoName != null)
                {
                    var value = urlSeo.GenerateSlug(seoName.GetValue(entity, null));
                    propertyInfo.SetValue(entity, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                    return true;
                }
            }
            return false;
        }
        private static bool GetControlDataColumn<T>(T entity, PropertyInfo propertyInfo)
        {
            var control = propertyInfo.GetCustomAttributes(typeof(DataControlColumn), true).Length > 0;
            if (control)
            {
                var column = entity.GetType().GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(DataControlColumn), true).Length > 0);
                if (column != null)
                {
                    var value = column.GetValue(entity, null);
                    var propName = column.Name;
                    var tblName = entity.GetType().Name;
                    var paramValue = new List<ParamValue>() { new ParamValue() { Where = new Dictionary<string, string>() { { propName, value.ToString() } } } };
                    var succesControl = DataProcessing<object>.Count(paramValue, tblName);
                    if (succesControl == 1)
                    {
                        return true;
                    }
                    propertyInfo.SetValue(entity, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                }
            }
            return false;
        }
    }

}