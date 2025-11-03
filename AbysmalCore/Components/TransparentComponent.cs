using System.Reflection;

namespace AbysmalCore.Components
{
    /// <summary>
    /// Adds concise reflection methods to this class
    /// </summary>
    public abstract class TransparentComponent
    {
        /// <summary>
        /// Returns a dictionary of all public, instance property names 
        /// mapped to their respective PropertyInfo objects
        /// </summary>
        public Dictionary<string, PropertyInfo> PeekPropertyInfo()
        {
            Type derivedType = GetType();
            PropertyInfo[] properties = derivedType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Dictionary<string, PropertyInfo> result = new();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.GetIndexParameters().Length == 0)
                    result.Add(prop.Name, prop);
            }
            return result;
        }

        /// <summary>
        /// Returns a dictionary of all public, instance property names 
        /// mapped to their current values
        /// </summary>
        public Dictionary<string, object?> PeekPropertyValues()
        {
            Type derivedType = GetType();
            PropertyInfo[] properties = derivedType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Dictionary<string, object?> result = new();
            foreach (PropertyInfo prop in properties)
            {
                // filter out indexers (int[i])
                if (prop.CanRead && prop.GetIndexParameters().Length == 0)
                {
                    object? value = prop.GetValue(this);
                    result.Add(prop.Name, value);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a dictionary of all public, instance method names 
        /// mapped to their respective MethodInfo objects
        /// </summary>
        public Dictionary<string, MethodInfo> PeekMethods()
        {
            Type derivedType = GetType();
            MethodInfo[] methods = derivedType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Dictionary<string, MethodInfo> result = new();
            foreach (MethodInfo meth in methods)
            {
                // filter out get and sets
                if (!meth.IsSpecialName)
                {
                    result.Add(meth.Name, meth);
                }
            }
            return result;
        }
    }
}