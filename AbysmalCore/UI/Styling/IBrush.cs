using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Styling
{
    /// <summary>
    /// Represents an abstraction of a brush used for drawing UI elements
    /// </summary>
    [DebugInfo("brush parent class")]
    public interface IBrush
    {
        /// used for brush validation
        public enum BrushType
        {
            /// <summary>
            /// A brush that draws an image
            /// </summary>
            ImageBrush,
            /// <summary>
            /// A brush that draws a 2-color linear gradient
            /// </summary>
            LinearGradientBrush,
            /// <summary>
            /// A brush that draws a 2-color radial gradient
            /// </summary>
            RadialGradientBrush,
            /// <summary>
            /// A brush that draws a shader
            /// </summary>
            ShaderBrush,
            /// <summary>
            /// A brush that draws a solid color
            /// </summary>
            SolidBrush,
            /// <summary>
            /// A brush that draws a 9slice texture
            /// </summary>
            NineSliceBrush,
        }

        /// <summary>
        /// The type of brush
        /// </summary>
        public abstract BrushType Type { get; }

        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="position">The position relative to the top left corner of the client area</param>
        /// <param name="size">The size</param>
        public abstract void DrawRectangle(Vector2Int position, Vector2Int size);
        /// <summary>
        /// Draws text
        /// </summary>
        /// <param name="font">The font to use</param>
        /// <param name="text">The text to draw</param>
        /// <param name="position">The position relative to the top left corner of the client area</param>
        /// <param name="fontSize">The size of the text</param>
        public abstract void DrawText(Font font, string text, Vector2Int position, int fontSize);
        /// <summary>
        /// Draws a rectangle with rounded corners
        /// </summary>
        /// <param name="position">The position relative to the top left corner of the client area</param>
        /// <param name="size">The size</param>
        /// <param name="radius">How rounded the corners are</param>
        public abstract void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius);

        /// <summary>
        /// Returns a solid color if unsupported
        /// </summary>
        public abstract Color Fallback();
    }
}
