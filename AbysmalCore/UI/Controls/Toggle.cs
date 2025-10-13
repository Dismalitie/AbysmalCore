using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;

namespace AbysmalCore.UI.Controls
{
    [DebugInfo("toggle control")]
    public class Toggle : UIElement
    {
        public delegate void StateChangedEventHandler(UIElement sender, bool state, Vector2Int mouse, int frame);
        public event StateChangedEventHandler? OnToggleStateChanged;

        public bool State
        {
            /// reflect the mirror
            get => _state;
            set
            {
                OnToggleStateChanged?.Invoke(this, _state, UserInterface.Mouse, UserInterface.Frame);
                _state = value;
            }
        }
        /// use a mirror here because we update the value
        /// in State's set accessor, and if we re-set the
        /// value without this mirror, we blow up the stack
        private bool _state;

        public string Text;
        public int FontSize;
        public Toggle(Vector2Int position, Vector2Int size, bool state = false, string label = "", int fontSize = 18)
        {
            Position = position;
            Size = size;
            State = state;
            Text = label;
            FontSize = fontSize;

            /// we can just use the base OnClicked func
            /// to toggle the state, subsequently triggering
            /// OnStateChanged
            OnClicked += Toggle_OnClicked;
        }

        private void Toggle_OnClicked(UIElement sender, Vector2Int mouse, int frame) => State = !State;

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
