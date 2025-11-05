using AbysmalCore.Components;
using System.Linq;
using System.Reflection;

namespace AbysmalCore.Debugging
{
    internal class InternalAnalysisLogs
    {
        private static Dictionary<(Type cls, Type analyzer), List<string>> _logs = new();
        public static void AddLog(Type cls, Type analyzer, string log)
        {
            (Type cls, Type analyzer) tupleKey = (cls, analyzer);

            if (_logs.ContainsKey(tupleKey)) _logs[tupleKey].Add(log);
            else _logs.Add(tupleKey, [log]);
        }

        public static string[] GetLogs(Type cls, Type analyzer) => _logs.ContainsKey((cls, analyzer)) ? _logs[(cls, analyzer)].ToArray() : [];
    }

    /// <summary>
    /// Static accessor to analyzing <typeparamref name="TClass"/> with <typeparamref name="TAnalyzer"/>
    /// </summary>
    /// <typeparam name="TClass">Class to analyze</typeparam>
    /// <typeparam name="TAnalyzer">Analyzer to use</typeparam>
    public class Analyzer<TClass, TAnalyzer> : InstantiableComponent<Analyzer<TClass, TAnalyzer>> where TAnalyzer : IAnalyzer, new() where TClass : class
    {
        private static PropertyInfo[] props(Type t) => t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        private static MethodInfo[] meths(Type t) => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(m => !m.IsSpecialName).ToArray();
        private static FieldInfo[] fields(Type t) => t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Analyzes the hosting class of the members
        /// </summary>
        public static void AnalyzeParentClass() => new TAnalyzer() { ParentClass = typeof(TClass) }.AnalyzeParentClass(typeof(TClass));
        /// <summary>
        /// Analyzes all public methods in a class
        /// </summary>
        public static void AnalyzeMethods() => new TAnalyzer() { ParentClass = typeof(TClass) }.AnalyzeMethods(meths(typeof(TClass)));
        /// <summary>
        /// Analyzes all public properties in a class
        /// </summary>
        public static void AnalyzeProperties() => new TAnalyzer() { ParentClass = typeof(TClass) }.AnalyzeProperties(props(typeof(TClass)));
        /// <summary>
        /// Analyzes all public fields in a class
        /// </summary>
        public static void AnalyzeFields() => new TAnalyzer() { ParentClass = typeof(TClass) }.AnalyzeFields(fields(typeof(TClass)));
        /// <summary>
        /// Analyzes all members in a class
        /// </summary>
        public static void AnalyzeMembers()
        {
            AnalyzeParentClass();
            AnalyzeMethods();
            AnalyzeProperties();
            AnalyzeFields();
        }

        /// <summary>
        /// Emits all logs to the console
        /// </summary>
        public static void Emit()
        {
            string[] logs = InternalAnalysisLogs.GetLogs(typeof(TClass), typeof(TAnalyzer));

            AbysmalDebug.SetLogRegion($"{typeof(TAnalyzer).Name} emitted analytics of {typeof(TClass)}");

            if (logs.Length == 0) AbysmalDebug.Warn(_this, $"No logs to emit for this analyzer");
            else
            {
                foreach (string log in logs)
                {
                    if (log.Contains("/!\\"))
                        AbysmalDebug.Console.WriteColorLn(log, ConsoleColor.Yellow);
                    else if (log.Contains("(!)"))
                        AbysmalDebug.Console.WriteColorLn(log, ConsoleColor.Red);
                    else if (log.Contains("[!]"))
                        AbysmalDebug.Console.WriteColorLn(log, ConsoleColor.DarkRed);
                    else if (log.Contains("(i)"))
                        AbysmalDebug.Console.WriteColorLn(log, ConsoleColor.DarkGray);
                    else AbysmalDebug.Console.WriteColorLn(log, ConsoleColor.Magenta);
                }
            }

            AbysmalDebug.UnsetLogRegion();
        }

        /// <summary>
        /// Emits all logs to a <see cref="string"/>[]
        /// </summary>
        public static string[] Emit(bool returnLogs) => InternalAnalysisLogs.GetLogs(typeof(TClass), typeof(TAnalyzer));
    }
}
