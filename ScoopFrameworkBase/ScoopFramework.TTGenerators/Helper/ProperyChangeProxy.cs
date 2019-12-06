using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace ScoopFramework.TTGenerators.Helper
{

    public class ProperyChangeProxy
    {
        private static AssemblyBuilder _ab;
        private static ModuleBuilder _mb;
        private static Dictionary<Type, Func<object>> Types = new Dictionary<Type, Func<object>>();

        public static TClass[] Create<TClass>(int size) where TClass : class,new()
        {
            var func = GetCreateFunction(typeof(TClass));
            var ret = new TClass[size];
            for (int i = 0; i < size; i++)
            {
                ret[i] = func() as TClass;
            }
            return ret;
        }
        public static TClass Create<TClass>() where TClass : class,new()
        {
            return GetCreateFunction(typeof(TClass))() as TClass;
        }
        public static object Create(Type type)
        {
            return GetCreateFunction(type)();
        }

        static Func<object> GetCreateFunction(Type type)
        {

            Func<object> ret;
            if (!Types.TryGetValue(type, out ret))
            {
                if (_ab == null)
                {
                    var assmName = new AssemblyName("IProperyChangeProxyDynamicAssembly");
                    _ab = AppDomain.CurrentDomain.DefineDynamicAssembly(assmName, AssemblyBuilderAccess.Run);
                    _mb = _ab.DefineDynamicModule(assmName.Name);
                }

                TypeBuilder typeBuilder = _mb.DefineType(type.Name + "__proxy", TypeAttributes.Public, type);

                MethodInfo raisePropertyChanged;
                if (!typeof(INotifyPropertyChanged).IsAssignableFrom(type))
                {
                    typeBuilder.AddInterfaceImplementation(typeof(INotifyPropertyChanged));

                    FieldBuilder eventField = CreatePropertyChangedEvent(typeBuilder);

                    raisePropertyChanged = CreateRaisePropertyChanged(typeBuilder, eventField);
                }
                else
                {
                    raisePropertyChanged = type.GetMethod("NotifyChange", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                }
                // get all the public or protected
                // virtual property setters.
                var props = from p in
                                type.GetProperties(
                                    BindingFlags.Public |
                                    BindingFlags.NonPublic |
                                    BindingFlags.Instance |
                                    BindingFlags.FlattenHierarchy)
                            where p.CanRead && p.CanWrite && p.GetSetMethod() != null &&
                            p.GetSetMethod().IsVirtual &&
                            (p.GetSetMethod().IsPublic ||
                            p.GetSetMethod().IsFamily)
                            select p;
                props.ToList().ForEach(
                (item) => WrapMethod(
                item, raisePropertyChanged, typeBuilder));

                var newtype = typeBuilder.CreateType();
                var exp = Expression.Lambda(typeof(Func<>).MakeGenericType(type), Expression.New(newtype));

                Types[type] = ret = (Func<object>)exp.Compile();
            }
            return ret;
        }


        private static FieldBuilder CreatePropertyChangedEvent(TypeBuilder typeBuilder)
        {
            // public event PropertyChangedEventHandler PropertyChanged;

            FieldBuilder eventField =
                typeBuilder.DefineField("PropertyChanged",
                typeof(PropertyChangedEventHandler),
                FieldAttributes.Private);
            EventBuilder eventBuilder =
                typeBuilder.DefineEvent(
                "PropertyChanged",
                EventAttributes.None,
                typeof(PropertyChangedEventHandler));

            eventBuilder.SetAddOnMethod(
            CreateAddRemoveMethod(typeBuilder, eventField, true));
            eventBuilder.SetRemoveOnMethod(
            CreateAddRemoveMethod(typeBuilder, eventField, false));

            return eventField;
        }
        private static MethodBuilder CreateAddRemoveMethod(TypeBuilder typeBuilder, FieldBuilder eventField, bool isAdd)
        {
            string prefix = "remove_";
            string delegateAction = "Remove";
            if (isAdd)
            {
                prefix = "add_";
                delegateAction = "Combine";
            }
            MethodBuilder addremoveMethod =
            typeBuilder.DefineMethod(prefix + "PropertyChanged",
               MethodAttributes.Public |
               MethodAttributes.SpecialName |
               MethodAttributes.NewSlot |
               MethodAttributes.HideBySig |
               MethodAttributes.Virtual |
               MethodAttributes.Final,
               null,
               new[] { typeof(PropertyChangedEventHandler) });
            MethodImplAttributes eventMethodFlags =
                MethodImplAttributes.Managed |
                MethodImplAttributes.Synchronized;
            addremoveMethod.SetImplementationFlags(eventMethodFlags);

            ILGenerator ilGen = addremoveMethod.GetILGenerator();

            // PropertyChanged += value; // PropertyChanged -= value;
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldfld, eventField);
            ilGen.Emit(OpCodes.Ldarg_1);
            ilGen.EmitCall(OpCodes.Call,
                typeof(Delegate).GetMethod(
                delegateAction,
                new[] { typeof(Delegate), typeof(Delegate) }),
                null);
            ilGen.Emit(OpCodes.Castclass, typeof(
            PropertyChangedEventHandler));
            ilGen.Emit(OpCodes.Stfld, eventField);
            ilGen.Emit(OpCodes.Ret);

            MethodInfo intAddRemoveMethod =
            typeof(INotifyPropertyChanged).GetMethod(
            prefix + "PropertyChanged");
            typeBuilder.DefineMethodOverride(
            addremoveMethod, intAddRemoveMethod);

            return addremoveMethod;
        }
        private static MethodBuilder CreateRaisePropertyChanged(TypeBuilder typeBuilder, FieldBuilder eventField)
        {
            MethodBuilder raisePropertyChangedBuilder =
                typeBuilder.DefineMethod(
                "RaisePropertyChanged",
                MethodAttributes.Family | MethodAttributes.Virtual,
                null, new Type[] { typeof(string) });

            ILGenerator raisePropertyChangedIl =
            raisePropertyChangedBuilder.GetILGenerator();
            Label labelExit = raisePropertyChangedIl.DefineLabel();

            // if (PropertyChanged == null)
            // {
            //      return;
            // }
            raisePropertyChangedIl.Emit(OpCodes.Ldarg_0);
            raisePropertyChangedIl.Emit(OpCodes.Ldfld, eventField);
            raisePropertyChangedIl.Emit(OpCodes.Ldnull);
            raisePropertyChangedIl.Emit(OpCodes.Ceq);
            raisePropertyChangedIl.Emit(OpCodes.Brtrue, labelExit);

            // this.PropertyChanged(this,
            // new PropertyChangedEventArgs(propertyName));
            raisePropertyChangedIl.Emit(OpCodes.Ldarg_0);
            raisePropertyChangedIl.Emit(OpCodes.Ldfld, eventField);
            raisePropertyChangedIl.Emit(OpCodes.Ldarg_0);
            raisePropertyChangedIl.Emit(OpCodes.Ldarg_1);
            raisePropertyChangedIl.Emit(OpCodes.Newobj,
                typeof(PropertyChangedEventArgs)
                .GetConstructor(new[] { typeof(string) }));
            raisePropertyChangedIl.EmitCall(OpCodes.Callvirt,
                typeof(PropertyChangedEventHandler)
                .GetMethod("Invoke"), null);

            // return;
            raisePropertyChangedIl.MarkLabel(labelExit);
            raisePropertyChangedIl.Emit(OpCodes.Ret);

            return raisePropertyChangedBuilder;
        }
        static public bool ArrayEquals(byte[] p1, byte[] p2)
        {
            if (object.ReferenceEquals(p1, p2))
                return true;
            if (p1 == null || p1 == null)
                return p1 == null && p2 == null;
            if (p1.Length == p2.Length)
            {

                for (int i = 0; i < p1.Length; i++)
                    if (p1[i] != p2[i])
                        return false;
                return true;
            }
            return false;
        }
        private static void WrapMethod(PropertyInfo item, MethodInfo raisePropertyChanged, TypeBuilder typeBuilder)
        {

            MethodInfo setMethod = item.GetSetMethod();

            MethodInfo getMethod = item.GetGetMethod();

            var notfiychanging = item.DeclaringType.GetMethod("NotifyChanging", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            //get an array of the parameter types.
            var types = from t in setMethod.GetParameters()
                        select t.ParameterType;

            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod(
                setMethod.Name, setMethod.Attributes,
                setMethod.ReturnType, types.ToArray());
            typeBuilder.DefineMethodOverride(
            setMethodBuilder, setMethod);

            ILGenerator setMethodWrapperIl = setMethodBuilder.GetILGenerator();
            var labelExit = setMethodWrapperIl.DefineLabel();

            //if(this.property != value)

            setMethodWrapperIl.Emit(OpCodes.Ldarg_0);
            setMethodWrapperIl.EmitCall(OpCodes.Call, getMethod, null);

            setMethodWrapperIl.Emit(OpCodes.Ldarg_1);
            if (item.PropertyType == typeof(string))
            {
                setMethodWrapperIl.Emit(OpCodes.Call, typeof(String).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) }));
                setMethodWrapperIl.Emit(OpCodes.Ldc_I4, 1);

            }
            else if (item.PropertyType == typeof(byte[]))
            {
                setMethodWrapperIl.Emit(OpCodes.Call, typeof(ProperyChangeProxy).GetMethod("ArrayEquals",
                    BindingFlags.Public | BindingFlags.Static));
                setMethodWrapperIl.Emit(OpCodes.Ldc_I4, 1);

            }
            setMethodWrapperIl.Emit(OpCodes.Ceq);
            setMethodWrapperIl.Emit(OpCodes.Brtrue, labelExit);
            if (notfiychanging != null)
            {
                setMethodWrapperIl.Emit(OpCodes.Ldarg_0);
                setMethodWrapperIl.Emit(OpCodes.Ldstr, item.Name);
                setMethodWrapperIl.EmitCall(
                OpCodes.Call, notfiychanging, null);
            }
            // setMethodWrapperIl.Emit(
            // base.[PropertyName] = value;
            setMethodWrapperIl.Emit(OpCodes.Ldarg_0);
            setMethodWrapperIl.Emit(OpCodes.Ldarg_1);
            setMethodWrapperIl.EmitCall(
            OpCodes.Call, setMethod, null);

            // RaisePropertyChanged("[PropertyName]");
            setMethodWrapperIl.Emit(OpCodes.Ldarg_0);
            setMethodWrapperIl.Emit(OpCodes.Ldstr, item.Name);
            setMethodWrapperIl.EmitCall(
            OpCodes.Call, raisePropertyChanged, null);

            setMethodWrapperIl.MarkLabel(labelExit);
            // return;
            setMethodWrapperIl.Emit(OpCodes.Ret);
        }
    }

    public class DynamicBindingProxy<T> : DynamicObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static readonly Dictionary<string, Dictionary<string, PropertyInfo>> properties =
            new Dictionary<string, Dictionary<string, PropertyInfo>>();
        private readonly T instance;
        private readonly string typeName;

        public DynamicBindingProxy(T instance)
        {
            this.instance = instance;
            var type = typeof(T);
            typeName = type.FullName;
            if (!properties.ContainsKey(typeName))
                SetProperties(type, typeName);
        }

        private static void SetProperties(Type type, string typeName)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var dict = props.ToDictionary(prop => prop.Name);
            properties.Add(typeName, dict);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (properties[typeName].ContainsKey(binder.Name))
            {
                result = properties[typeName][binder.Name].GetValue(instance, null);
                return true;
            }
            result = null;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (properties[typeName].ContainsKey(binder.Name))
            {
                properties[typeName][binder.Name].SetValue(instance, value, null);
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(binder.Name));
                return true;
            }
            return false;
        }
    }
}
