namespace AbysmalCore.Components
{
    /// <summary>
    /// Adds base methods to allow type conversion
    /// </summary>
    public interface ConvertibleComponent
    {
        /// <summary>
        /// Converts this instance into type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <returns></returns>
        public object Convert<T>();

        /// <summary>
        /// Whether this instance can be converted into type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        public bool CanConvert<T>();

        /// <summary>
        /// Whether this instance can be converted into type <typeparamref name="T"/>, if so, outputs to <paramref name="value"/>
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <param name="value">The output variable if convertible</param>
        /// <returns>true if convertible</returns>
        public bool CanConvert<T>(out object? value);
    }
}
