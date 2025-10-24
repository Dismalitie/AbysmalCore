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
        /// <param name="fragPath">Filepath to the fragment shader</param>
        /// <param name="vertPath">Filepath to the vertex shader</param>
        /// <param name="color">The backup color</param>
        public ShaderBrush(string fragPath, string vertPath, Color color)
        {
            Shader = LoadShader(vertPath, fragPath);
            UserInterface.UnloadList.Add(Shader);

            fallback = color;
        }

        /// <summary>
        /// Gets a shader uniform's location
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        public int GetShaderUniform(string name) => GetShaderLocation(Shader, name);

        /// <summary>
        /// Sets a shader uniform
        /// </summary>
        /// <typeparam name="T">Uniform type</typeparam>
        /// <param name="loc">The location (<see cref="GetShaderUniform(string)"/>)</param>
        /// <param name="value">The value to set it to</param>
        /// <param name="type">The type of uniform</param>
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
