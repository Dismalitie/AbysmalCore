using AbysmalCore.UI.Styling;
using System.ComponentModel;
using System.Xml.Linq;

namespace AbysmalCore.UI
{
    /// <summary>
    /// Represents an abstraction of a user interface element
    /// </summary>
    public abstract class UIElement : INotifyPropertyChanged
    {
        // default name is * because when
        // using GetChild you can do
        // GetChild("*")
        /// <summary>
        /// Unique identifier of this <see cref="UIElement"/> (used for retrieval with <see cref="GetChild(string)"/>)
        /// </summary>
        public string Name = "*";
        /// <summary>
        /// Position of this <see cref="UIElement"/> relative to the top left of its parent
        /// </summary>
        public Vector2Int Position;
        /// <summary>
        /// Size of this <see cref="UIElement"/> in pixels
        /// </summary>
        public Vector2Int Size;

        /// <summary>
        /// Dictionary of supported brushes for each style type (used for validation)
        /// </summary>
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
        /// <summary>
        /// If true, unsupported brushes will use their fallback color instead of throwing an exception
        /// </summary>
        protected bool _UseFallbackForUnsupportedBrushes = false;

        /// <summary>
        /// Child controls of this <see cref="UIElement"/>
        /// </summary>
        public List<UIElement> Children { get => _children; }
        /// mirror because Children is get only and we need
        /// to manipulate the list internally with AddChild
        /// GetChild and RemoveChild
        private List<UIElement> _children = new();
        /// <summary>
        /// Adds a child <see cref="UIElement"/>
        /// </summary>
        /// <param name="element">Instance of the element to add</param>
        public void AddChild(UIElement element)
        {
            element.Position = new(element.Position.X + Position.X, element.Position.Y + Position.Y);
            _children.Add(element);
        }
        /// <summary>
        /// Removes a child <see cref="UIElement"/>
        /// </summary>
        /// <param name="element">Instance of the element to remove</param>
        public void RemoveChild(UIElement element) => _children.Remove(element);
        /// <summary>
        /// Removes the first child <see cref="UIElement"/> with the specified name
        /// </summary>
        /// <param name="name">Name of the element to remove</param>
        public void RemoveChild(string name) => _children.RemoveAll(c => c.Name == name);
        /// <summary>
        /// Returns the first child <see cref="UIElement"/> with the specified name
        /// </summary>
        /// <param name="name">Name of the element to retrieve</param>
        /// <returns>First instance of <see cref="UIElement"/> with <paramref name="name"/>, else null</returns>
        public UIElement? GetChild(string name)
        {
            try { return Children.First(c => c.Name == name); } catch { }
            return null;
        }
        /// <summary>
        /// Returns every child <see cref="UIElement"/>
        /// </summary>
        /// <returns><see cref="UIElement"/>[]</returns>
        public UIElement[] GetChildren() => _children.ToArray();

        /// <summary>
        /// Whether the mouse is currently hovering over this <see cref="UIElement"/>
        /// </summary>
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

                    _hovered = value;
                }
            }
        }
        // mirror to prevent recursive setting
        // which crashes the stack
        private bool _hovered = false;
        /// <summary>
        /// Determines whether the mouse is over this <see cref="UIElement"/> and the left mouse button is held down
        /// </summary>
        public bool Clicked
        {
            get => _clicked;
            set
            {
                // we do this so OnClicked on fires once
                if (_clicked != value && Enabled && Clicked)
                    OnClicked?.Invoke(this, UserInterface.Mouse, UserInterface.Frame);
                _clicked = value;
            }
        }
        // same here
        private bool _clicked;
        /// <summary>
        /// Delegate for when the control is clicked
        /// </summary>
        /// <param name="sender">The control</param>
        /// <param name="mouse">The mouse position</param>
        /// <param name="frame">The current frame</param>
        public delegate void OnClickedEventArgs(UIElement sender, Vector2Int mouse, int frame);
        /// <summary>
        /// Delegate for when the control is hovered over
        /// </summary>
        /// <param name="sender">The control</param>
        /// <param name="mouse">The mouse position</param>
        /// <param name="frame">The current frame</param>
        public delegate void OnHoveredEventArgs(UIElement sender, Vector2Int mouse, int frame);
        /// <summary>
        /// The property that changed
        /// </summary>
        public enum StateChangeType 
        { 
            /// <summary>
            /// The enabled property
            /// </summary>
            Enabled, 
            /// <summary>
            /// The visibility property
            /// </summary>
            Visible,
            /// <summary>
            /// The hovered property
            /// </summary>
            Hovered, 
            /// <summary>
            /// The clicked property
            /// </summary>
            Clicked 
        }
        /// <summary>
        /// Fired when this <see cref="UIElement"/> is clicked
        /// </summary>
        public event OnClickedEventArgs? OnClicked;
        /// <summary>
        /// Fired once upon hovering over this <see cref="UIElement"/>
        /// </summary>
        public event OnHoveredEventArgs? OnHovered;

        /// <summary>
        /// Delegate used when the mouse enters or exits the controls bounds
        /// </summary>
        /// <param name="sender">The control</param>
        /// <param name="mouse">The mouse position</param>
        /// <param name="frame">The current frame</param>
        public delegate void OnMouseEnterExitEventArgs(UIElement sender, Vector2Int mouse, int frame);
        /// <summary>
        /// Fired once when the mouse enters the bounds of this <see cref="UIElement"/>
        /// </summary>
        public event OnMouseEnterExitEventArgs? OnMouseEnter;
        /// <summary>
        /// Fired once when the mouse exits the bounds of this <see cref="UIElement"/>
        /// </summary>
        public event OnMouseEnterExitEventArgs? OnMouseExit;
        /// <summary>
        /// Fired when an element of this <see cref="UIElement"/> changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Determines whether this <see cref="UIElement"/> is enabled (can be interacted with)
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value) _enabled = value;
            }
        }
        private bool _enabled = true;

        /// <summary>
        /// Determines whether this <see cref="UIElement"/> is visible (drawn)
        /// </summary>
        public bool Visible
        {
            get => _visible;
            set
            {
                if (_visible != value) _visible = value;
            }
        }
        private bool _visible = true;

        /// <summary>
        /// Style map containing styles for different states of this <see cref="UIElement"/>
        /// </summary>
        public StyleMap StyleMap = new();
        // dynamic accessor to make drawing
        // a fucktonne easier
        /// <summary>
        /// Returns the current <see cref="Style"/> based on the state of this <see cref="UIElement"/>
        /// </summary>
        public Style CurrentStyle
        {
            get
            {
                if (!Enabled && Hovered) return StyleMap.HoveredDisabled!;
                else if (!Enabled) return StyleMap.Disabled!;
                else if (Hovered)
                {
                    if (Clicked) return StyleMap.Clicked!;
                    else return StyleMap.Hovered!;
                }
                else return StyleMap.Normal;
            }
        }

        /// <summary>
        /// Draws the actual control
        /// </summary>
        protected abstract void _draw();
        /// <summary>
        /// Draws this <see cref="UIElement"/> and its children
        /// </summary>
        public void Draw()
        {
            if (_UseFallbackForUnsupportedBrushes == false) StyleMap.ValidateBrushes(SupportedBrushes, this);
            // call the actual draw function
            _draw();

            // now we draw the children
            foreach (UIElement element in Children)
            {
                // dont draw what we cant see
                if (!element.Visible) continue;

                // we need to keep children within the bounds of their parent
                // so it actually makes sense
                element.Position.X = Math.Clamp(element.Position.X, Position.X, Position.X + Size.X);
                element.Position.Y = Math.Clamp(element.Position.Y, Position.Y, Position.Y + Size.Y);
                element.Size.X = Math.Clamp(element.Size.X, 0, Size.X);
                element.Size.Y = Math.Clamp(element.Size.Y, 0, Size.Y);

                // i just discoverd CollisionPointRec :(
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

        /// <summary>
        /// Disables interaction with this <see cref="UIElement"/>
        /// </summary>
        public virtual void Disable() => Enabled = false;
        /// <summary>
        /// Enables interaction with this <see cref="UIElement"/>
        /// </summary>
        public virtual void Enable() => Enabled = true;

        /// <summary>
        /// Hides this <see cref="UIElement"/>
        /// </summary>
        public virtual void Hide() => Visible = false;
        /// <summary>
        /// Unhides this <see cref="UIElement"/>
        /// </summary>
        public virtual void Show() => Visible = true;
    }
}