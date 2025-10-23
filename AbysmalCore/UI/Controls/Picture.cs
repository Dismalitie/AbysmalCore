using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;

namespace AbysmalCore.UI.Controls
{
    /// <summary>
    /// A control for displaying a picture/image
    /// </summary>
    [DebugInfo("picture control")]
    public class Picture : UIElement
    {
        /// <summary>
        /// The image to draw
        /// </summary>
        public Image Image;
        private Texture2D _tex;
        /// <summary>
        /// Whether to tint the image with a color
        /// </summary>
        public bool Tint;

        /// <summary>
        /// Creates a new Picture control
        /// </summary>
        /// <param name="image"></param>The image to draw
        /// <param name="tint"></param>Whether to tint the image
        public Picture(Image image, bool tint = false)
        {
            Image = image;
            _tex = LoadTextureFromImage(Image);
            Tint = tint;
            UserInterface.UnloadList.Add(_tex);

            SupportedBrushes = new()
            {
                {StyleMap.ControlStyleType.Sharp,
                [
                    IBrush.BrushType.ShaderBrush,
                    IBrush.BrushType.SolidBrush,
                ]},
            };
        }

        /// <inheritdoc/>
        protected override void _draw()
        {
            DrawTexturePro(_tex, 
                new(Position.ToSys(), Size.ToSys()), 
                new(0, 0, _tex.Width, _tex.Height), 
                new(), 0, Color.White);
        }
    }
}
