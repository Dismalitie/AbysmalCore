using AbysmalCore.UI.Styling.Brushes;

namespace AbysmalCore.UI.Styling
{
    public class Style
    {
        public IBrush BorderColor = new SolidBrush(Color.DarkGray);
        public int BorderWeight = 2;
        public int BorderRadius = 5;
        public IBrush FillColor = new SolidBrush(Color.Gray);
        public IBrush TextColor = new SolidBrush(Color.White);
        public Font Font = GetFontDefault();

        public void ValidateBrushes(IBrush.BrushType[] supported, UIElement control)
        {
            if (!supported.Contains(BorderColor.Type))
                throw new NotSupportedException($"{BorderColor.Type} not supported on {control.GetType().Name} ({control.Name})");
            if (!supported.Contains(FillColor.Type))
                throw new NotSupportedException($"{FillColor.Type} not supported on {control.GetType().Name} ({control.Name})");
        }
    }
}
