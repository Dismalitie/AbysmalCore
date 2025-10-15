using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Styling.Brushes
{
    [DebugInfo("brush")]
    public class ShaderBrush : IBrush
    {
        public IBrush.BrushType Type => IBrush.BrushType.ShaderBrush;

        public Shader Shader;
        private Color fallback;
        public ShaderBrush(string fragPath, string vertPath, Color color)
        {
            Shader = LoadShader(vertPath, fragPath);
            UserInterface.UnloadList.Add(Shader);

            fallback = color;
        }

        public int GetShaderUniform(string name) => GetShaderLocation(Shader, name);

        public void SetShaderValue<T>(int loc, T value, ShaderUniformDataType type) where T : unmanaged =>
             Raylib.SetShaderValue(Shader, loc, value, type);

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
