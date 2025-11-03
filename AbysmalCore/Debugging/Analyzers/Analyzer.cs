using AbysmalCore.Components;
using System.Reflection;

namespace AbysmalCore.Debugging.Analyzers
{
    /// <summary>
    /// Base class for custom analyzers
    /// </summary>
    public abstract class Analyzer<T, TAnalyzer> where T : class where TAnalyzer : Analyzer<T, TAnalyzer>
    {
        private static Dictionary<Type, Type> _analyzed = new();

        private string _tName = typeof(T).Name;
        private string _tAnalyzerName = typeof(TAnalyzer).Name;
        private Type _tType = typeof(T);
        private Type _tAnalyzerType = typeof(TAnalyzer);

        /// <summary>
        /// Initiates analysis
        /// </summary>
        protected Analyzer()
        {
            if (_analyzed.ContainsKey(_tType) == false || _analyzed[_tType] != _tAnalyzerType)
            {
                AbysmalDebug.SetLogRegion($"{_tAnalyzerName} of {GetType().FullName ?? GetType().Name}");

                AnalyzeMethods(meths(_tType));
                AnalyzeProperties(props(_tType));

                AbysmalDebug.UnsetLogRegion();
                _analyzed.Add(_tType, _tAnalyzerType);
            }
        }

        /// <summary>
        /// Analyzes all methods in the derived class
        /// </summary>
        public virtual void AnalyzeMethods(MethodInfo[] methods) => Log("No method analysis");
        /// <summary>
        /// Analyzes all properties in the derived class
        /// </summary>
        public virtual void AnalyzeProperties(PropertyInfo[] properties) => Log("No property analysis");

        /// <summary>
        /// Logs an analysis
        /// </summary>
        /// <param name="log">Log message</param>
        /// <remarks>
        /// Should not be used outside of analysis methods
        /// </remarks>
        protected void Log(string log) =>
            AbysmalDebug.Console.WriteColorLn($"[{_tAnalyzerName}][analysis:{_tName}] {log}", ConsoleColor.Magenta);

        private PropertyInfo[] props(Type t) => t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        private MethodInfo[] meths(Type t) => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(m => !m.IsSpecialName).ToArray();
    }
}
