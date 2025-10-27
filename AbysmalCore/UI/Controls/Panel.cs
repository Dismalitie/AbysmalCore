using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;
using System.ComponentModel;

namespace AbysmalCore.UI.Controls
{
    /// <summary>
    /// A container panel control
    /// </summary>
    [DebugInfo("panel control")]
    public class Panel : UIElement, INotifyPropertyChanged
    {
        /// <summary>
        /// Creates a panel
        /// </summary>
        /// <param name="position">The position relative to the top left corner of the client area</param>
        /// <param name="size">The size</param>
        /// <param name="children">Any child controls to draw</param>
        public Panel(Vector2Int position, Vector2Int size, List<UIElement>? children = null)
        {
            Position = position;
            Size = size;

            // cant normally assign because the children
            // are readonly, so we manually add them
            if (children != null)
                foreach (UIElement child in children)
                    AddChild(child);
        }

        /// <inheritdoc/>
        protected override void _draw()
        {
            // mostly just copied from <see cref="Button"/>
            if (StyleMap.ControlStyle == StyleMap.ControlStyleType.Rounded)
            {
                try { StyleMap.Normal.BorderColor.DrawRectangleRounded(Position, Size, StyleMap.Normal.BorderRadius); }
                catch { DrawRectangleRounded(new(Position.X, Position.Y, Size.X, Size.Y), (float)StyleMap.Normal.BorderRadius / 10, 1, StyleMap.Normal.BorderColor.Fallback()); }

                Vector2Int nonBorderPos = new(Position.X + StyleMap.Normal.BorderWeight, Position.Y + StyleMap.Normal.BorderWeight);
                Vector2Int nonBorderSz = new(Size.X - StyleMap.Normal.BorderWeight * 2, Size.Y - StyleMap.Normal.BorderWeight * 2);
                try { StyleMap.Normal.FillColor.DrawRectangleRounded(nonBorderPos, nonBorderSz, StyleMap.Normal.BorderRadius); }
                catch { DrawRectangleRounded(new(nonBorderPos.X, nonBorderPos.Y, nonBorderSz.X, nonBorderSz.Y), StyleMap.Normal.BorderRadius / 10, 1, StyleMap.Normal.FillColor.Fallback()); }
            }
            else if (StyleMap.ControlStyle == StyleMap.ControlStyleType.Sharp)
            {
                try { StyleMap.Normal.BorderColor.DrawRectangle(Position, Size); }
                catch { DrawRectangle(Position.X, Position.Y, Size.X, Size.Y, StyleMap.Normal.BorderColor.Fallback()); }

                Vector2Int nonBorderPos = new(Position.X + StyleMap.Normal.BorderWeight, Position.Y + StyleMap.Normal.BorderWeight);
                Vector2Int nonBorderSz = new(Size.X - StyleMap.Normal.BorderWeight * 2, Size.Y - StyleMap.Normal.BorderWeight * 2);
                try { StyleMap.Normal.FillColor.DrawRectangle(nonBorderPos, nonBorderSz); }
                catch { DrawRectangle(nonBorderPos.X, nonBorderPos.Y, nonBorderSz.X, nonBorderSz.Y, StyleMap.Normal.FillColor.Fallback()); }
            }
        }
    }
}
