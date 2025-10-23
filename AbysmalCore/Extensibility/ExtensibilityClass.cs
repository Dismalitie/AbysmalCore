using AbysmalCore.Debugging;
using System.Reflection;

namespace AbysmalCore.Extensibility
{
    [DebugInfo("abysmal extensibility framework property", false)]
    public class ExtensibilityProperty
    {
        private enum propertyType
        {
            property,
            field,
            privateMember,
        }

        private object _info;
        private object _instance;
        private propertyType _type;

        public string Name { get; }
        public Type Type { get; }
        public bool IsPrivate => _type == propertyType.privateMember;
        public object? Value
        {
            get
            {
                if (_type == propertyType.property)
                {
                    PropertyInfo? inf = _info as PropertyInfo;
                    if (inf!.CanRead)
                        return inf.GetMethod?.Invoke(_instance, null);
                    else
                    {
                        AbysmalDebug.Log(this, Name + " is write-only, cannot get value in " + _instance.GetType().FullName);
                        return null;
                    }
                }
                else if (_type == propertyType.field)
                {
                    FieldInfo? inf = _info as FieldInfo;
                    return inf!.GetValue(_instance);
                }
                else return null;
            }
            set
            {
                if (_type == propertyType.property)
                {
                    PropertyInfo? inf = _info as PropertyInfo;
                    if (inf!.CanWrite)
                        inf.SetMethod?.Invoke(_instance, [value]);
                    else AbysmalDebug.Log(this, Name + " is read-only, cannot set value in " + _instance.GetType().FullName);
                }
                else if (_type == propertyType.field)
                {
                    FieldInfo? inf = _info as FieldInfo;
                    if (inf!.IsLiteral || inf!.IsInitOnly)
                        AbysmalDebug.Log(this, Name + " is read-only, cannot set value in " + _instance.GetType().FullName);
                    else inf.SetValue(_instance, value);
                }
            }
        }

        public ExtensibilityProperty(PropertyInfo pi, object instance)
        {
            _type = propertyType.property;
            _instance = instance;
            _info = pi;

            Name = pi.Name;
            Type = pi.PropertyType;
        }

        public ExtensibilityProperty(FieldInfo fi, object instance)
        {
            if (fi.IsPublic) _type = propertyType.field;
            else _type = propertyType.privateMember;

            _instance = instance;
            _info = fi;

            Name = fi.Name;
            Type = fi.FieldType;
        }
    }

    [DebugInfo("abysmal extensibility framework method", false)]
    public class ExtensibilityMethod
    {
        private MethodInfo _info;
        private object _instance;

        public string Name { get; }
        public Type ReturnType { get; }
        public int ParameterCount { get; }

        public ExtensibilityMethod(MethodInfo mi, object instance)
        {
            _info = mi;
            _instance = instance;

            Name = mi.Name;
            ReturnType = mi.ReturnType;
            ParameterCount = mi.GetParameters().Length;
        }

        public Type GetParameterType(int index) => _info.GetParameters()[index].ParameterType;

        public object? Invoke(params object[] args)
        {
            AbysmalDebug.Log(this, $"Invoking method {Name} with {args.Length} arguments");
            return _info.Invoke(_instance, args);
        }
        public T Invoke<T>(params object[] args)
        {
            AbysmalDebug.Log(this, $"Invoking method {Name} with {args.Length} arguments and converting to {typeof(T).FullName}");
            return (T)_info.Invoke(_instance, args);
        }
        public T Invoke<T>(Func<object?, T> converter, params object[] args)
        {
            AbysmalDebug.Log(this, $"Invoking method {Name} with {args.Length} arguments and converting to {typeof(T).FullName} using custom converter");
            return converter(_info.Invoke(_instance, args));
        }
    }

    [DebugInfo("abysmal extensibility framework class", false)]
    public class ExtensibilityClass
    {
        public Dictionary<string, ExtensibilityProperty> Properties { get; }
        public Dictionary<string, ExtensibilityMethod> Methods { get; }
        public object Instance { get; }

        public ExtensibilityClass(Type t, bool getPrivate = false)
        {
            Properties = new();
            Methods = new();
            Instance = Activator.CreateInstance(t)!;

            /// always search for public instance members
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            /// only add NonPublic if getPrivate is true
            if (getPrivate)
                flag |= BindingFlags.NonPublic;

            foreach (PropertyInfo pi in Instance.GetType().GetProperties(flag))
                Properties[pi.Name] = new(pi, Instance);
            AbysmalDebug.Log(this, $"Found {Properties.Count} properties in class {Instance.GetType().FullName}");

            foreach (FieldInfo fi in Instance.GetType().GetFields(flag)) 
                Properties[fi.Name] = new(fi, Instance);
            AbysmalDebug.Log(this, $"Found {Properties.Count} properties (including fields) in class {Instance.GetType().FullName}");

            foreach (MethodInfo mi in Instance.GetType().GetMethods(flag))
                Methods[mi.Name] = new(mi, Instance);
            AbysmalDebug.Log(this, $"Found {Methods.Count} methods in class {Instance.GetType().FullName}");
        }

        public ExtensibilityClass(object instance, bool getPrivate = false)
        {
            Properties = new();
            Methods = new();
            Instance = instance;

            /// always search for public instance members
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            /// only add NonPublic if getPrivate is true
            if (getPrivate)
                flag |= BindingFlags.NonPublic;

            foreach (PropertyInfo pi in Instance.GetType().GetProperties(flag))
                Properties[pi.Name] = new(pi, Instance);
            AbysmalDebug.Log(this, $"Found {Properties.Count} properties in class {Instance.GetType().FullName}");

            foreach (FieldInfo fi in Instance.GetType().GetFields(flag))
                Properties[fi.Name] = new(fi, Instance);
            AbysmalDebug.Log(this, $"Found {Properties.Count} properties (including fields) in class {Instance.GetType().FullName}");

            foreach (MethodInfo mi in Instance.GetType().GetMethods(flag))
                Methods[mi.Name] = new(mi, Instance);
            AbysmalDebug.Log(this, $"Found {Methods.Count} methods in class {Instance.GetType().FullName}");
        }

        public bool HasMethod(string name) => Methods.ContainsKey(name);
        public bool HasProperty(string name) => Properties.ContainsKey(name);
        public object New() => Activator.CreateInstance(Instance.GetType())!;
    }
}
