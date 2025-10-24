using AbysmalCore.Debugging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;

namespace AbysmalCore.Extensibility
{
    /// <summary>
    /// Compiler and wrapper initializer
    /// </summary>
    [DebugInfo("abysmal extensibility framework", false)]
    public class AbysmalExtensibility
    {
        /// <summary>
        /// Compiles C# source code into an assembly
        /// </summary>
        /// <param name="src">Source code to compile</param>
        public static Assembly CompileAssembly(string src)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(src);
            string assemblyName = Path.GetRandomFileName();

            // get references from host assembly
            List<PortableExecutableReference> references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .ToList();

            AbysmalDebug.Log(new AbysmalExtensibility(), $"Compiling assembly <anonymous>.{assemblyName} with {references.Count} references");
            Stopwatch sw = Stopwatch.StartNew();
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                [syntaxTree],
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // compile to memory stream
            using MemoryStream ms = new();
            EmitResult result = compilation.Emit(ms);
            sw.Stop();
            AbysmalDebug.Log(new AbysmalExtensibility(), $"Compilation {(result.Success ? "succeeded" : "failed")} for assembly <anonymous>.{assemblyName} in {sw.ElapsedMilliseconds}ms");

            if (!result.Success)
                AbysmalDebug.Error(new AbysmalExtensibility(), "Compilation failed.", true);

            // go to beginning of stream
            ms.Seek(0, SeekOrigin.Begin);

            // load the assembly and return it
            Assembly asm = AssemblyLoadContext.Default.LoadFromStream(ms);

            return asm;
        }

        /// <summary>
        /// Returns an instance of a class from a compiled assembly that is matched to an interface or abstract class
        /// </summary>
        /// <typeparam name="T">Interface or abstract class type</typeparam>
        /// <param name="asm">The host assembly</param>
        /// <param name="cls">The class name to instantiate</param>
        public static T GetUniformClass<T>(Assembly asm, string cls)
        {
            Type? type = asm.GetType(cls)!;
            return (T)Activator.CreateInstance(type)!;
        }

        /// <summary>
        /// Returns an ExtensibilityClass instance for uniform access to methods and properties of a class in a compiled assembly
        /// </summary>
        /// <param name="asm">The host assembly</param>
        /// <param name="cls">The class name to instantiate</param>
        /// <param name="getPrivate">Whether to include private members</param>
        public static AbysmalExtensibilityClass GetClass(Assembly asm, string cls, bool getPrivate = false)
        {
            Type? type = asm.GetType(cls)!;
            return new(type, getPrivate);
        }
    }
}
