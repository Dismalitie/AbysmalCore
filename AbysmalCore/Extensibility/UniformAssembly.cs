using AbysmalCore.Debugging;
using System.Reflection;

namespace AbysmalCore.Extensibility
{
    /// <summary>
    /// Encapsulates an assembly and its properties into a uniform interaction context
    /// </summary>
    /// <remarks>
    /// IMPORTANT: Any time a class needs to be retrieved by string,
    /// it will always be its full name (e.g: Tests.TestClass, not TestClass).
    /// </remarks>
    [DebugInfo("represents an assembly in the extensibility framework")]
    public class UniformAssembly
    {
        private Assembly _assembly;

        /// <summary>
        /// Contains all the public classes in the assembly
        /// </summary>
        /// <remarks>Key is the name of the class</remarks>
        public Dictionary<string, UniformClass> Classes { get; } = new();
        /// <summary>
        /// The name of the assembly
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new uniform assembly
        /// </summary>
        public UniformAssembly(Assembly asm, bool getPrivate = false)
        {
            _assembly = asm;
            Name = asm.GetName().Name ?? asm.FullName ?? asm.GetName().FullName;

            Type[] t = getPrivate ? asm.GetTypes() : asm.GetExportedTypes();
            foreach (Type cls in t)
            {
                string? name = cls.FullName ?? cls.Name;
                if (cls.IsClass) Classes.Add(name, new(cls, getPrivate));
            }
        }

        /// <summary>
        /// Checks whether this assembly has declared a class with <paramref name="name"/>
        /// </summary>
        /// <param name="name">Name of the class</param>
        public bool HasClass(string name) => Classes.ContainsKey(name);

        /// <summary>
        /// Returns the class with <paramref name="name"/>
        /// </summary>
        /// <param name="name">Name of the class</param>
        /// <returns>null if not found</returns>
        public UniformClass? GetClass(string name)
        {
            if (HasClass(name)) return Classes[name];
            else return null;
        }

        /// <summary>
        /// Invokes the entrypoint of an assembly
        /// </summary>
        /// <param name="args">The string command line args</param>
        /// <param name="output">What the entrypoint returns if declared</param>
        /// <returns>false if no entrypoint found, else true</returns>
        public bool InvokeEntrypoint(string[]? args, out object? output)
        {
            if (_assembly.EntryPoint == null)
            {
                AbysmalDebug.Log(this, $"{_assembly.FullName} has no entry point, aborting");
                output = null;
                return false;
            }

            UniformMethod method = new(_assembly.EntryPoint, _assembly);
            // exclamation mark here because if its null its fine
            output = method.Invoke(args!);
            return true;
        }
    }
}
