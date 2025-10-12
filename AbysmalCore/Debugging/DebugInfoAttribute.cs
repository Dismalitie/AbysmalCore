namespace AbysmalCore.Debugging
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DebugInfoAttribute : Attribute
    {
        public readonly string Name;
        public readonly string Description;

        public DebugInfoAttribute(string name, string desc = "")
        {
            Name = name;
            Description = desc;
        }
    }
}
