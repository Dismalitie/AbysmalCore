using AbysmalCore.UI;
using AbysmalCore.UI.Styling;
using AbysmalCore.UI.Styling.Brushes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.UI.Controls
{
    public class Button : UIElement
    {
        public string Text;
        public int FontSize;

        public Button(string text, Vector2Int position, Vector2Int? size = null, int fontSize = 20)
        {
            Text = text;
            Position = position;
            FontSize = fontSize;

            if (size != null) Size = (Vector2Int)size;
            else Size = new(150, 50);
        }

        public override void _draw()
        {
            /// the actual button bit excluding the border
            Vector2Int nonBorderPos = new(Position.X + CurrentStyle.BorderWeight, Position.Y + CurrentStyle.BorderWeight);
            Vector2Int nonBorderSz = new(Size.X - CurrentStyle.BorderWeight * 2, Size.Y - CurrentStyle.BorderWeight * 2);

            /// messy
            if (StyleMap.ControlStyle == StyleMap.ControlStyleType.Rounded)
            {
                try { CurrentStyle.BorderColor.DrawRectangleRounded(Position, Size, CurrentStyle.BorderRadius); }
                catch { DrawRectangleRounded(new(Position.X, Position.Y, Size.X, Size.Y), 
                    (float)CurrentStyle.BorderRadius / 10, 1, 
                    CurrentStyle.BorderColor.Fallback()); }

                try { CurrentStyle.FillColor.DrawRectangleRounded(nonBorderPos, nonBorderSz, CurrentStyle.BorderRadius); }
                catch { DrawRectangleRounded(new(nonBorderPos.X, nonBorderPos.Y, nonBorderSz.X, nonBorderSz.Y), 
                    CurrentStyle.BorderRadius / 10, 1, 
                    CurrentStyle.FillColor.Fallback()); }
            }
            else if (StyleMap.ControlStyle == StyleMap.ControlStyleType.Sharp)
            {
                try { CurrentStyle.BorderColor.DrawRectangle(Position, Size); }
                catch { DrawRectangle(Position.X, Position.Y, Size.X, Size.Y, CurrentStyle.BorderColor.Fallback()); }
                
                try { CurrentStyle.FillColor.DrawRectangle(nonBorderPos, nonBorderSz); }
                catch { DrawRectangle(nonBorderPos.X, nonBorderPos.Y, nonBorderSz.X, nonBorderSz.Y, CurrentStyle.FillColor.Fallback()); }
            }

            /// / 2 because we need roughly the middle
            int len = MeasureText(Text, FontSize) / 2;

            DrawText(Text,
                Position.X + Size.X / 2 - len, Position.Y + Size.Y / 2 - FontSize / 2,
                FontSize, CurrentStyle.TextColor.Fallback());
        }
    }
}
