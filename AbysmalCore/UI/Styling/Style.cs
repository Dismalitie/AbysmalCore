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
        public IBrush BorderColor = new SolidBrush(Color.DarkGray);
        public int BorderWeight = 2;
        public int BorderRadius = 5;
        public IBrush FillColor = new SolidBrush(Color.Gray);
        public IBrush TextColor = new SolidBrush(Color.White);
        public Font Font = GetFontDefault();

        /// <summary>
        /// Checks if the brushes used in this style are supported by the given control
        /// </summary>
        /// <param name="supported"></param>Array of supported brush types
        /// <param name="control"></param>Control to validate against
        public void ValidateBrushes(IBrush.BrushType[] supported, UIElement control)
        {
            if (!supported.Contains(BorderColor.Type))
                throw new NotSupportedException($"{BorderColor.Type} not supported on {control.GetType().Name} ({control.Name})");
            if (!supported.Contains(FillColor.Type))
                throw new NotSupportedException($"{FillColor.Type} not supported on {control.GetType().Name} ({control.Name})");
        }
    }
}
