namespace AbysmalCore.Debugging
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    [DebugInfo("info to display in a debug message")]
    public class DebugInfoAttribute : Attribute
    {
        public readonly string Description;

        public DebugInfoAttribute(string desc) => Description = desc;
    }
}
