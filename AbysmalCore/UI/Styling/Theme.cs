using AbysmalCore.UI.Styling.Brushes;

namespace AbysmalCore.UI.Styling
{
    public struct Theme
    {
        public StyleMap DefaultStyleMap;
        public IBrush Background = new SolidBrush(Color.White);

        public Theme() { }

        public Theme(Color core, Color layer, Color accent, Color text, StyleMap.ControlStyleType style = StyleMap.ControlStyleType.Rounded)
        {
            Background = new SolidBrush(core);
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
            };
        }

        public Theme(IBrush core, IBrush layer, IBrush accent, IBrush text, StyleMap.ControlStyleType style = StyleMap.ControlStyleType.Rounded)
        {
            Background = core;
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
                    BorderColor = new SolidBrush(Color.DarkGray),
                    TextColor = new SolidBrush(Color.DarkGray),
                },
                Clicked = new()
                {
                    BorderRadius = 5,
                    BorderWeight = 2,
                    TextColor = text,
                    BorderColor = accent,
                    FillColor = layer,
                },
            };
        }
    }
}
