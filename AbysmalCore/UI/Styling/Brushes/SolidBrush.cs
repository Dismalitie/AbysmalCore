using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.UI.Styling.Brushes
{
    public class SolidBrush : IBrush
    {
        public IBrush.BrushType Type => IBrush.BrushType.SolidBrush;

        public Color Color;
        public SolidBrush(Color color) => Color = color;

        public void DrawRectangle(Vector2Int position, Vector2Int size) =>
             Raylib.DrawRectangle(position.X, position.Y, size.X, size.Y, Color);

        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius) =>
                    Raylib.DrawRectangleRounded(new(position.ToSys(), size.ToSys()), (float)radius / 10, 100, Color);

        public void DrawText(Font font, string text, Vector2Int position, int fontSize) =>
                    DrawTextEx(font, text, position.ToSys(), fontSize, 3f, Color);

        /// this is actually redundant since every style supports
        /// solid colors but we do it to satisfy the compiler as
        /// it is abstractly derived from IBrush
        public Color Fallback() => Color;
    }
}
