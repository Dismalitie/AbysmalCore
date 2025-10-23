using AbysmalCore.Debugging;

namespace AbysmalCore.UI.Styling.Brushes
{
    /// <summary>
    /// Draws a shader
    /// </summary>
    [DebugInfo("brush")]
    public class ShaderBrush : IBrush
    {
        /// <inheritdoc/>
        public IBrush.BrushType Type => IBrush.BrushType.ShaderBrush;

        /// <summary>
        /// The shader to draw
        /// </summary>
        public Shader Shader;
        private Color fallback;
        /// <summary>
        /// Creates a new shader brush
        /// </summary>
        /// <param name="fragPath"></param>Filepath to the fragment shader
        /// <param name="vertPath"></param>Filepath to the vertex shader
        /// <param name="color"></param>The backup color
        public ShaderBrush(string fragPath, string vertPath, Color color)
        {
            Shader = LoadShader(vertPath, fragPath);
            UserInterface.UnloadList.Add(Shader);

            fallback = color;
        }

        /// <summary>
        /// Gets a shader uniform's location
        /// </summary>
        /// <param name="name"></param>The name of the uniform
        /// <returns></returns>
        public int GetShaderUniform(string name) => GetShaderLocation(Shader, name);

        /// <summary>
        /// Sets a shader uniform
        /// </summary>
        /// <typeparam name="T"></typeparam>Uniform type
        /// <param name="loc"></param>The location (<see cref="GetShaderUniform(string)"/>)
        /// <param name="value"></param>The value to set it to
        /// <param name="type"></param>The type of uniform
        public void SetShaderValue<T>(int loc, T value, ShaderUniformDataType type) where T : unmanaged =>
             Raylib.SetShaderValue(Shader, loc, value, type);

        /// <inheritdoc/>
        public void DrawRectangle(Vector2Int position, Vector2Int size)
        {
            BeginShaderMode(Shader);
            Raylib.DrawRectangle(position.X, position.Y, size.X, size.Y, fallback);
            EndShaderMode();
        }

        /// <inheritdoc/>
        public void DrawRectangleRounded(Vector2Int position, Vector2Int size, int radius)
        {
            BeginShaderMode(Shader);
            Raylib.DrawRectangleRounded(new(position.ToSys(), size.ToSys()), (float)radius / 10, 100, fallback);
            EndShaderMode();
        }

        /// <inheritdoc/>
        public void DrawText(Font font, string text, Vector2Int position, int fontSize)
        {
            BeginShaderMode(Shader);
            DrawTextEx(font, text, position.ToSys(), fontSize, 3f, fallback);
            EndShaderMode();
        }

        /// <inheritdoc/>
        public Color Fallback() => fallback;
    }
}
