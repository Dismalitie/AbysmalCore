using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Styling
{
    [DebugInfo("control specific state styles")]
    public struct StyleMap
    {
        public enum ControlStyleType
        {
            Rounded,
            Sharp,
        }

        public ControlStyleType ControlStyle = ControlStyleType.Sharp;
        public Style Hovered;
        public Style Clicked;
        public Style Normal;
        public Style Disabled;
        public Style HoveredDisabled;
        public Style Activated;
        public Style HoveredActivated;

        public StyleMap(bool useGlobalTheme = false)
        {
            if (useGlobalTheme)
            {
                Hovered ??= Window.GlobalTheme.DefaultStyleMap.Hovered;
                Clicked ??= Window.GlobalTheme.DefaultStyleMap.Clicked;
                Normal ??= Window.GlobalTheme.DefaultStyleMap.Normal;
                Disabled ??= Window.GlobalTheme.DefaultStyleMap.Disabled;
                ControlStyle = Window.GlobalTheme.DefaultStyleMap.ControlStyle;
                Activated ??= Window.GlobalTheme.DefaultStyleMap.Activated;
                HoveredDisabled ??= Window.GlobalTheme.DefaultStyleMap.HoveredDisabled;
                HoveredActivated ??= Window.GlobalTheme.DefaultStyleMap.HoveredActivated;
            }
            else
            {
                Hovered ??= new();
                Clicked ??= new();
                Normal ??= new();
                Disabled ??= new();
                ControlStyle = ControlStyleType.Sharp;
                Activated ??= new();
                HoveredDisabled ??= new();
                HoveredActivated ??= new();
            }
        }

        public void ValidateBrushes(Dictionary<ControlStyleType, IBrush.BrushType[]> supported, UIElement control)
        {
            Hovered?.ValidateBrushes(supported[ControlStyle], control);
            Clicked?.ValidateBrushes(supported[ControlStyle], control);
            Normal?.ValidateBrushes(supported[ControlStyle], control);
            Disabled?.ValidateBrushes(supported[ControlStyle], control);
        }
    }
}
