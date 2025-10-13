using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;

namespace AbysmalCore.UI.Controls
{
    [DebugInfo("picture control")]
    public class Picture : UIElement
    {
        public Image Image;
        private Texture2D _tex;
        public bool Tint;
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

        protected override void _draw()
        {
            DrawTexturePro(_tex, 
                new(Position.ToSys(), Size.ToSys()), 
                new(0, 0, _tex.Width, _tex.Height), 
                new(), 0, Color.White);
        }
    }
}
