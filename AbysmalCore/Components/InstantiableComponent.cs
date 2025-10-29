namespace AbysmalCore.Components
{
    /// <summary>
    /// Generates a short hand for <c>new Class()</c>
    /// </summary>
    /// <remarks>
    /// Mostly used in classes with no constructor where <see cref="Debugging.AbysmalDebug.Log(object, string, bool)"/>
    /// is used as it takes an instance
    /// </remarks>
    /// <typeparam name="T">Instance type of deriving class</typeparam>
    public class InstantiableComponent<T> where T : class, new()
    {
        /// <summary>
        /// Returns a new instance of this class
        /// </summary>
        public static T _this => new T();
    }
}
