using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Styling.Brushes
{
    /// <summary>
    /// Draws a 2-color linear gradient
    /// </summary>
    [DebugInfo("brush")]
    public class LinearGradientBrush : IBrush
    {
        /// <inheritdoc/>
        public IBrush.BrushType Type => IBrush.BrushType.LinearGradientBrush;

        /// <summary>
        /// The direction of the gradient
        /// </summary>
        public enum GradientDirection
        {
            /// <summary>
            /// Draws color 1 at the bottom, color 2 at the top
            /// </summary>
            Vertical,
            /// <summary>
            /// Draws color 1 on the left, color 2 on the right
            /// </summary>
            Horizontal,
            /// <summary>
            /// Draws color 1 in the bottom left, color 2 in the top right
            /// </summary>
            DiagonalRight, 
            /// <summary>
            /// Draws color 1 in the top left, color 2 in the bottom right
            /// </summary>
            DiagonalLeft,
        }

        /// <summary>
        /// The direction to draw the gradient
        /// </summary>
        public GradientDirection Direction;
        /// <summary>
        /// The first color
        /// </summary>
        public Color Color1;
        /// <summary>
        /// The second color
        /// </summary>
        public Color Color2;

        /// <summary>
        /// Creates a new linear gradient brush
        /// </summary>
        /// <param name="direction">The direction to draw the colors</param>
        /// <param name="c1">The first color</param>
        /// <param name="c2">The second color</param>
        public LinearGradientBrush(GradientDirection direction, Color c1, Color c2)
        {
            Direction = direction;
            Color1 = c1; Color2 = c2;
        }

        /// <inheritdoc/>
        public void DrawRectangle(Vector2Int position, Vector2Int size)
        {
            // holy code soup
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
        /// <inheritdoc/>
        public void DrawText(Font font, string text, Vector2Int position, int fontSize) => throw new NotSupportedException();
        /// <inheritdoc/>
        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius) => throw new NotSupportedException();
        /// <inheritdoc/>
        public Color Fallback() => Color1;
    }
}
