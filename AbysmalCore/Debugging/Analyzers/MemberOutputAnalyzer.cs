using AbysmalCore.Components;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace AbysmalCore.Debugging.Analyzers
{
    /// <summary>
    /// Emits all member types, names and values (if applicable)
    /// </summary>
    public class MemberOutputAnalyzer : IAnalyzer
    {
        private void fmt(string name, string type, string? value = null)
        {
            if (value != null)
                Log($"{type}: {name} = {value} (upon init)");
            else
                Log($"{type}: {name}");
        }

        /// <inheritdoc/>
        public override void AnalyzeMethods(MethodInfo[] methods) { foreach (MethodInfo m in methods) fmt(m.Name, "Method"); }
        /// <inheritdoc/>
        public override void AnalyzeProperties(PropertyInfo[] properties) 
        {
            foreach (PropertyInfo p in properties)
                fmt(p.Name,
                    "Property",
                    p.CanRead ? (p.GetValue(Activator.CreateInstance(ParentClass)) != null ? p.GetValue(Activator.CreateInstance(ParentClass))!.ToString() : null) : null
                ); 
        }
        /// <inheritdoc/>
        public override void AnalyzeFields(FieldInfo[] fields)
        {
            foreach (FieldInfo f in fields)
                fmt(f.Name,
                    "Field",
                    f.GetValue(Activator.CreateInstance(ParentClass)) != null ? f.GetValue(Activator.CreateInstance(ParentClass))!.ToString() : null
                );
        }
    }
}
