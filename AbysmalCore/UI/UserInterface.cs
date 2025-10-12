using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;

namespace AbysmalCore.UI
{
    [DebugInfo("AbysmalCore ui manager")]
    public class UserInterface
    {
        public List<UIElement> Elements;

        public static UserInterface? Instance;

        public static Theme GlobalTheme = new(
            new Color(40, 16, 16),    /// core
            new Color(129, 52, 52),   /// layer
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
            Instance = this;
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

        public void BootstrapWindow(Vector2Int size, string title)
        {
            InitWindow(size.X, size.Y, title);
            SetTargetFPS(60);
        }

        private static RenderTexture2D _icon;
        public void BeginDrawingWindowIcon(Vector2Int sz)
        {
            RenderTexture2D rt = LoadRenderTexture(sz.X, sz.Y);
            BeginTextureMode(rt);
            ClearBackground(Color.Blank);
            _icon = rt;

            Debug.Log(this, "Window icon draw started");
        }

        public void EndDrawingWindowIcon()
        {
            EndTextureMode();
            Image img = LoadImageFromTexture(_icon.Texture);
            SetWindowIcon(img);
            /// we can actually unload it immediately
            /// since this is the only occasion we use
            /// it for and the icon references img, not
            /// _icon
            Debug.Log(this, "Freeing icon from memory");
            UnloadRenderTexture(_icon);

            Debug.Log(this, "Window icon draw ended");
        }

        public void Init(Color? bg = null)
        {
            if (bg == null) bg = Color.White;

            while (!WindowShouldClose())
            {
                BeginDrawing();
                ClearBackground((Color)bg);
                DrawUI();
                EndDrawing();
            }

            /// unload the gpu and cpu stuff here before exiting
            foreach (object obj in UnloadList)
            {
                Debug.Log(this, $"Freeing {obj.GetType().Name} from memory");

                switch (obj)
                {
                    case Texture2D tex:
                        UnloadTexture(tex);
                        break;
                    case RenderTexture2D rt:
                        UnloadRenderTexture(rt);
                        break;
                    case Image img:
                        UnloadImage(img);
                        break;
                    case Shader sh:
                        UnloadShader(sh);
                        break;
                    default:
                        Debug.Warn(this, $"Tried to free {obj.GetType().Name}, couldnt validate type");
                        break;
                }
            }
        }
    }
}
