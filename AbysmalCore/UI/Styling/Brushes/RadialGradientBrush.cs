namespace AbysmalCore.UI.Styling.Brushes
{
    public class RadialGradientBrush : IBrush
    {
        public IBrush.BrushType Type => IBrush.BrushType.RadialGradientBrush;

        public float Density;
        public Color Inner;
        public Color Outer;

        private Texture2D? gradient;
        private int? srcWidth;
        private int? srcHeight;

        public RadialGradientBrush(float blending, Color inner, Color outer)
        {
            Density = blending;
            Inner = inner; Outer = outer;
        }

        public void DrawRectangle(Vector2Int position, Vector2Int size)
        {
            /// we only want to generate it once
            if (gradient == null)
            {
                Image img = GenImageGradientRadial(size.X, size.Y, Density, Inner, Outer);
                srcWidth = img.Width;
                srcHeight = img.Height;

                gradient = LoadTextureFromImage(img);
                UserInterface.TextureUnloadList.Add((Texture2D)gradient);
            }

            /// we keep track of the source width and height
            /// so we can get the full image and squash it to
            /// scale with the dest size
            DrawTexturePro((Texture2D)gradient,
                new Rectangle(0, 0, (float)srcWidth, (float)srcHeight),
                new Rectangle(position.ToSys(), size.ToSys()),
                new(0), 0, Color.White);
        }

        public void DrawText(Font font, string text, Vector2Int position, int fontSize) => throw new NotSupportedException();
        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius) => throw new NotSupportedException();

        public Color Fallback() => Inner;
    }
}
