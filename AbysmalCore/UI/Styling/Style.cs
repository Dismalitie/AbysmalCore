using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling.Brushes;

namespace AbysmalCore.UI.Styling
{
    /// <summary>
    /// Contains style information for a specific control
    /// </summary>
    [DebugInfo("control specific style")]
    public class Style
    {
        /// <summary>
        /// The color of the border on a control
        /// </summary>
        public IBrush BorderColor = new SolidBrush(Color.DarkGray);
        /// <summary>
        /// How thick the border is
        /// </summary>
        public int BorderWeight = 2;
        /// <summary>
        /// How rounded the border is
        /// </summary>
        public int BorderRadius = 5;
        /// <summary>
        /// Brush to use on the interior of the control
        /// </summary>
        public IBrush FillColor = new SolidBrush(Color.Gray);
        /// <summary>
        /// Brush to use on the border of the brush
        /// </summary>
        public IBrush TextColor = new SolidBrush(Color.White);
        /// <summary>
        /// Font to use when drawing text
        /// </summary>
        public Font Font = GetFontDefault();

        /// <summary>
        /// Checks if the brushes used in this style are supported by the given control
        /// </summary>
        /// <param name="supported">Array of supported brush types</param>
        /// <param name="control">Control to validate against</param>
        public void ValidateBrushes(IBrush.BrushType[] supported, UIElement control)
        {
            if (!supported.Contains(BorderColor.Type))
                throw new NotSupportedException($"{BorderColor.Type} not supported on {control.GetType().Name} ({control.Name})");
            if (!supported.Contains(FillColor.Type))
                throw new NotSupportedException($"{FillColor.Type} not supported on {control.GetType().Name} ({control.Name})");
        }
    }
}
