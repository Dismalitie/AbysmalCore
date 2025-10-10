using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.UI.Styling
{
    public struct StyleMap
    {
        public enum ControlStyleType
        {
            Rounded,
            Sharp,
        }

        public ControlStyleType ControlStyle = ControlStyleType.Sharp;
        public Style Hovered = new();
        public Style Clicked = new();
        public Style Normal = new();
        public Style Disabled = new();

        public StyleMap() { }

        public void ValidateBrushes(Dictionary<ControlStyleType, IBrush.BrushType[]> supported, UIElement control)
        {
            Hovered.ValidateBrushes(supported[ControlStyle], control);
            Clicked.ValidateBrushes(supported[ControlStyle], control);
            Normal.ValidateBrushes(supported[ControlStyle], control);
            Disabled.ValidateBrushes(supported[ControlStyle], control);
        }
    }
}
