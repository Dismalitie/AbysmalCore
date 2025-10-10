using AbysmalCore.UI.Styling;

namespace AbysmalCore.UI.Controls
{
    public class Toggle : UIElement
    {
        public delegate void StateChangedEventHandler(UIElement sender, bool state, Vector2Int mouse, int frame);
        public event StateChangedEventHandler? OnStateChanged;

        public bool State
        {
            /// reflect the mirror
            get => _state;
            set
            {
                OnStateChanged?.Invoke(this, _state, UserInterface.Mouse, UserInterface.Frame);
                _state = value;
            }
        }
        /// use a mirror here because we update the value
        /// in State's set accessor, and if we re-set the
        /// value without this mirror, we blow up the stack
        private bool _state;

        public Style ToggledStyle;
        public Toggle(Vector2Int position, Vector2Int size, Style toggledStyle, bool state = false)
        {
            Position = position;
            Size = size;
            State = state;
            ToggledStyle = toggledStyle;

            /// we can just use the base OnClicked func
            /// to toggle the state, subsequently triggering
            /// OnStateChanged
            OnClicked += Toggle_OnClicked;
        }

        private void Toggle_OnClicked(UIElement sender, Vector2Int mouse, int frame) => State = !State;

        public override void _draw()
        {
            Style current = CurrentStyle;
            if (State) current = ToggledStyle;

            current.ValidateBrushes(SupportedBrushes[StyleMap.ControlStyle], this);

            try { current.BorderColor.DrawRectangle(Position, Size); }
            catch { DrawRectangle(Position.X, Position.Y, Size.X, Size.Y, current.BorderColor.Fallback()); }

            Vector2Int nonBorderPos = new(Position.X + current.BorderWeight, Position.Y + current.BorderWeight);
            Vector2Int nonBorderSz = new(Size.X - current.BorderWeight * 2, Size.Y - current.BorderWeight * 2);
            try { current.FillColor.DrawRectangle(nonBorderPos, nonBorderSz); }
            catch { DrawRectangle(nonBorderPos.X, nonBorderPos.Y, nonBorderSz.X, nonBorderSz.Y, current.FillColor.Fallback()); }
        }
    }
}
