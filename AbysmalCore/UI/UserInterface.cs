using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;

namespace AbysmalCore.UI
{
    [DebugInfo("ui manager")]
    public class UserInterface
    {
        public List<UIElement> Elements;
        //public static UserInterface? Instance;

        //public static Theme GlobalTheme = new(
        //    new Color(40, 16, 16),    /// core
        //    new Color(129, 52, 52),   /// layer
        //    new Color(245, 101, 101), /// accent
        //    Color.White
        //);

        public static Theme GlobalTheme = new(
            new Color(245, 101, 101), /// accent
            Color.White
        );

        /// objects like tex2d, rendertextures, images
        /// in this list will be unloaded upon exiting
        public static List<object> UnloadList = new();

        public static int Frame;

        /// ease of access
        public static Vector2Int Mouse => new(GetMouseX(), GetMouseY());

        public UserInterface(List<UIElement>? elements = null)
        {
            //Instance = this;
            if (elements == null) Elements = new();
            else Elements = elements;
        }
        public void RemoveElement(UIElement element) => Elements.Remove(element);
        public void RemoveElement(string name) => Elements.RemoveAll(c => c.Name == name);
        public UIElement? GetElement(string name)
        {
            try { return Elements.First(c => c.Name == name); } catch { }
            return null;
        }

        public UIElement[] GetElements() => Elements.ToArray();
        public void AddElement(UIElement element) => Elements.Add(element);

        public void DrawUI()
        {
            if (GlobalTheme.Background != null) GlobalTheme.Background.DrawRectangle(new(0, 0), new(GetRenderWidth(), GetRenderHeight()));

            Frame++;
            foreach (UIElement element in Elements)
            {
                if (!element.Visible) continue;

                bool x = false;
                bool y = false;

                if (GetMouseX() >= element.Position.X && GetMouseX() <= element.Position.X + element.Size.X) x = true;
                if (GetMouseY() >= element.Position.Y && GetMouseY() <= element.Position.Y + element.Size.Y) y = true;

                if (x && y)
                {
                    element.Hovered = true;
                    if (IsMouseButtonPressed(MouseButton.Left) && element.Enabled) element.Clicked = true;
                    else element.Clicked = false;
                }
                else element.Hovered = false;

                element.Draw();
            }
        }
    }
}
