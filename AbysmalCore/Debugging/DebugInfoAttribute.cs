namespace AbysmalCore.Debugging
{
    /// <summary>
    /// Attribute for adding debug information to classes like names and importance
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    [DebugInfo("info to display in a debug message")]
    public class DebugInfoAttribute : Attribute
    {
        public readonly string Description;
        public readonly bool Important;

        public enum ImportanceActionType { Highlight, Pause }
        public static ImportanceActionType ImportanceAction = ImportanceActionType.Highlight;

        public DebugInfoAttribute(string desc, bool important = false)
        {
            Description = desc;
            Important = important;
        }
    }
}
