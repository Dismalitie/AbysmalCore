using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Controls
{
    [DebugInfo("AbysmalCore label control")]
    public class Label : UIElement
    {
        public string Text;
        public int FontSize;

        public Label(string text, Vector2Int position, int fontSize = 20)
        {
            Text = text;
            Position = position;
            FontSize = fontSize;

            /// calculating the height is a pain in the ass because
            /// FontSize is not in pixels and im too lazy to convert
            /// so we make it represented by a single point (no hover
            /// functionality)
            Size = new(0);
        }

        protected override void _draw()
        {
            /// simple
            try { CurrentStyle.TextColor.DrawText(CurrentStyle.Font, Text, Position, FontSize); }
            catch { DrawText(Text, Position.X, Position.Y, FontSize, CurrentStyle.TextColor.Fallback()); }
        }
    }
}
