using AbysmalCore.Components;
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
    public class ExtensibilityHelper : InstantiableComponent<ExtensibilityHelper>
    {
        /// <summary>
        /// Compiles C# source code into an assembly
        /// </summary>
        /// <param name="src">Source code to compile</param>
        /// <param name="diagnostics">List of warnings and errors produced during compilation</param>
        /// <param name="assemblies">Array of string references to assembly references</param>
        /// <param name="deriveHost">Whether to inherit the currently loaded assemblies of this <see cref="AppDomain"/></param>
        /// <returns>null if compilation failed, see <paramref name="diagnostics"/></returns>
        public static Assembly? CompileAssemblyFromString(string src, out string[] diagnostics, string[]? assemblies = null, bool deriveHost = true)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(src);
            string assemblyName = Path.GetRandomFileName();

            // get references from host assembly
            List<PortableExecutableReference> references = new();
            if (deriveHost) references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .ToList();
            if (assemblies != null)
                references = (List<PortableExecutableReference>)references
                    .Concat(assemblies
                    .Select(a => MetadataReference.CreateFromFile(Path.GetFullPath(a))));

            AbysmalDebug.Log(_this, $"Compiling assembly <anonymous>.{assemblyName} with {references.Count} references");
            Stopwatch sw = Stopwatch.StartNew();
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                [syntaxTree],
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            // compile to memory stream
            using MemoryStream ms = new();
            EmitResult result = compilation.Emit(ms);
            sw.Stop();
            AbysmalDebug.Log(_this, $"Compilation {(result.Success ? "succeeded" : "failed")} for assembly <anonymous>.{assemblyName} in {sw.ElapsedMilliseconds}ms");

            diagnostics = result.Diagnostics.Select(d => $"[{d.Severity}][{d.Id}] {d.GetMessage()}").ToArray();

            if (!result.Success)
            {
                AbysmalDebug.Error(_this, $"Compilation failed for assembly <anonymous>.{assemblyName}");
                return null;
            }

            // go to beginning of stream
            ms.Seek(0, SeekOrigin.Begin);

            // load the assembly and return it
            Assembly asm = AssemblyLoadContext.Default.LoadFromStream(ms);

            return asm;
        }

        /// <summary>
        /// What type of file to read the source from
        /// </summary>
        public enum AssemblyFileType
        {
            /// <summary>
            /// Represents an assembly in the Global Assembly Cache
            /// </summary>
            /// <remarks>
            /// In .NET Core / .NET 5+, the exe does not contain the IL code, 
            /// and is rather a launcher for the accompanying DLL. Trying to
            /// load the exe will result in a <see cref="BadImageFormatException"/>
            /// </remarks>
            Executable,
            /// <summary>
            /// Represents a compiled .dll file
            /// </summary>
            DynamicLinkLibrary,
            /// <summary>
            /// Represents an uncompiled C# source file
            /// </summary>
            Source,
        }

        /// <summary>
        /// Reads the C# source contents of a file and compiles it to an <see cref="Assembly"/>
        /// </summary>
        /// <param name="path">The filepath of the assembly</param>
        /// <param name="type">The way to read the file</param>
        /// <param name="diagnostics">List of warnings and errors produced during compilation</param>
        /// <param name="assemblies">Array of string references to assembly references</param>
        /// <param name="deriveHost">Whether to inherit the currently loaded assemblies of this <see cref="AppDomain"/></param>
        public static Assembly? CompileAssemblyFromFile(string path, AssemblyFileType type, out string[] diagnostics, string[]? assemblies = null, bool deriveHost = true)
        {
            if (type == AssemblyFileType.Source)
            {
                string file = File.ReadAllText(path);
                return CompileAssemblyFromString(path, out diagnostics, assemblies, deriveHost);
            }
            // the only 2 options left is exe and dll, which both use the same loader
            else
            {
                diagnostics = Array.Empty<string>();
                return Assembly.LoadFrom(path);
            }
        }

        /// <summary>
        /// Encapsulates assembly <paramref name="asm"/> into a new <see cref="UniformAssembly"/>
        /// </summary>
        /// <param name="asm">The assembly to encapsulate</param>
        /// <param name="getPrivate">Determines whether to expose private items in the assembly</param>
        public static UniformAssembly LoadAssembly(Assembly asm, bool getPrivate = false) => new(asm, getPrivate);
    }
}
