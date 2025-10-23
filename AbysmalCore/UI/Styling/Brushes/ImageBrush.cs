using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Styling.Brushes
{
    /// <summary>
    /// Draws an image
    /// </summary>
    [DebugInfo("brush")]
    public class ImageBrush : IBrush
    {
        /// <inheritdoc/>
        public IBrush.BrushType Type => IBrush.BrushType.ImageBrush;

        /// <summary>
        /// Image to draw
        /// </summary>
        public Image Image;
        private Texture2D tex;
        private Color fallback;

        /// <summary>
        /// Creates a new image brush
        /// </summary>
        /// <param name="img"></param>The image to draw
        /// <param name="color"></param>The fallback color
        public ImageBrush(Image img, Color? color = null)
        {
            Image = img;
            // UNSAFE?! (cpp bindings return pointer[])
            // just grab the first color because i was too lazy to average
            // colors with a func
            if (color == null) unsafe { fallback = LoadImageColors(img)[0]; }
            else fallback = (Color)color;

            tex = LoadTextureFromImage(Image);
            // texture2ds are stored on the gpu, so we
            // have to unload them upon exiting
            UserInterface.UnloadList.Add(tex);
        }

        /// <inheritdoc/>
        public void DrawRectangle(Vector2Int position, Vector2Int size) =>
                    DrawTexturePro(tex,
                        new(0, 0, tex.Width, tex.Height),
                        new(position.ToSys(),
                        size.ToSys()),
                        new(0), 0, Color.White);

        /// <inheritdoc/>
        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius) => throw new NotSupportedException();

        /// <inheritdoc/>
        public void DrawText(Font font, string text, Vector2Int position, int fontSize) => throw new NotSupportedException();

        /// <inheritdoc/>
        public Color Fallback() => fallback;
    }
}
