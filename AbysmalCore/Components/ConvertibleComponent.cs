namespace AbysmalCore.Components
{
    /// <summary>
    /// Adds base methods to allow type conversion
    /// </summary>
    public abstract class ConvertibleComponent
    {
        /// <summary>
        /// Converts this instance into type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        public virtual T Convert<T>() where T : class => this as T ?? throw new InvalidCastException($"Cannot {GetType().Name} to {typeof(T).Name}");

        /// <summary>
        /// Whether this instance can be converted into type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        public virtual bool CanConvert<T>() where T : class => this as T != null;

        /// <summary>
        /// Whether this instance can be converted into type <typeparamref name="T"/>, if so, outputs to <paramref name="value"/>
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <param name="value">The output variable if convertible</param>
        /// <returns>true if convertible</returns>
        public virtual bool CanConvert<T>(out T? value) where T : class
        {
            value = this as T;
            return value != null;
        }
    }
}
