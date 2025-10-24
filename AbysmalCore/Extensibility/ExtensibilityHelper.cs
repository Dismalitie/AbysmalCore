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
    public class ExtensibilityHelper
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

            AbysmalDebug.Log(new ExtensibilityHelper(), $"Compiling assembly <anonymous>.{assemblyName} with {references.Count} references");
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
            AbysmalDebug.Log(new ExtensibilityHelper(), $"Compilation {(result.Success ? "succeeded" : "failed")} for assembly <anonymous>.{assemblyName} in {sw.ElapsedMilliseconds}ms");

            if (!result.Success)
                AbysmalDebug.Error(new ExtensibilityHelper(), "Compilation failed.", true);

            // go to beginning of stream
            ms.Seek(0, SeekOrigin.Begin);

            // load the assembly and return it
            Assembly asm = AssemblyLoadContext.Default.LoadFromStream(ms);

            return asm;
        }

        /// <summary>
        /// Encapsulates assembly <paramref name="asm"/> into a new <see cref="UniformAssembly"/>
        /// </summary>
        /// <param name="asm">The assembly to encapsulate</param>
        /// <param name="getPrivate">Determines whether to expose private items in the assembly</param>
        public static UniformAssembly LoadAssembly(Assembly asm, bool getPrivate = false) => new(asm, getPrivate);
    }
}
