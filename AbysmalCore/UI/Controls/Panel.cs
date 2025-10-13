using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;

namespace AbysmalCore.UI.Controls
{
    [DebugInfo("panel control")]
    public class Panel : UIElement
    {
        public Panel(Vector2Int position, Vector2Int size, List<UIElement>? children = null)
        {
            Position = position;
            Size = size;

            /// cant normally assign because the children
            /// are readonly, so we manually add them
            if (children != null)
                foreach (UIElement child in children)
                    AddChild(child);
        }

        protected override void _draw()
        {
            /// mostly just copied from <see cref="Button"/>
            if (StyleMap.ControlStyle == StyleMap.ControlStyleType.Rounded)
            {
                try { CurrentStyle.BorderColor.DrawRectangleRounded(Position, Size, CurrentStyle.BorderRadius); }
                catch { DrawRectangleRounded(new(Position.X, Position.Y, Size.X, Size.Y), (float)CurrentStyle.BorderRadius / 10, 1, CurrentStyle.BorderColor.Fallback()); }

                Vector2Int nonBorderPos = new(Position.X + CurrentStyle.BorderWeight, Position.Y + CurrentStyle.BorderWeight);
                Vector2Int nonBorderSz = new(Size.X - CurrentStyle.BorderWeight * 2, Size.Y - CurrentStyle.BorderWeight * 2);
                try { CurrentStyle.FillColor.DrawRectangleRounded(nonBorderPos, nonBorderSz, CurrentStyle.BorderRadius); }
                catch { DrawRectangleRounded(new(nonBorderPos.X, nonBorderPos.Y, nonBorderSz.X, nonBorderSz.Y), CurrentStyle.BorderRadius / 10, 1, CurrentStyle.FillColor.Fallback()); }
            }
            else if (StyleMap.ControlStyle == StyleMap.ControlStyleType.Sharp)
            {
                try { CurrentStyle.BorderColor.DrawRectangle(Position, Size); }
                catch { DrawRectangle(Position.X, Position.Y, Size.X, Size.Y, CurrentStyle.BorderColor.Fallback()); }

                Vector2Int nonBorderPos = new(Position.X + CurrentStyle.BorderWeight, Position.Y + CurrentStyle.BorderWeight);
                Vector2Int nonBorderSz = new(Size.X - CurrentStyle.BorderWeight * 2, Size.Y - CurrentStyle.BorderWeight * 2);
                try { CurrentStyle.FillColor.DrawRectangle(nonBorderPos, nonBorderSz); }
                catch { DrawRectangle(nonBorderPos.X, nonBorderPos.Y, nonBorderSz.X, nonBorderSz.Y, CurrentStyle.FillColor.Fallback()); }
            }
        }
    }
}
