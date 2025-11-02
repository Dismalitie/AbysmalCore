using AbysmalCore.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.Extensibility
{
    /// <summary>
    /// Defines a method in the Abysmal Extensibility Framework
    /// </summary>
    [DebugInfo("abysmal extensibility framework method", false)]
    public class UniformMethod
    {
        private MethodInfo _info;
        private object _instance;

        /// <summary>
        /// The name of the method
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// What type the method returns
        /// </summary>
        public Type ReturnType { get; }
        /// <summary>
        /// The number of parameters the method takes
        /// </summary>
        public int ParameterCount { get; }

        /// <summary>
        /// Creates a new AbysmalExtensibilityMethod wrapping the specified MethodInfo
        /// </summary>
        /// <param name="mi">The MethodInfo to wrap</param>
        /// <param name="instance">The instance of the underlying class</param>
        public UniformMethod(MethodInfo mi, object instance)
        {
            _info = mi;
            _instance = instance;

            Name = mi.Name;
            ReturnType = mi.ReturnType;
            ParameterCount = mi.GetParameters().Length;
        }

        /// <summary>
        /// Returns the type of the parameter at the specified index
        /// </summary>
        /// <param name="index">The index of the parameter</param>
        public Type GetParameterType(int index) => _info.GetParameters()[index].ParameterType;

        /// <summary>
        /// Executes the method with the specified arguments
        /// </summary>
        /// <param name="errHandler">An output for the exception if one occurs</param>
        /// <param name="args">The arguments to pass to the method</param>
        public object? Invoke(out Exception? errHandler, params object[] args)
        {
            AbysmalDebug.Log(this, $"Invoking method {Name} with {args.Length} arguments");
            try
            {
                errHandler = null;
                return _info.Invoke(_instance, args);
            }
            catch (Exception ex)
            {
                errHandler = ex;
                return null;
            }
        }
        /// <summary>
        /// Invokes the method with the specified arguments and converts the result to the specified type
        /// </summary>
        /// <typeparam name="T">The type to convert the result to</typeparam>
        /// <param name="args">Arguments to pass to the method</param>
        /// <param name="errHandler">An output for the exception if one occurs</param>
        public T? Invoke<T>(out Exception? errHandler, params object[] args)
        {
            AbysmalDebug.Log(this, $"Invoking method {Name} with {args.Length} arguments and converting to {typeof(T).FullName}");
            try
            {
                errHandler = null;
                return (T)_info.Invoke(_instance, args)!;
            }
            catch (Exception ex)
            {
                errHandler = ex;
                return default;
            }
        }
        /// <summary>
        /// Invokes the method with the specified arguments and converts the result using the provided converter function
        /// </summary>
        /// <typeparam name="T">The type to convert the result to</typeparam>
        /// <param name="converter">Lambda function to convert the result</param>
        /// <param name="args">Arguments to pass to the method</param>
        /// <param name="errHandler">An output for the exception if one occurs</param>
        public T? Invoke<T>(out Exception? errHandler, Func<object?, T> converter, params object[] args)
        {
            AbysmalDebug.Log(this, $"Invoking method {Name} with {args.Length} arguments and converting to {typeof(T).FullName} using custom converter");
            try
            {
                errHandler = null;
                return converter(_info.Invoke(_instance, args));
            }
            catch (Exception ex)
            {
                errHandler = ex;
                return default;
            }
        }
    }
}
