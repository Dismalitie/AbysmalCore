using System;
using System.Reflection;


namespace AbysmalCore.Debugging
{
    /// <summary>
    /// Base implementation for an analyzer
    /// </summary>
    public class IAnalyzer
    {
        /// <summary>
        /// The class being analyzed
        /// </summary>
        public Type ParentClass
        {
            get => parentClass!;
            set
            {
                if (!init_parentClass)
                {
                    parentClass = value;
                    init_parentClass = true;
                }
            }
        }
        private Type? parentClass;
        private bool init_parentClass = false;

        /// <summary>
        /// Analyzes all methods in <see cref="ParentClass"/>
        /// </summary>
        /// <param name="methods"></param>
        public virtual void AnalyzeMethods(MethodInfo[] methods) => Log($"(i) No method analytics for {ParentClass.Name}");
        /// <summary>
        /// Analyzes all properties in <see cref="ParentClass"/>
        /// </summary>
        /// <param name="properties"></param>
        public virtual void AnalyzeProperties(PropertyInfo[] properties) => Log($"(i) No property analytics for {ParentClass.Name}");
        /// <summary>
        /// Analyzes all fields in <see cref="ParentClass"/>
        /// </summary>
        public virtual void AnalyzeFields(FieldInfo[] fields) => Log($"(i) No field analytics for {ParentClass.Name}");
        /// <summary>
        /// Analyzes the hosting class of the members
        /// </summary>
        /// <param name="parent">Equivalent to <see cref="ParentClass"/></param>
        public virtual void AnalyzeParentClass(Type parent) => Log($"(i) No class analytics for {ParentClass.Name}");

        /// <summary>
        /// Adds a log to this analyzer's buffer
        /// </summary>
        /// <param name="analytic"></param>
        public void Log(string analytic) => InternalAnalysisLogs.AddLog(parentClass!, GetType(), $"[{ParentClass.Name} -> {GetType().Name}] {analytic}");
    }
}
