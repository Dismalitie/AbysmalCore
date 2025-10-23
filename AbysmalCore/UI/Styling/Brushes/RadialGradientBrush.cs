using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Styling.Brushes
{
    /// <summary>
    /// Draws a radial gradient
    /// </summary>
    [DebugInfo("brush")]
    public class RadialGradientBrush : IBrush
    {
        /// <inheritdoc/>
        public IBrush.BrushType Type => IBrush.BrushType.RadialGradientBrush;

        /// <summary>
        /// How much to blend the 2 colors
        /// </summary>
        public float Blending;
        /// <summary>
        /// The inner color
        /// </summary>
        public Color Inner;
        /// <summary>
        /// The outer color
        /// </summary>
        public Color Outer;

        private Texture2D? gradient;
        private int? srcWidth;
        private int? srcHeight;

        /// <summary>
        /// Creates a new radial gradient brush
        /// </summary>
        /// <param name="blending"></param>How much to blend the colors
        /// <param name="inner"></param>The inner color
        /// <param name="outer"></param>The outer color
        public RadialGradientBrush(float blending, Color inner, Color outer)
        {
            Blending = blending;
            Inner = inner; Outer = outer;
        }

        /// <inheritdoc/>
        public void DrawRectangle(Vector2Int position, Vector2Int size)
        {
            // we only want to generate it once
            if (gradient == null)
            {
                AbysmalDebug.Log(this, $"Generating radial gradient texture ({size.X}x{size.Y}px)");
                Image img = GenImageGradientRadial(size.X, size.Y, Blending, Inner, Outer);
                srcWidth = img.Width;
                srcHeight = img.Height;

                gradient = LoadTextureFromImage(img);
                UserInterface.UnloadList.Add(gradient);
            }

            // we keep track of the source width and height
            // so we can get the full image and squash it to
            // scale with the dest size
            // we also use ! here because we set it above
            DrawTexturePro((Texture2D)gradient!,
                new Rectangle(0, 0, (float)srcWidth!, (float)srcHeight!),
                new Rectangle(position.ToSys(), size.ToSys()),
                new(0), 0, Color.White);
        }

        /// <inheritdoc/>
        public void DrawText(Font font, string text, Vector2Int position, int fontSize) => throw new NotSupportedException();
        /// <inheritdoc/>
        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius) => throw new NotSupportedException();

        /// <inheritdoc/>
        public Color Fallback() => Inner;
    }
}
