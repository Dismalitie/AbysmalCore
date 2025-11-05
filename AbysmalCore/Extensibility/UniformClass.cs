using AbysmalCore.Debugging;
using System.Reflection;

namespace AbysmalCore.Extensibility
{
    /// <summary>
    /// A uniform wrapper for classes used defined in reflected assemblies
    /// </summary>
    [DebugInfo("abysmal extensibility framework class", false)]
    public class UniformClass
    {
        private Type _t;

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
        /// <remarks>
        /// May be null if class is abstract
        /// </remarks>
        public object? Instance { get; }

        /// <summary>
        /// The name of the class
        /// </summary>
        public string Name { get => _t.FullName ?? _t.Name; }
        /// <summary>
        /// Whether the class was marked with the private keyword
        /// </summary>
        public bool IsPrivate { get => _t.IsNestedPrivate; }
        /// <summary>
        /// Whether the class was marked with the abstract keyword
        /// </summary>
        public bool IsAbstract { get => _t.IsAbstract; }
        /// <summary>
        /// Whether the class was marked with the internal keyword
        /// </summary>
        public bool IsInternal { get => !_t.IsNested && !_t.IsPublic; }
        /// <summary>
        /// Whether the class was marked with the sealed keyword
        /// </summary>
        public bool IsSealed { get =>  _t.IsSealed; }

        /// <summary>
        /// Creates a new ExtensibilityClass wrapping the specified type
        /// </summary>
        /// <param name="t">The type to wrap</param>
        /// <param name="getPrivate">Whether to include private members</param>
        public UniformClass(Type t, bool getPrivate = false)
        {
            Properties = new();
            Methods = new();
            if (!t.IsAbstract) Instance = Activator.CreateInstance(t)!;
            _t = t;

            // always search for public instance members
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            // only add NonPublic if getPrivate is true
            if (getPrivate)
                flag |= BindingFlags.NonPublic;

            if (Instance != null)
            {
                foreach (PropertyInfo pi in Instance!.GetType().GetProperties(flag))
                    Properties[pi.Name] = new(pi, Instance);
                AbysmalDebug.Log(this, $"Found {Properties.Count} properties in class {Instance.GetType().FullName}");

                foreach (FieldInfo fi in Instance!.GetType().GetFields(flag))
                    Properties[fi.Name] = new(fi, Instance);
                AbysmalDebug.Log(this, $"Found {Properties.Count} properties (including fields) in class {Instance.GetType().FullName}");

                foreach (MethodInfo mi in Instance!.GetType().GetMethods(flag))
                    Methods[mi.Name] = new(mi, Instance);
                AbysmalDebug.Log(this, $"Found {Methods.Count} methods in class {Instance.GetType().FullName}");
            }
            else AbysmalDebug.Log(this, $"Instance {Name} was null, likely abstract, no methods or properties were indexed");
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
            _t = instance.GetType();

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
        public object New() => Activator.CreateInstance(Instance!.GetType())!;
        /// <summary>
        /// Returns an instance of this class that is derived from an interface or abstraction (<typeparamref name="T"/>)
        /// </summary>
        /// <typeparam name="T">Interface or abstract class type</typeparam>
        public T DeriveFrom<T>()
        {
            Type? type = Instance!.GetType()!;
            return (T)Activator.CreateInstance(type)!;
        }
    }
}
