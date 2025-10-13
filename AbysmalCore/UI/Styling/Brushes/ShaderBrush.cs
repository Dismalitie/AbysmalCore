using AbysmalCore.Debugging;
using Raylib_cs;

namespace AbysmalCore.UI.Styling.Brushes
{
    [DebugInfo("brush")]
    public class ShaderBrush : IBrush
    {
        public IBrush.BrushType Type => IBrush.BrushType.ShaderBrush;

        public Shader Shader;
        public Dictionary<string, int> Arguments = new(); /// i called them arguments because
                                                          /// i wanted to, and it represents
                                                          /// what they actually are
        private Color fallback;
        public ShaderBrush(string fragPath, string vertPath, Color color, string[] args)
        {
            Shader = LoadShader(fragPath, vertPath);
            UserInterface.UnloadList.Add(Shader);

            foreach (string arg in args) Arguments.Add(arg, GetShaderLocation(Shader, arg));
            fallback = color;
        }

        public void SetShaderValue<T>(string arg, T value, ShaderUniformDataType type) where T : unmanaged =>
             Raylib.SetShaderValue(Shader, Arguments[arg], value, type);

        public void DrawRectangle(Vector2Int position, Vector2Int size)
        {
            BeginShaderMode(Shader);
            Raylib.DrawRectangle(position.X, position.Y, size.X, size.Y, fallback);
            EndShaderMode();
        }

        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius)
        {
            BeginShaderMode(Shader);
            Raylib.DrawRectangleRounded(new(position.ToSys(), size.ToSys()), (float)radius / 10, 100, fallback);
            EndShaderMode();
        }

        public void DrawText(Font font, string text, Vector2Int position, int fontSize)
        {
            BeginShaderMode(Shader);
            DrawTextEx(font, text, position.ToSys(), fontSize, 3f, fallback);
            EndShaderMode();
        }

        public Color Fallback() => fallback;
    }
}
