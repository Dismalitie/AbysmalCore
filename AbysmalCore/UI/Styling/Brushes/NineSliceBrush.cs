using AbysmalCore.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.UI.Styling.Brushes
{
    [DebugInfo("brush")]
    public class NineSliceBrush : IBrush
    {
        public IBrush.BrushType Type => IBrush.BrushType.NineSliceBrush;

        public Image Texture;
        private Texture2D tex;
        private Color fallback = Color.Black;
        public NPatchInfo NineSliceInfo;
        public NineSliceBrush(Image texture, NPatchInfo nineSlice)
        {
            Texture = texture;
            NineSliceInfo = nineSlice;

            /// unsafeness because cpp binds return pointer[]
            unsafe { fallback = LoadImageColors(texture)[0]; }

            tex = LoadTextureFromImage(Texture);
            /// tex2ds on gpu, unload
            UserInterface.UnloadList.Add(tex);
        }

        public void DrawRectangle(Vector2Int position, Vector2Int size) =>
                DrawTextureNPatch(tex, NineSliceInfo, new(position.ToSys(), size.ToSys()), new(0), 0, Color.White);

        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius) =>
                       DrawTextureNPatch(tex, NineSliceInfo, new(position.ToSys(), size.ToSys()), new(0), 0, Color.White);

        public void DrawText(Font font, string text, Vector2Int position, int fontSize) => throw new NotSupportedException();

        public Color Fallback() => fallback;
    }
}
