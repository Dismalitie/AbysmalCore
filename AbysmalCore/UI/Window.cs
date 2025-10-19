using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling;

namespace AbysmalCore.UI
{
    [DebugInfo("window instance")]
    public class Window
    {
        public Vector2Int Size
        {
            get => new(GetRenderWidth(), GetRenderHeight());
            set => SetWindowSize(value.X, value.Y);
        }
        public Vector2Int Position
        {
            get => new(GetWindowPosition());
            set => SetWindowPosition(value.X, value.Y);
        }
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

        public bool Resizable
        {
            get => IsWindowState(ConfigFlags.ResizableWindow);
            set
            {
                if (value) SetWindowState(ConfigFlags.ResizableWindow);
                else ClearWindowState(ConfigFlags.ResizableWindow);
            }
        }

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

        public void Exit() => CloseWindow();
        public void Hide() => SetWindowState(ConfigFlags.HiddenWindow);
        public void Show() => ClearWindowState(ConfigFlags.HiddenWindow);

        public enum WindowState
        {
            Normal,
            Minimized,
            Maximized,
            Fullscreen
        }
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
        public void SetIcon(Vector2Int sz, Func<object?> draw)
        {
            RenderTexture2D rt = LoadRenderTexture(sz.X, sz.Y);
            BeginTextureMode(rt);
            ClearBackground(Color.Blank);
            _icon = rt;
            Debug.Log(this, "Window icon draw started");

            draw?.Invoke();

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

        private static Color? _bg = GlobalTheme.Core;
        public void Init(UserInterface ui)
        {
            _bg ??= Color.White;

            while (!WindowShouldClose())
            {
                BeginDrawing();
                ClearBackground((Color)_bg);
                ui.DrawUI();
                EndDrawing();
            }

            /// unload the gpu and cpu stuff here before exiting
            foreach (object obj in UserInterface.UnloadList)
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
