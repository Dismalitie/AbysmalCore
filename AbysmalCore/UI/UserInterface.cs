using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;

namespace AbysmalCore.UI
{
    /// <summary>
    /// Contains methods and properties for managing the user interface
    /// </summary>
    [DebugInfo("ui manager")]
    public class UserInterface
    {
        /// <summary>
        /// List of all UI elements in the GUI. Should be manipulated via the provided methods.
        /// </summary>
        public List<UIElement> Elements { get; private set; }

        /// objects like tex2d, rendertextures, images
        /// in this list will be unloaded upon exiting
        /// 
        /// <summary>
        /// A list of objects to unload when the window is closed. Should be
        /// <see cref="Texture2D"/>,
        /// <see cref="RenderTexture2D"/>,
        /// <see cref="Image"/> or
        /// <see cref="Shader"/>
        /// </summary>
        public static List<object> UnloadList = new();

        /// <summary>
        /// Keeps track of the number of times <see cref="DrawUI"/> has been called
        /// </summary>
        public static int Frame { get; private set; }

        /// ease of access
        /// 
        /// <summary>
        /// Represents the current mouse position (not relative to any element)
        /// </summary>
        public static Vector2Int Mouse => new(GetMouseX(), GetMouseY());

        /// <summary>
        /// Creates a new user interface
        /// </summary>
        /// <param name="elements"></param>Inital children
        public UserInterface(List<UIElement>? elements = null)
        {
            //Instance = this; 
            if (elements == null) Elements = new();
            else Elements = elements;
        }

        /// <summary>
        /// Removes a <see cref="UIElement"/> from the GUI
        /// </summary>
        /// <param name="element"></param>Instance of the element to remove
        public void RemoveElement(UIElement element) => Elements.Remove(element);

        /// <summary>
        /// Removes the first <see cref="UIElement"/> with the specified name from the GUI
        /// </summary>
        /// <param name="name"></param>Name of the element to remove
        public void RemoveElement(string name) => Elements.RemoveAll(c => c.Name == name);

        /// <summary>
        /// Returns the first <see cref="UIElement"/> with the specified name from the GUI
        /// </summary>
        /// <param name="name"></param>Name of the element to retrieve
        /// <returns>First instance of <see cref="UIElement"/> with <paramref name="name"/>, else null</returns>
        public UIElement? GetElement(string name)
        {
            try { return Elements.First(c => c.Name == name); } catch { }
            return null;
        }

        /// <summary>
        /// Returns every <see cref="UIElement"/> in the GUI
        /// </summary>
        /// <returns><see cref="UIElement"/>[]</returns>
        public UIElement[] GetElements() => Elements.ToArray();

        /// <summary>
        /// Adds a <see cref="UIElement"/> to the GUI
        /// </summary>
        /// <param name="element"></param>Instance of the element to add
        public void AddElement(UIElement element) => Elements.Add(element);

        /// <summary>
        /// Increments frame and draws every element in the GUI
        /// </summary>
        public void DrawUI()
        {
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
