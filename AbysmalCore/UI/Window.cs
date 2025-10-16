using AbysmalCore.Debugging;

namespace AbysmalCore.UI
{
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

        public Window(Vector2Int size, string title, bool resizeable = true)
        {
            InitWindow(size.X, size.Y, title);
            _title = title;

            if (resizeable) SetWindowState(ConfigFlags.ResizableWindow);
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

        public void Init(UserInterface ui, Color? bg = null)
        {
            bg ??= Color.White;

            while (!WindowShouldClose())
            {
                BeginDrawing();
                ClearBackground((Color)bg);
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
