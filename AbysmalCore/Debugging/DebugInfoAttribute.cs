namespace AbysmalCore.Debugging
{
    /// <summary>
    /// Attribute for adding debug information to classes like names and importance
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    [DebugInfo("info to display in a debug message")]
    public class DebugInfoAttribute : Attribute
    {
        /// <summary>
        /// Breif description of the class
        /// </summary>
        public readonly string Description;
        /// <summary>
        /// A short more readable name
        /// </summary>
        public readonly bool Important;

        /// <summary>
        /// Determines what to do upon logging an important message
        /// </summary>
        public enum ImportanceActionType 
        { 
            /// <summary>
            /// Highlights the message in magenta
            /// </summary>
            Highlight, 
            /// <summary>
            /// Pauses application execution until console input is provided
            /// </summary>
            Pause 
        }
        /// <summary>
        /// Determines what to do upon an important message being logged
        /// </summary>
        public static ImportanceActionType ImportanceAction = ImportanceActionType.Highlight;

        /// <summary>
        /// Adds debug info to a class
        /// </summary>
        /// <param name="desc"></param>A description
        /// <param name="important"></param>Whether messages originating from this class are important
        public DebugInfoAttribute(string desc, bool important = false)
        {
            Description = desc;
            Important = important;
        }
    }
}
