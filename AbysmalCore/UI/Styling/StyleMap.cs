using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Styling
{
    /// <summary>
    /// Contains style information for a specific control states
    /// </summary>
    [DebugInfo("control specific state styles")]
    public struct StyleMap
    {
        /// <summary>
        /// The shape style of the control
        /// </summary>
        public enum ControlStyleType
        {
            /// <summary>
            /// A control with rounded corners
            /// </summary>
            Rounded,
            /// <summary>
            /// A control with square corners
            /// </summary>
            Sharp,
        }

        /// <summary>
        /// The style of the control
        /// </summary>
        public ControlStyleType ControlStyle = ControlStyleType.Sharp;
        /// <summary>
        /// Style to use when the mouse is in the controls bounds
        /// </summary>
        public Style Hovered;
        /// <summary>
        /// Style to use when the mouse is in the controls bounds and the left mouse button is down
        /// </summary>
        public Style Clicked;
        /// <summary>
        /// Style to use when no other styles are applicable
        /// </summary>
        public Style Normal;
        /// <summary>
        /// Style to use when the control cannot be interacted with
        /// </summary>
        public Style Disabled;
        /// <summary>
        /// Style to use when the control cannot be interacted with and the mouse is in the controls bounds
        /// </summary>
        public Style HoveredDisabled;
        /// <summary>
        /// Style to used when focused
        /// </summary>
        public Style Activated;
        /// <summary>
        /// Style to use when focused and mouse is in the controls bounds
        /// </summary>
        public Style HoveredActivated;

        /// <summary>
        /// Creates a new style map
        /// </summary>
        /// <param name="useGlobalTheme"></param>Whether to use the <see cref="Window"/>'s theme
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

        /// <summary>
        /// Checks compatible brushes with the current ones
        /// </summary>
        /// <param name="supported"></param>List of supported brush types
        /// <param name="control"></param>Control to validate against
        public void ValidateBrushes(Dictionary<ControlStyleType, IBrush.BrushType[]> supported, UIElement control)
        {
            Hovered?.ValidateBrushes(supported[ControlStyle], control);
            Clicked?.ValidateBrushes(supported[ControlStyle], control);
            Normal?.ValidateBrushes(supported[ControlStyle], control);
            Disabled?.ValidateBrushes(supported[ControlStyle], control);
        }
    }
}
