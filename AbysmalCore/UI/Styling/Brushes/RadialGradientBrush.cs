using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Styling.Brushes
{
    [DebugInfo("brush")]
    public class RadialGradientBrush : IBrush
    {
        public IBrush.BrushType Type => IBrush.BrushType.RadialGradientBrush;

        public float Blending;
        public Color Inner;
        public Color Outer;

        private Texture2D? gradient;
        private int? srcWidth;
        private int? srcHeight;

        public RadialGradientBrush(float blending, Color inner, Color outer)
        {
            Blending = blending;
            Inner = inner; Outer = outer;
        }

        public void DrawRectangle(Vector2Int position, Vector2Int size)
        {
            /// we only want to generate it once
            if (gradient == null)
            {
                Debug.Log(this, $"Generating radial gradient texture ({size.X}x{size.Y}px)");
                Image img = GenImageGradientRadial(size.X, size.Y, Blending, Inner, Outer);
                srcWidth = img.Width;
                srcHeight = img.Height;

                gradient = LoadTextureFromImage(img);
                UserInterface.UnloadList.Add(gradient);
            }

            /// we keep track of the source width and height
            /// so we can get the full image and squash it to
            /// scale with the dest size
            /// we also use ! here because we set it above
            DrawTexturePro((Texture2D)gradient!,
                new Rectangle(0, 0, (float)srcWidth!, (float)srcHeight!),
                new Rectangle(position.ToSys(), size.ToSys()),
                new(0), 0, Color.White);
        }

        public void DrawText(Font font, string text, Vector2Int position, int fontSize) => throw new NotSupportedException();
        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius) => throw new NotSupportedException();

        public Color Fallback() => Inner;
    }
}
