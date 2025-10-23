using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling.Brushes;

namespace AbysmalCore.UI.Styling
{
    /// <summary>
    /// Represents a global theme for the user interface including palette colors and default styles
    /// </summary>
    [DebugInfo("ui global style")]
    public struct Theme
    {
        /// <summary>
        /// Stylemap to use when no brushes specified
        /// </summary>
        public StyleMap DefaultStyleMap;
        /// <summary>
        /// The middle color
        /// </summary>
        public Color Layer;
        /// <summary>
        /// The base color
        /// </summary>
        public Color Core;
        /// <summary>
        /// The accent color
        /// </summary>
        public Color Accent;
        /// <summary>
        /// Color to use when drawing text
        /// </summary>
        public Color Text;

        /// <summary>
        /// Creates a new theme
        /// </summary>
        public Theme() { }

        /// <summary>
        /// Creates a new theme from 3 colors and a control style
        /// </summary>
        /// <param name="core"></param>The
        /// <param name="layer"></param>
        /// <param name="accent"></param>
        /// <param name="text"></param>
        /// <param name="style"></param>
        public Theme(Color core, Color layer, Color accent, Color text, StyleMap.ControlStyleType style = StyleMap.ControlStyleType.Rounded)
        {
            Core = core;
            Layer = layer;
            Accent = accent;
            Text = text;

            DefaultStyleMap = new()
            {
                ControlStyle = StyleMap.ControlStyleType.Rounded,
                Normal = new()
                {
                    BorderRadius = 5,
                    BorderWeight = 2,
                    TextColor = new SolidBrush(text),
                    BorderColor = new SolidBrush(accent),
                    FillColor = new SolidBrush(layer),
                },
                Hovered = new()
                {
                    BorderRadius = 5,
                    BorderWeight = 2,
                    TextColor = new SolidBrush(accent),
                    FillColor = new SolidBrush(core),
                    BorderColor = new SolidBrush(accent),
                },
                Disabled = new()
                {
                    FillColor = new SolidBrush(core),
                    BorderColor = new SolidBrush(Color.Gray),
                    TextColor = new SolidBrush(Color.Gray),
                },
                HoveredDisabled = new()
                {
                    FillColor = new SolidBrush(core),
                    BorderColor = new SolidBrush(Color.DarkGray),
                    TextColor = new SolidBrush(Color.DarkGray),
                },
                Clicked = new()
                {
                    BorderRadius = 5,
                    BorderWeight = 2,
                    TextColor = new SolidBrush(text),
                    BorderColor = new SolidBrush(accent),
                    FillColor = new SolidBrush(layer),
                },
                Activated = new()
                {
                    FillColor = new SolidBrush(accent),
                    BorderColor = new SolidBrush(layer),
                    BorderWeight = 2,
                    BorderRadius = 5
                },
                HoveredActivated = new()
                {
                    FillColor = new SolidBrush(accent),
                    BorderColor = new SolidBrush(layer),
                    BorderWeight = 3,
                    BorderRadius = 5
                }
            };
        }

        /// <summary>
        /// Creates a new theme from 3 brushes and a control style
        /// </summary>
        /// <param name="core"></param>
        /// <param name="layer"></param>
        /// <param name="accent"></param>
        /// <param name="text"></param>
        /// <param name="style"></param>
        public Theme(IBrush core, IBrush layer, IBrush accent, IBrush text, StyleMap.ControlStyleType style = StyleMap.ControlStyleType.Rounded)
        {
            Core = core.Fallback();
            Layer = layer.Fallback();
            Accent = accent.Fallback();
            Text = text.Fallback();

            DefaultStyleMap = new()
            {
                ControlStyle = StyleMap.ControlStyleType.Rounded,
                Normal = new()
                {
                    BorderRadius = 5,
                    BorderWeight = 2,
                    TextColor = text,
                    BorderColor = accent,
                    FillColor = layer
                },
                Hovered = new()
                {
                    BorderRadius = 5,
                    BorderWeight = 2,
                    TextColor = accent,
                    FillColor = core,
                    BorderColor = accent,
                },
                Disabled = new()
                {
                    FillColor = core,
                    BorderColor = new SolidBrush(Color.Gray),
                    TextColor = new SolidBrush(Color.Gray),
                },
                Clicked = new()
                {
                    BorderRadius = 5,
                    BorderWeight = 2,
                    TextColor = text,
                    BorderColor = accent,
                    FillColor = layer,
                },
                Activated = new()
                {
                    FillColor = accent,
                    BorderColor = layer,
                    BorderWeight = 2,
                    BorderRadius = 5
                },
                HoveredActivated = new()
                {
                    FillColor = accent,
                    BorderColor = layer,
                    BorderWeight = 3,
                    BorderRadius = 5
                },
                HoveredDisabled = new()
                {
                    FillColor = core,
                    BorderColor = new SolidBrush(Color.DarkGray),
                    TextColor = new SolidBrush(Color.DarkGray),
                }
            };
        }

        /// <summary>
        /// Dynamically creates a new theme from a single color
        /// </summary>
        /// <param name="c"></param>The color
        /// <param name="text"></param>The color to use on text
        public Theme(Color c, Color text)
        {
            Text = text;

            int layerDivisor = 3;
            int coreDivisor = 5;

            Color layer = new(c.R / layerDivisor, c.G / layerDivisor, c.B / layerDivisor);
            AbysmalDebug.Log(this, $"Generated layer color {layer} from {c}");
            Color core = new(c.R / coreDivisor, c.G / coreDivisor, c.B / coreDivisor);
            AbysmalDebug.Log(this, $"Generated base color {core} from {c}");

            Core = core;
            Layer = layer;
            Accent = c;

            Theme t = new(core, layer, c, text);
            DefaultStyleMap = t.DefaultStyleMap;
        }
    }
}
