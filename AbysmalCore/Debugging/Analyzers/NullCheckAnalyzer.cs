using System.Reflection;

namespace AbysmalCore.Debugging.Analyzers
{
    /// <summary>
    /// Checks members for null returns
    /// </summary>
    /// <typeparam name="T">Type to check</typeparam>
    public abstract class NullCheckAnalyzer<T> : Analyzer<T, NullCheckAnalyzer<T>> where T : class
    {
        static readonly NullabilityInfoContext _ctx = new();

        /// <inheritdoc/>
        public override void AnalyzeMethods(MethodInfo[] methods)
        {
            foreach (MethodInfo m in methods)
            {
                NullabilityInfo info = _ctx.Create(m.ReturnParameter);
                Type parameterType = m.ReturnType;

                bool n = Nullable.GetUnderlyingType(parameterType) != null;
                if (n != (info.ReadState == NullabilityState.Nullable)) n = true;

                if (n) Log($"/!\\ Method {m.Name} has a nullable return type");
            }
        }

        /// <inheritdoc/>
        public override void AnalyzeProperties(PropertyInfo[] properties)
        {
            foreach (PropertyInfo p in properties)
            {
                NullabilityInfo info = _ctx.Create(p);
                Type parameterType = p.PropertyType;

                bool n = Nullable.GetUnderlyingType(parameterType) != null;
                if (n != (info.ReadState == NullabilityState.Nullable)) n = true;

                if (n) Log($"/!\\ Property {p.Name} has a nullable return type");
            }
        }
    }
}