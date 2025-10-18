using AbysmalCore.Debugging;
using AbysmalCore.UI.Styling.Brushes;

namespace AbysmalCore.UI.Styling
{
    [DebugInfo("ui global style")]
    public struct Theme
    {
        public StyleMap DefaultStyleMap;
        public Color Layer;
        public Color Core;
        public Color Accent;
        public Color Text;

        public Theme() { }

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

        public Theme(Color c, Color text)
        {
            Text = text;

            int layerDivisor = 3;
            int coreDivisor = 5;

            Color layer = new(c.R / layerDivisor, c.G / layerDivisor, c.B / layerDivisor);
            Debug.Log(this, $"Generated layer color {layer} from {c}");
            Color core = new(c.R / coreDivisor, c.G / coreDivisor, c.B / coreDivisor);
            Debug.Log(this, $"Generated base color {core} from {c}");

            Core = core;
            Layer = layer;
            Accent = c;

            Theme t = new(core, layer, c, text);
            DefaultStyleMap = t.DefaultStyleMap;
        }
    }
}
