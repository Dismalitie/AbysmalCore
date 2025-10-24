using AbysmalCore.Debugging;
using System.Reflection;

namespace AbysmalCore.Extensibility
{
    /// <summary>
    /// Defines a property or field in the Abysmal Extensibility Framework
    /// </summary>
    [DebugInfo("abysmal extensibility framework property", false)]
    public class UniformProperty
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

        /// <summary>
        /// The name of the property or field
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The type of the property or field
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// Whether the property or field is private
        /// </summary>
        public bool IsPrivate => _type == propertyType.privateMember;
        /// <summary>
        /// Gets and sets the value of the property or field
        /// </summary>
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

        /// <summary>
        /// Creates a new AbysmalExtensibilityProperty wrapping the specified PropertyInfo
        /// </summary>
        /// <param name="pi">The PropertyInfo to wrap</param>
        /// <param name="instance">The instance of the underlying class</param>
        public UniformProperty(PropertyInfo pi, object instance)
        {
            _type = propertyType.property;
            _instance = instance;
            _info = pi;

            Name = pi.Name;
            Type = pi.PropertyType;
        }

        /// <summary>
        /// Creates a new AbysmalExtensibilityProperty wrapping the specified FieldInfo
        /// </summary>
        /// <param name="fi">The FieldInfo to wrap</param>
        /// <param name="instance">The instance of the underlying class</param>
        public UniformProperty(FieldInfo fi, object instance)
        {
            if (fi.IsPublic) _type = propertyType.field;
            else _type = propertyType.privateMember;

            _instance = instance;
            _info = fi;

            Name = fi.Name;
            Type = fi.FieldType;
        }
    }

    /// <summary>
    /// Defines a method in the Abysmal Extensibility Framework
    /// </summary>
    [DebugInfo("abysmal extensibility framework method", false)]
    public class UniformMethod
    {
        private MethodInfo _info;
        private object _instance;

        /// <summary>
        /// The name of the method
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// What type the method returns
        /// </summary>
        public Type ReturnType { get; }
        /// <summary>
        /// The number of parameters the method takes
        /// </summary>
        public int ParameterCount { get; }

        /// <summary>
        /// Creates a new AbysmalExtensibilityMethod wrapping the specified MethodInfo
        /// </summary>
        /// <param name="mi">The MethodInfo to wrap</param>
        /// <param name="instance">The instance of the underlying class</param>
        public UniformMethod(MethodInfo mi, object instance)
        {
            _info = mi;
            _instance = instance;

            Name = mi.Name;
            ReturnType = mi.ReturnType;
            ParameterCount = mi.GetParameters().Length;
        }

        /// <summary>
        /// Returns the type of the parameter at the specified index
        /// </summary>
        /// <param name="index">The index of the parameter</param>
        public Type GetParameterType(int index) => _info.GetParameters()[index].ParameterType;

        /// <summary>
        /// Executes the method with the specified arguments
        /// </summary>
        /// <param name="args">The arguments to pass to the method</param>
        public object? Invoke(params object[] args)
        {
            AbysmalDebug.Log(this, $"Invoking method {Name} with {args.Length} arguments");
            return _info.Invoke(_instance, args);
        }
        /// <summary>
        /// Invokes the method with the specified arguments and converts the result to the specified type
        /// </summary>
        /// <typeparam name="T">The type to convert the result to</typeparam>
        /// <param name="args">Arguments to pass to the method</param>
        /// <returns></returns>
        public T Invoke<T>(params object[] args)
        {
            AbysmalDebug.Log(this, $"Invoking method {Name} with {args.Length} arguments and converting to {typeof(T).FullName}");
            return (T)_info.Invoke(_instance, args)!;
        }
        /// <summary>
        /// Invokes the method with the specified arguments and converts the result using the provided converter function
        /// </summary>
        /// <typeparam name="T">The type to convert the result to</typeparam>
        /// <param name="converter">Lambda function to convert the result</param>
        /// <param name="args">Arguments to pass to the method</param>
        public T Invoke<T>(Func<object?, T> converter, params object[] args)
        {
            AbysmalDebug.Log(this, $"Invoking method {Name} with {args.Length} arguments and converting to {typeof(T).FullName} using custom converter");
            return converter(_info.Invoke(_instance, args));
        }
    }

    /// <summary>
    /// A uniform wrapper for classes used defined in reflected assemblies
    /// </summary>
    [DebugInfo("abysmal extensibility framework class", false)]
    public class UniformClass
    {
        /// <summary>
        /// Properties of the class
        /// </summary>
        public Dictionary<string, UniformProperty> Properties { get; }
        /// <summary>
        /// Methods of the class
        /// </summary>
        public Dictionary<string, UniformMethod> Methods { get; }
        /// <summary>
        /// The instance of the underlying class
        /// </summary>
        public object Instance { get; }

        /// <summary>
        /// Creates a new ExtensibilityClass wrapping the specified type
        /// </summary>
        /// <param name="t">The type to wrap</param>
        /// <param name="getPrivate">Whether to include private members</param>
        public UniformClass(Type t, bool getPrivate = false)
        {
            Properties = new();
            Methods = new();
            Instance = Activator.CreateInstance(t)!;

            // always search for public instance members
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            // only add NonPublic if getPrivate is true
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

        /// <summary>
        /// Creates a new ExtensibilityClass wrapping the specified instance
        /// </summary>
        /// <param name="instance">The instance to wrap</param>
        /// <param name="getPrivate">Whether to include private members</param>
        public UniformClass(object instance, bool getPrivate = false)
        {
            Properties = new();
            Methods = new();
            Instance = instance;

            // always search for public instance members
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            // only add NonPublic if getPrivate is true
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

        /// <summary>
        /// Returns whether the class has a method with the specified name
        /// </summary>
        /// <param name="name">The name of the method</param>
        public bool HasMethod(string name) => Methods.ContainsKey(name);
        /// <summary>
        /// Returns whether the class has a property with the specified name
        /// </summary>
        /// <param name="name">The name of the property</param>
        public bool HasProperty(string name) => Properties.ContainsKey(name);

        /// <summary>
        /// Returns the method defined in this class with <paramref name="name"/>
        /// </summary>
        /// <param name="name">The name of the method</param>
        /// <returns>null if method undefined</returns>
        public UniformMethod? GetMethod(string name)
        {
            if (HasMethod(name)) return Methods[name];
            else return null;
        }
        /// <summary>
        /// Returns the method defined in this class with <paramref name="name"/>
        /// </summary>
        /// <param name="name">The name of the method</param>
        /// <returns>null if method undefined</returns>
        public UniformProperty? GetProperty(string name)
        {
            if (HasProperty(name)) return Properties[name];
            else return null;
        }

        /// <summary>
        /// Instantiates a new instance of the underlying class
        /// </summary>
        public object New() => Activator.CreateInstance(Instance.GetType())!;
        /// <summary>
        /// Returns an instance of this class that is derived from an interface or abstraction (<typeparamref name="T"/>)
        /// </summary>
        /// <typeparam name="T">Interface or abstract class type</typeparam>
        public T ToUniform<T>()
        {
            Type? type = Instance.GetType()!;
            return (T)Activator.CreateInstance(type)!;
        }
    }
}
