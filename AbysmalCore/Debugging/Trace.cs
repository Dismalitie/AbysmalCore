namespace AbysmalCore.Debugging
{
    internal struct Trace
    {
        public object Object { get; }
        public object Origin { get; }
        public string Description { get; }

        public Trace(object @this, object obj, string desc = "No description")
        {
            Object = @this;
            Origin = obj;
            Description = desc;
        }
    }
}
