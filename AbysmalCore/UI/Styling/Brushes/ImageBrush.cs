using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.UI.Styling.Brushes
{
    internal class ImageBrush : IBrush
    {
        public IBrush.BrushType Type => IBrush.BrushType.ImageBrush;

        public Image Image;
        private Texture2D tex;
        private Color fallback;
        public ImageBrush(Image img, Color? color = null)
        {
            Image = img;
            /// UNSAFE?! (cpp bindings return pointer[])
            /// just grab the first color because i was too lazy to average
            /// colors with a func
            if (color == null) unsafe { fallback = LoadImageColors(img)[0]; }
            else fallback = (Color)color;

            tex = LoadTextureFromImage(Image);
            /// texture2ds are stored on the gpu, so we
            /// have to unload them upon exiting
            UserInterface.UnloadList.Add(tex);
        }

        public void DrawRectangle(Vector2Int position, Vector2Int size) =>
                    DrawTexturePro(tex,
                        new(0, 0, tex.Width, tex.Height),
                        new(position.ToSys(),
                        size.ToSys()),
                        new(0), 0, Color.White);

        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius) => throw new NotSupportedException();

        public void DrawText(Font font, string text, Vector2Int position, int fontSize) => throw new NotSupportedException();

        public Color Fallback() => fallback;
    }
}
