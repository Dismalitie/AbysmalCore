using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.UI
{
    public class UserInterface
    {
        public List<UIElement> Elements;

        /// unload lists (gpu mem)
        public static List<Texture2D> TextureUnloadList = new();
        public static List<Shader> ShaderUnloadList = new();

        public static int Frame;

        /// ease of access
        public static Vector2Int Mouse => new(GetMouseX(), GetMouseY());

        public UserInterface(List<UIElement>? elements = null)
        {
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

            /// unload the gpu stuff here before exiting
            foreach (Texture2D t2d in TextureUnloadList) UnloadTexture(t2d);
        }
    }
}
