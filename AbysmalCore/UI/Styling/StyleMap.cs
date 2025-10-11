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
        public Style Hovered;
        public Style Clicked;
        public Style Normal;
        public Style Disabled;

        public StyleMap(bool useGlobalTheme = false)
        {
            if (useGlobalTheme)
            {
                Hovered ??= UserInterface.GlobalTheme.DefaultStyleMap.Hovered;
                Clicked ??= UserInterface.GlobalTheme.DefaultStyleMap.Clicked;
                Normal ??= UserInterface.GlobalTheme.DefaultStyleMap.Normal;
                Disabled ??= UserInterface.GlobalTheme.DefaultStyleMap.Disabled;
                ControlStyle = UserInterface.GlobalTheme.DefaultStyleMap.ControlStyle;
            }
        }

        public void ValidateBrushes(Dictionary<ControlStyleType, IBrush.BrushType[]> supported, UIElement control)
        {
            Hovered.ValidateBrushes(supported[ControlStyle], control);
            Clicked.ValidateBrushes(supported[ControlStyle], control);
            Normal.ValidateBrushes(supported[ControlStyle], control);
            Disabled.ValidateBrushes(supported[ControlStyle], control);
        }
    }
}
