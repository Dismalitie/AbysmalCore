using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Styling.Brushes
{
    /// <summary>
    /// Draws a solid color
    /// </summary>
    [DebugInfo("brush")]
    public class SolidBrush : IBrush
    {
        /// <inheritdoc/>
        public IBrush.BrushType Type => IBrush.BrushType.SolidBrush;

        /// <summary>
        /// Color to draw
        /// </summary>
        public Color Color;
        /// <summary>
        /// Creates a new solid color brush
        /// </summary>
        /// <param name="color">The color of the brush</param>
        public SolidBrush(Color color) => Color = color;

        /// <inheritdoc/>
        public void DrawRectangle(Vector2Int position, Vector2Int size) =>
             Raylib.DrawRectangle(position.X, position.Y, size.X, size.Y, Color);

        /// <inheritdoc/>
        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius) =>
                    Raylib.DrawRectangleRounded(new(position.ToSys(), size.ToSys()), (float)radius / 10, 100, Color);

        /// <inheritdoc/>
        public void DrawText(Font font, string text, Vector2Int position, int fontSize) =>
                    DrawTextEx(font, text, position.ToSys(), fontSize, 3f, Color);

        // this is actually redundant since every style supports
        // solid colors but we do it to satisfy the compiler as
        // it is abstractly derived from IBrush
        /// <inheritdoc/>
        public Color Fallback() => Color;
    }
}
