using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Styling.Brushes
{
    /// <summary>
    /// Draws a 9slice texture
    /// </summary>
    [DebugInfo("brush")]
    public class NineSliceBrush : IBrush
    {
        /// <inheritdoc/>
        public IBrush.BrushType Type => IBrush.BrushType.NineSliceBrush;

        /// <summary>
        /// The texture to draw
        /// </summary>
        public Image Texture;
        private Texture2D tex;
        private Color fallback = Color.Black;
        /// <summary>
        /// Information used in drawing
        /// </summary>
        public NPatchInfo NineSliceInfo;

        /// <summary>
        /// Creates a new nine slice brush
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="nineSlice"></param>
        public NineSliceBrush(Image texture, NPatchInfo nineSlice)
        {
            Texture = texture;
            NineSliceInfo = nineSlice;

            // unsafeness because cpp binds return pointer[]
            unsafe { fallback = LoadImageColors(texture)[0]; }

            tex = LoadTextureFromImage(Texture);
            // tex2ds on gpu, unload
            UserInterface.UnloadList.Add(tex);
        }

        /// <inheritdoc/>
        public void DrawRectangle(Vector2Int position, Vector2Int size) =>
                DrawTextureNPatch(tex, NineSliceInfo, new(position.ToSys(), size.ToSys()), new(0), 0, Color.White);

        /// <inheritdoc/>
        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius) =>
                       DrawTextureNPatch(tex, NineSliceInfo, new(position.ToSys(), size.ToSys()), new(0), 0, Color.White);

        /// <inheritdoc/>
        public void DrawText(Font font, string text, Vector2Int position, int fontSize) => throw new NotSupportedException();

        /// <inheritdoc/>
        public Color Fallback() => fallback;
    }
}
