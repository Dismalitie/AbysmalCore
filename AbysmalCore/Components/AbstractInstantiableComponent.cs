using AbysmalCore.Debugging;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace AbysmalCore.Components
{
    /// <summary>
    /// Spoofs a class of abstract type <typeparamref name="T"/> 
    /// with the same name and attributes and generates static
    /// accessor of aformentioned spoofed class
    /// </summary>
    /// <remarks>
    /// Members do not transfer
    /// 
    /// IMPORTANT: This component is extremely inefficient, and is advised
    /// to be cached in a property. It generates a new spoofed class each
    /// time <see cref="_this"/> is called, which can take anywhere from
    /// 20ms to 60ms. Use sparingly.
    /// </remarks>
    /// <typeparam name="T">The type to spoof</typeparam>
    public abstract class AbstractInstantiableComponent<T> where T : class
    {
        /// <summary>
        /// Returns a new instance of this class
        /// </summary>
        protected static object _this
        {
            get
            {
                Type oType = typeof(T);
                if (!oType.IsAbstract)
                    AbysmalDebug.Warn(oType, $"Class is not abstract, unnessecary overhead; Use {nameof(InstantiableComponent<object>)} instead", true);

                string tName = oType.Name;

                // spoof assembly and module
                AssemblyName assemblyName = new AssemblyName(oType.Assembly.GetName().Name ?? oType.Assembly.GetName().FullName);
                AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(oType.Module.Name);

                // spoof name here
                TypeBuilder typeBuilder = moduleBuilder.DefineType(
                    tName,
                    oType.Attributes);

                typeBuilder.SetParent(oType.BaseType);

                foreach (var attrData in oType.GetCustomAttributesData())
                {
                    try
                    {
                        // get args
                        object[] constructorArgs = attrData.ConstructorArguments
                            .Select(arg => arg.Value)
                            .ToArray();

                        // get properties
                        var namedProperties = attrData.NamedArguments
                            .Where(n => !n.IsField)
                            .Select(n => (PropertyInfo)n.MemberInfo)
                            .ToArray();

                        var propertyValues = attrData.NamedArguments
                            .Where(n => !n.IsField)
                            .Select(n => n.TypedValue.Value)
                            .ToArray();

                        var namedFields = attrData.NamedArguments
                            .Where(n => n.IsField)
                            .Select(n => (FieldInfo)n.MemberInfo)
                            .ToArray();

                        var fieldValues = attrData.NamedArguments
                            .Where(n => n.IsField)
                            .Select(n => n.TypedValue.Value)
                            .ToArray();

                        // instatiate with cloned attrib props
                        var attributeBuilder = new CustomAttributeBuilder(
                            attrData.Constructor,
                            constructorArgs,
                            namedProperties,
                            propertyValues,
                            namedFields,
                            fieldValues);

                        typeBuilder.SetCustomAttribute(attributeBuilder);
                    }
                    catch (Exception)
                    {
                        // potential failures for complex or internal attribs
                    }
                }

                return typeBuilder.CreateType();
            }
        }
    }
}
