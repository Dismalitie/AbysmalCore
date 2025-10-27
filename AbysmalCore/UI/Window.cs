using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;
using System.ComponentModel;

namespace AbysmalCore.UI
{
    /// <summary>
    /// Contains methods and properties for managing the application window
    /// </summary>
    [DebugInfo("window instance")]
    public class Window : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Size of the window in pixels
        /// </summary>
        public Vector2Int Size
        {
            get => new(GetRenderWidth(), GetRenderHeight());
            set => SetWindowSize(value.X, value.Y);
        }
        /// <summary>
        /// The position of the window relative to the top left of the screen
        /// </summary>
        public Vector2Int Position
        {
            get => new(GetWindowPosition());
            set => SetWindowPosition(value.X, value.Y);
        }
        /// <summary>
        /// Text caption displayed in the window title bar
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                SetWindowTitle(value);
                _title = value;
            }
        }
        private string _title;

        /// <summary>
        /// Determines if window resize handles and maximize button are available
        /// </summary>
        public bool Resizable
        {
            get => IsWindowState(ConfigFlags.ResizableWindow);
            set
            {
                if (value) SetWindowState(ConfigFlags.ResizableWindow);
                else ClearWindowState(ConfigFlags.ResizableWindow);
            }
        }

        /// <summary>
        /// Global theme of colors to use by default across the GUI
        /// </summary>
        public static Theme GlobalTheme
        {
            get => _gTheme;
            set
            {
                Color c = value.Core;
                Color t = value.Text;
                unsafe
                {
                    IntPtr hwnd = (nint)GetWindowHandle();
                    DwmPInvokeHelper.SetNonClientColor(hwnd, (c.R, c.G, c.B));
                    DwmPInvokeHelper.SetNonClientTextColor(hwnd, (t.R, t.G, t.B));
                }
                _bg = c;
                _gTheme = value;
            }
        }
        private static Theme _gTheme;

        /// <summary>
        /// Creates a window
        /// </summary>
        /// <param name="size">The size</param>
        /// <param name="title">The caption in the titlebar</param>
        /// <param name="theme">The defualt theme</param>
        /// <param name="resizeable">Whether the window is resizable</param>
        public Window(Vector2Int size, string title, Theme? theme = null, bool resizeable = true)
        {
            // we could make window methods static since you can only have
            // one window, but then it would allow functions to be called
            // before the window is actually created.

            InitWindow(size.X, size.Y, title);
            _title = title;

            if (resizeable) SetWindowState(ConfigFlags.ResizableWindow);
            SetWindowState(ConfigFlags.VSyncHint); // prevent flickering when drawing on large sizes
            SetWindowState(ConfigFlags.AlwaysRunWindow);

            GlobalTheme = theme ?? new Theme(new Color(245, 101, 101), Color.White);
            Color c = GlobalTheme.Core;
            Color t = GlobalTheme.Text;
            unsafe
            {
                // update window colors manually since it doesnt do
                // it automatically on the first set
                IntPtr hwnd = (nint)GetWindowHandle();
                DwmPInvokeHelper.SetNonClientColor(hwnd, (c.R, c.G, c.B));
                DwmPInvokeHelper.SetNonClientTextColor(hwnd, (t.R, t.G, t.B));
            }
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        public void Exit() => CloseWindow();
        /// <summary>
        /// Hides the window
        /// </summary>
        public void Hide() => SetWindowState(ConfigFlags.HiddenWindow);
        /// <summary>
        /// Unhides the window
        /// </summary>
        public void Show() => ClearWindowState(ConfigFlags.HiddenWindow);

        /// <summary>
        /// Represents the current view of the window
        /// </summary>
        public enum WindowState
        {
            /// <summary>
            /// No other states applicable
            /// </summary>
            Normal,
            /// <summary>
            /// Not visible, still running
            /// </summary>
            Minimized,
            /// <summary>
            /// Fully visible
            /// </summary>
            Maximized,
            /// <summary>
            /// Fully visible, no titlebar
            /// </summary>
            Fullscreen
        }

        /// <summary>
        /// The current state of the window (e.g. minimized, maximized, fullscreen, normal)
        /// </summary>
        public WindowState State
        {
            get => _wState;
            set
            {
                switch (value)
                {
                    case WindowState.Normal:
                        if (_wState == WindowState.Fullscreen) ToggleBorderlessWindowed();
                        else RestoreWindow();
                        break;
                    case WindowState.Minimized:
                        MinimizeWindow();
                        break;
                    case WindowState.Maximized:
                        MaximizeWindow();
                        break;
                    case WindowState.Fullscreen:
                        if (_wState != WindowState.Fullscreen) ToggleBorderlessWindowed();
                        break;
                }

                _wState = value;
            }
        }
        private WindowState _wState = WindowState.Normal;

        private RenderTexture2D _icon;

        /// <summary>
        /// Draws and sets the window icon using the provided lambda function
        /// </summary>
        /// <param name="sz">Size of the icon</param>
        /// <param name="draw">Lamba function that draws the icon</param>
        public void SetIcon(Vector2Int sz, Func<object?> draw)
        {
            RenderTexture2D rt = LoadRenderTexture(sz.X, sz.Y);
            BeginTextureMode(rt);
            ClearBackground(Color.Blank);
            _icon = rt;
            AbysmalDebug.Log(this, "Window icon draw started");

            draw?.Invoke();

            EndTextureMode();
            Image img = LoadImageFromTexture(_icon.Texture);
            SetWindowIcon(img);
            // we can actually unload it immediately
            // since this is the only occasion we use
            // it for and the icon references img, not
            // _icon
            AbysmalDebug.Log(this, "Freeing icon from memory");
            UnloadRenderTexture(_icon);
            AbysmalDebug.Log(this, "Window icon draw ended");
        }

        private static Color? _bg = GlobalTheme.Core;
        private static UserInterface? _ui;
        /// <summary>
        /// Fires when a property of this <see cref="Window"/> changes
        /// </summary>

        public event PropertyChangedEventHandler? PropertyChanged;

        // objects like tex2d, rendertextures, images
        // in this list will be unloaded upon exiting
        /// <summary>
        /// A list of objects to unload when the window is closed
        /// </summary>
        /// <remarks>
        /// Should be:
        /// <see cref="Texture2D"/>,
        /// <see cref="RenderTexture2D"/>,
        /// <see cref="Image"/> or
        /// <see cref="Shader"/>
        /// </remarks>
        public static List<object> UnloadList = new();

        /// <summary>
        /// Initializes the window loop with the provided <see cref="UserInterface"/>
        /// </summary>
        /// <param name="ui">User interface to draw</param>
        public void Init(UserInterface ui)
        {
            _ui = ui;
            _bg ??= Color.White;

            while (!WindowShouldClose())
            {
                BeginDrawing();
                ClearBackground((Color)_bg);
                _ui.DrawUI();
                EndDrawing();
            }

            Dispose();
        }

        /// <summary>
        /// Updates the user interface to draw in the window loop
        /// </summary>
        /// <param name="ui">User Interface instance</param>
        public void SetUI(UserInterface ui)
        {
            // we can dispose the current resources
            // because they will just be generated again
            // upon setting
            Dispose();
            _ui = ui;
        }

        /// <summary>
        /// Unloads GPU allocated resources
        /// </summary>
        public void Dispose()
        {
            foreach (object obj in UnloadList)
            {
                AbysmalDebug.Log(this, $"Freeing {obj.GetType().Name} from memory");

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
                        AbysmalDebug.Warn(this, $"Tried to free {obj.GetType().Name}, couldnt validate type");
                        break;
                }
            }
        }
    }
}
