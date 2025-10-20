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
            ImageBrush,
            LinearGradientBrush,
            RadialGradientBrush,
            ShaderBrush,
            SolidBrush,
            NineSliceBrush,
        }

        public abstract BrushType Type { get; }

        public abstract void DrawRectangle(Vector2Int position, Vector2Int size);
        public abstract void DrawText(Font font, string text, Vector2Int position, int fontSize);
        public abstract void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius);

        public abstract Color Fallback();
    }
}
