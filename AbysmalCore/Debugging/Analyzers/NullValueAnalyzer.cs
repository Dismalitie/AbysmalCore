using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.Debugging.Analyzers
{
    /// <summary>
    /// Checks return types of properties and fields and emits if they are nullable or have null values upon init
    /// </summary>
    public class NullValueAnalyzer : IAnalyzer
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

                if (p.CanRead)
                {
                    if (p.GetValue(Activator.CreateInstance(ParentClass)) == null && !n)
                        Log($"/!\\ Property {p.Name} has a null value upon class initialization even though it is not nullable");
                    else if (p.GetValue(Activator.CreateInstance(ParentClass)) == null)
                        Log($"/!\\ Property {p.Name} has a null value upon class initialization");
                    else if (n)
                        Log($"/!\\ Property {p.Name} has a nullable return type");
                }
            }
        }
        /// <inheritdoc/>
        public override void AnalyzeFields(FieldInfo[] fields)
        {
            foreach (FieldInfo f in fields)
            {
                NullabilityInfo info = _ctx.Create(f);
                Type parameterType = f.FieldType;

                bool n = Nullable.GetUnderlyingType(parameterType) != null;
                if (n != (info.ReadState == NullabilityState.Nullable)) n = true;

                if (f.GetValue(Activator.CreateInstance(ParentClass)) == null && !n)
                    Log($"/!\\ Field {f.Name} has a null value upon class initialization even though it is not nullable");
                else if (f.GetValue(Activator.CreateInstance(ParentClass)) == null)
                    Log($"/!\\ Field {f.Name} has a null value upon class initialization");
                else if (n)
                    Log($"/!\\ Field {f.Name} has a nullable return type");
            }
        }
    }
}
