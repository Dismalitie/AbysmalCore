using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.UI.Styling
{
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
