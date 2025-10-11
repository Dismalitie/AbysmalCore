using AbysmalCore.UI.Styling;
using AbysmalCore.UI.Styling.Brushes;
using System.Runtime.InteropServices.Marshalling;
using System.Xml.Linq;

namespace AbysmalCore.UI
{
    public abstract class UIElement
    {
        /// default name is * because when
        /// using GetChild you can do
        /// GetChild("*")
        public string Name = "*";
        public Vector2Int Position;
        public Vector2Int Size;

        public Dictionary<StyleMap.ControlStyleType, IBrush.BrushType[]> SupportedBrushes = new()
        {
            { StyleMap.ControlStyleType.Sharp, 
            [
                IBrush.BrushType.ImageBrush,
                IBrush.BrushType.LinearGradientBrush,
                IBrush.BrushType.RadialGradientBrush,
                IBrush.BrushType.ShaderBrush,
                IBrush.BrushType.SolidBrush,
                IBrush.BrushType.NineSliceBrush,
            ]}, 
            { StyleMap.ControlStyleType.Rounded, 
            [
                IBrush.BrushType.SolidBrush,
                IBrush.BrushType.ShaderBrush,
            ]}
        };
        public bool _UseFallbackForUnsupportedBrushes = false;

        public List<UIElement> Children { get => _children; }
        /// mirror because Children is get only and we need
        /// to manipulate the list internally with AddChild
        /// GetChild and RemoveChild
        private List<UIElement> _children = new();
        public void AddChild(UIElement element)
        {
            element.Position = new(element.Position.X + Position.X, element.Position.Y + Position.Y);
            _children.Add(element);
        }
        public void RemoveChild(UIElement element) => _children.Remove(element);
        public void RemoveChild(string name) => _children.RemoveAll(c => c.Name == name);
        public UIElement GetChild(string name) => _children.First(c => c.Name == name);

        public bool Hovered
        {
            get => _hovered;
            set
            {
                if (_hovered != value)
                {
                    OnHovered?.Invoke(this, UserInterface.Mouse, UserInterface.Frame);
                    if (value == true) OnMouseEnter?.Invoke(this, UserInterface.Mouse, UserInterface.Frame);
                    else OnMouseExit?.Invoke(this, UserInterface.Mouse, UserInterface.Frame);
                }
                _hovered = value;
            }
        }
        /// mirror to prevent recursive setting
        /// which crashes the stack
        private bool _hovered = false;
        public bool Clicked
        {
            get => _clicked;
            set
            {
                /// we do this so OnClicked on fires once
                if (_clicked != value && Enabled && Clicked)
                    OnClicked?.Invoke(this, UserInterface.Mouse, UserInterface.Frame);
                _clicked = value;
            }
        }
        /// same here
        private bool _clicked;

        public delegate void OnClickedEventArgs(UIElement sender, Vector2Int mouse, int frame);
        public delegate void OnHoveredEventArgs(UIElement sender, Vector2Int mouse, int frame);
        public event OnClickedEventArgs? OnClicked;
        public event OnHoveredEventArgs? OnHovered;

        public delegate void OnMouseEnterExitEventArgs(UIElement sender, Vector2Int mouse, int frame);
        public event OnMouseEnterExitEventArgs? OnMouseEnter;
        public event OnMouseEnterExitEventArgs? OnMouseExit;

        public bool Enabled = true;
        public bool Visible = true;

        public StyleMap StyleMap = new();
        /// dynamic accessor to make drawing
        /// a fucktonne easier
        public Style CurrentStyle
        {
            get
            {
                if (!Enabled) return StyleMap.Disabled!;
                else if (Hovered)
                {
                    if (Clicked) return StyleMap.Clicked!;
                    else return StyleMap.Hovered!;
                }
                else return StyleMap.Normal!;
            }
        }

        public abstract void _draw();
        public void Draw()
        {
            if (_UseFallbackForUnsupportedBrushes == false) StyleMap.ValidateBrushes(SupportedBrushes, this);
            /// call the actual draw function
            _draw();

            /// now we draw the children
            foreach (UIElement element in Children)
            {
                /// dont draw what we cant see
                if (!element.Visible) continue;

                /// we need to keep children within the bounds of their parent
                /// so it actually makes sense
                element.Position.X = Math.Clamp(element.Position.X, Position.X, Position.X + Size.X);
                element.Position.Y = Math.Clamp(element.Position.Y, Position.Y, Position.Y + Size.Y);
                element.Size.X = Math.Clamp(element.Size.X, 0, Size.X);
                element.Size.Y = Math.Clamp(element.Size.Y, 0, Size.Y);

                /// i just discoverd CollisionPointRec :(
                bool x = false;
                bool y = false;
                if (GetMouseX() >= element.Position.X && GetMouseX() <= element.Position.X + element.Size.X) x = true;
                if (GetMouseY() >= element.Position.Y && GetMouseY() <= element.Position.Y + element.Size.Y) y = true;

                if (x && y)
                {
                    element.Hovered = true;
                    if (IsMouseButtonDown(MouseButton.Left) && element.Enabled) element.Clicked = true;
                    else element.Clicked = false;
                }
                else element.Hovered = false;

                element.Draw();
            }
        }

        public virtual void Disable() => Enabled = false;
        public virtual void Enable() => Enabled = true;

        public virtual void Hide() => Visible = false;
        public virtual void Show() => Visible = true;
    }
}