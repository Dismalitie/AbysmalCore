using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;
using System.ComponentModel;

namespace AbysmalCore.UI.Controls
{
    /// <summary>
    /// A button-like control with 2 states: on and off
    /// </summary>
    [DebugInfo("toggle control")]
    public class Toggle : UIElement, INotifyPropertyChanged
    {
        /// <summary>
        /// Delegate used when a toggle is clicked
        /// </summary>
        /// <param name="sender"></param>The toggle
        /// <param name="state"></param>The new state
        /// <param name="mouse"></param>The mouses position
        /// <param name="frame"></param>The current frame
        public delegate void StateChangedEventHandler(UIElement sender, bool state, Vector2Int mouse, int frame);
        /// <summary>
        /// Event fired when toggled
        /// </summary>
        public event StateChangedEventHandler? OnToggleStateChanged;

        /// <summary>
        /// Whether the toggle is checked or unchecked
        /// </summary>
        public bool State
        {
            // reflect the mirror
            get => _state;
            set
            {
                _state = value;
                OnToggleStateChanged?.Invoke(this, _state, UserInterface.Mouse, UserInterface.Frame);
            }
        }
        // use a mirror here because we update the value
        // in State's set accessor, and if we re-set the
        // value without this mirror, we blow up the stack
        private bool _state;

        /// <summary>
        /// The optional label next to the toggle
        /// </summary>
        public string Text;
        /// <summary>
        /// The size of the label
        /// </summary>
        public int FontSize;

        /// <summary>
        /// Creates a new toggle
        /// </summary>
        /// <param name="position">The position relative to the top left corner of the client area</param>
        /// <param name="size">The size</param>
        /// <param name="state">The default state</param>
        /// <param name="label">Optional label text</param>
        /// <param name="fontSize">Size of the optional label</param>
        public Toggle(Vector2Int position, Vector2Int size, bool state = false, string label = "", int fontSize = 18)
        {
            Position = position;
            Size = size;
            State = state;
            Text = label;
            FontSize = fontSize;

            // we can just use the base OnClicked func
            // to toggle the state, subsequently triggering
            // OnStateChanged
            OnClicked += Toggle_OnClicked;
        }

        private void Toggle_OnClicked(UIElement sender, Vector2Int mouse, int frame) => State = !State;

        /// <inheritdoc/>
        protected override void _draw()
        {
            Style current = CurrentStyle;
            if (State && Hovered) current = StyleMap.HoveredActivated!;
            else if (State) current = StyleMap.Activated!;

            current.ValidateBrushes(SupportedBrushes[StyleMap.ControlStyle], this);

            if (StyleMap.ControlStyle == StyleMap.ControlStyleType.Rounded)
            {
                if (Text != "")
                {
                    try { CurrentStyle.TextColor.DrawText(CurrentStyle.Font, Text, new(Position.X + Size.X + 7, Position.Y + (Size.Y / 3)), FontSize); }
                    catch { DrawText(Text, Position.X, Position.Y, FontSize, CurrentStyle.TextColor.Fallback()); }
                }

                try { current.BorderColor.DrawRectangleRounded(Position, Size, current.BorderRadius); }
                catch
                {
                    DrawRectangleRounded(new(Position.X, Position.Y, Size.X, Size.Y),
                    (float)current.BorderRadius / 10, 1,
                    current.BorderColor.Fallback());
                }
                Vector2Int nonBorderPos = new(Position.X + current.BorderWeight, Position.Y + current.BorderWeight);
                Vector2Int nonBorderSz = new(Size.X - current.BorderWeight * 2, Size.Y - current.BorderWeight * 2);
                try { current.FillColor.DrawRectangleRounded(nonBorderPos, nonBorderSz, current.BorderRadius); }
                catch
                {
                    DrawRectangleRounded(new(nonBorderPos.X, nonBorderPos.Y, nonBorderSz.X, nonBorderSz.Y),
                    current.BorderRadius / 10, 1,
                    current.FillColor.Fallback());
                }
            }
            else if (StyleMap.ControlStyle == StyleMap.ControlStyleType.Sharp)
            {
                try { current.BorderColor.DrawRectangle(Position, Size); }
                catch { DrawRectangle(Position.X, Position.Y, Size.X, Size.Y, current.BorderColor.Fallback()); }
                Vector2Int nonBorderPos = new(Position.X + current.BorderWeight, Position.Y + current.BorderWeight);
                Vector2Int nonBorderSz = new(Size.X - current.BorderWeight * 2, Size.Y - current.BorderWeight * 2);
                try { current.FillColor.DrawRectangle(nonBorderPos, nonBorderSz); }
                catch { DrawRectangle(nonBorderPos.X, nonBorderPos.Y, nonBorderSz.X, nonBorderSz.Y, current.FillColor.Fallback()); }
            }
        }
    }
}
