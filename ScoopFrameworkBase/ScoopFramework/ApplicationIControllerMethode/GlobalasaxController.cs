namespace ScoopFramework.ApplicationIControllerMethode
{
    public class GlobalasaxController
    {
        //public List<SH_PageAction> ControllerInMethod()
        //{
        //    var actions = new List<SH_PageAction>();
        //    var delete = new List<SH_PageAction>();
        //    var dbPageActionList =
        //        DataAccessLayer<SH_PageAction>.EntityManagementSelect(new List<ParamValue>(), "SH_PageAction");
        //    try
        //    {

        //        var controllerlar =
        //            from a in AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.StartsWith("HastaneTakipOto"))
        //            from t in a.GetTypes()
        //            where typeof(IController).IsAssignableFrom(t)
        //            select t;

        //        foreach (var typeController in controllerlar)
        //        {
        //            var controllerType = typeController;
        //            var area = "";

        //            if (controllerType.FullName.Contains("Areas."))
        //            {
        //                const string splittext = "Areas.";
        //                area = controllerType.FullName.Remove(0,
        //                    controllerType.FullName.IndexOf("Areas.", StringComparison.Ordinal) + splittext.Length)
        //                  .Split('.')
        //                  .FirstOrDefault();

        //                if (!string.IsNullOrEmpty(area)) area = "/" + area;
        //            }

        //            foreach (var item in new ReflectedControllerDescriptor(controllerType).GetCanonicalActions().ToList())
        //            {
        //                var returnParameter = ((ReflectedActionDescriptor)(item)).MethodInfo.ReturnParameter;

        //                if (returnParameter != null)
        //                {
        //                    var methodParametre = string.Join(",",
        //                                       ((ReflectedActionDescriptor)(item)).MethodInfo.GetParameters()
        //                                           .Select(info => info.Name)
        //                                           .ToArray());

        //                    var controller = controllerType.Name.Replace("Controller", "");
        //                    var method = item.ActionName;
        //                    var controllerAction = string.Format("{0}/{1}/{2}", area, controller, method);

        //                    var attr = item.GetCustomAttributes(false).Select(o => (((System.Attribute)o).TypeId)).Where(o => ((Type)o).Name.Equals("AllowEveryone"));

        //                    foreach (var o in attr.Where(o => !StaticTempMethod.CustomAttr.Any(pair => pair.Key.Equals(controllerAction))))
        //                    {
        //                        StaticTempMethod.CustomAttr.Add(controllerAction, ((Type)o).Name.ToString());
        //                    }

        //                    if (!delete.Any(p => p.Action.ToUpper(new CultureInfo("en-US", false)).Equals(controllerAction.ToUpper(new CultureInfo("en-US", false)))))
        //                    {
        //                        delete.Add(new SH_PageAction() { Action = controllerAction });
        //                    }
        //                    if (!dbPageActionList.Any(p => p.Action.ToUpper(new CultureInfo("en-US", false)).Equals(controllerAction.ToUpper(new CultureInfo("en-US", false)))))
        //                    {
        //                        if (!actions.Any(p => p.Action.ToUpper(new CultureInfo("en-US", false)).Equals(controllerAction.ToUpper(new CultureInfo("en-US", false)))))
        //                        {
        //                            actions.Add(new SH_PageAction() { id = new Random().Next(1, 10000), Action = controllerAction, Status = false });
        //                            actions.Add(new SH_PageAction()
        //                            {
        //                                id = new Random().Next(1, 10000),
        //                                created = DateTime.Now,
        //                                Action = controllerAction,
        //                                Status = false,
        //                                Controller = controller,
        //                                Method = method,
        //                                ReturnParametre = returnParameter.ParameterType.Name,
        //                                MethodParametre = !string.IsNullOrEmpty(methodParametre) ? methodParametre : null
        //                            });
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        foreach (var shPageAction in dbPageActionList)
        //        {
        //            var item = delete.FirstOrDefault(action => action.Action.Equals(shPageAction.Action));
        //            if (item != null)
        //            {
        //                delete.Remove(item);
        //            }
        //            else
        //            {
        //                var deleteResult = DataAccessLayer<SH_PageAction>.EntityManagementDelete(shPageAction, "SH_PageAction");
        //            }
        //        }



        //        if (actions.Count > 0)
        //        {
        //            var insertResult = DataAccessLayer<SH_PageAction>.EntityManagementInsert(actions, "SH_PageAction");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is ReflectionTypeLoadException)
        //        {
        //            var typeLoadException = ex as ReflectionTypeLoadException;
        //            var loaderExceptions = typeLoadException.LoaderExceptions;
        //        }
        //    }
        //    return actions;
        //}
    }
}
