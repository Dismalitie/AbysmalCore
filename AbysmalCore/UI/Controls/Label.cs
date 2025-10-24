using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Controls
{
    /// <summary>
    /// A simple text label control
    /// </summary>
    [DebugInfo("label control")]
    public class Label : UIElement
    {
        /// <summary>
        /// The text to draw
        /// </summary>
        public string Text;
        /// <summary>
        /// The size of the text
        /// </summary>
        public int FontSize;

        /// <summary>
        /// Creates a label
        /// </summary>
        /// <param name="text">The text to draw</param>
        /// <param name="position">The position relative to the top left corner of the client area</param>
        /// <param name="fontSize">The size of the text</param>
        public Label(string text, Vector2Int position, int fontSize = 20)
        {
            Text = text;
            Position = position;
            FontSize = fontSize;

            // calculating the height is a pain in the ass because
            // FontSize is not in pixels and im too lazy to convert
            // so we make it represented by a single point (no hover
            // functionality)
            Size = new(0);
        }

        /// <inheritdoc/>
        protected override void _draw()
        {
            // simple
            try { CurrentStyle.TextColor.DrawText(CurrentStyle.Font, Text, Position, FontSize); }
            catch { DrawText(Text, Position.X, Position.Y, FontSize, CurrentStyle.TextColor.Fallback()); }
        }
    }
}
