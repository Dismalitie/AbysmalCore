using AbysmalCore.UI.Styling.Brushes;
using Raylib_cs;

namespace AbysmalCore.UI.Styling
{
    public class Style
    {
        public IBrush BorderColor = new SolidBrush(Color.DarkGray);
        public int BorderWeight = 5;
        public int BorderRadius = 2;
        public IBrush FillColor = new SolidBrush(Color.Gray);
        public SolidBrush TextColor = new(Color.White);

        public void ValidateBrushes(IBrush.BrushType[] supported, UIElement control)
        {
            if (!supported.Contains(BorderColor.Type))
                throw new NotSupportedException($"{BorderColor.Type} not supported on {control.GetType().Name} ({control.Name})");
            if (!supported.Contains(FillColor.Type))
                throw new NotSupportedException($"{FillColor.Type} not supported on {control.GetType().Name} ({control.Name})");
        }
    }
}
