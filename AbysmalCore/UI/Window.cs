using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;

namespace AbysmalCore.UI
{
    /// <summary>
    /// Contains methods and properties for managing the application window
    /// </summary>
    [DebugInfo("window instance")]
    public class Window
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
                    DwmPInvokeHelper.SetNonClientColor(hwnd, c.R, c.G, c.B);
                    DwmPInvokeHelper.SetNonClientTextColor(hwnd, t.R, t.G, t.B);
                }
                _bg = c;
                _gTheme = value;
            }
        }
        private static Theme _gTheme;

        public Window(Vector2Int size, string title, Theme? theme = null, bool resizeable = true)
        {
            /// we could make window methods static since you can only have
            /// one window, but then it would allow functions to be called
            /// before the window is actually created.

            InitWindow(size.X, size.Y, title);
            _title = title;

            if (resizeable) SetWindowState(ConfigFlags.ResizableWindow);
            SetWindowState(ConfigFlags.VSyncHint); /// prevent flickering when drawing on large sizes
            SetWindowState(ConfigFlags.AlwaysRunWindow);

            GlobalTheme = theme ?? new Theme(new Color(245, 101, 101), Color.White);
            Color c = GlobalTheme.Core;
            Color t = GlobalTheme.Text;
            unsafe
            {
                /// update window colors manually since it doesnt do
                /// it automatically on the first set
                IntPtr hwnd = (nint)GetWindowHandle();
                DwmPInvokeHelper.SetNonClientColor(hwnd, c.R, c.G, c.B);
                DwmPInvokeHelper.SetNonClientTextColor(hwnd, t.R, t.G, t.B);
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

        public enum WindowState
        {
            Normal,
            Minimized,
            Maximized,
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
        /// <param name="sz"></param>Size of the icon
        /// <param name="draw"></param>Lamba function that draws the icon
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
            /// we can actually unload it immediately
            /// since this is the only occasion we use
            /// it for and the icon references img, not
            /// _icon
            AbysmalDebug.Log(this, "Freeing icon from memory");
            UnloadRenderTexture(_icon);
            AbysmalDebug.Log(this, "Window icon draw ended");
        }

        private static Color? _bg = GlobalTheme.Core;
        private static UserInterface? _ui;

        /// <summary>
        /// Initializes the window loop with the provided <see cref="UserInterface"/>
        /// </summary>
        /// <param name="ui"></param>User interface to draw
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

            /// unload the gpu and cpu stuff here before exiting
            foreach (object obj in UserInterface.UnloadList)
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

        /// <summary>
        /// Updates the user interface to draw in the window loop
        /// </summary>
        /// <param name="ui"></param>User Interface instance
        public void SetUI(UserInterface ui) => _ui = ui;
    }
}
