using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.UI.Styling.Brushes
{
    public class LinearGradientBrush : IBrush
    {
        public IBrush.BrushType Type => IBrush.BrushType.LinearGradientBrush;

        public enum GradientDirection
        {
            Vertical, Horizontal,
            DiagonalRight, DiagonalLeft,
        }

        public GradientDirection Direction;
        public Color Color1;
        public Color Color2;
        public LinearGradientBrush(GradientDirection direction, Color c1, Color c2)
        {
            Direction = direction;
            Color1 = c1; Color2 = c2;
        }

        public void DrawRectangle(Vector2Int position, Vector2Int size)
        {
            /// holy code soup
            if (Direction == GradientDirection.Vertical)
                DrawRectangleGradientV(position.X, position.Y, size.X, size.Y, Color1, Color2);
            else if (Direction == GradientDirection.Horizontal)
                DrawRectangleGradientH(position.X, position.Y, size.X, size.Y, Color1, Color2);
            else if (Direction == GradientDirection.DiagonalRight)
                DrawRectangleGradientEx(new Rectangle(position.X, position.Y, size.X, size.Y),
                    ColorLerp(Color1, Color2, 0.5f), 
                    Color1, Color2, 
                    ColorLerp(Color1, Color2, 0.5f));
            else if (Direction == GradientDirection.DiagonalLeft)
                DrawRectangleGradientEx(new Rectangle(position.X, position.Y, size.X, size.Y),
                    ColorLerp(Color1, Color2, 0.5f),
                    Color2, Color1,
                    ColorLerp(Color1, Color2, 0.5f));
        }

        public void DrawText(Font font, string text, Vector2Int position, int fontSize) => throw new NotSupportedException();
        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius) => throw new NotSupportedException();

        public Color Fallback() => Color1;
    }
}
