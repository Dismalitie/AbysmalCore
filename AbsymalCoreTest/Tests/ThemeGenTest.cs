using AbysmalCore.Debugging;
using AbysmalCore.UI;
using AbysmalCore.UI.Controls;
using AbysmalCore.UI.Styling.Brushes;
using Raylib_cs;

namespace AbsymalCoreTest.Tests
{
    internal class ThemeGenTest : ITest
    {
        private static Window? w;
        private static UserInterface ui = new();

        public static UserInterface GetUserInterface(Window ctx, Dictionary<string, object>? args = null)
        {
            w = ctx;

            AbysmalDebug.Enabled = true;
            ui = new();

            ui.AddElement(new Button("random theme", new(200), new(200, 50))
            {
                Name = "btn",
                StyleMap = new(true)
            });

            ui.AddElement(new Toggle(new(20, 20), new(20), false, "toggle")
            {
                Name = "tgl",
                StyleMap = new(true)
            });

            ui.AddElement(new Panel(new(20, 55), new(100), null)
            {
                Name = "radialSwatch",
                StyleMap = new(false)
                {
                    Normal = new()
                    {
                        FillColor = new SolidBrush(Color.Red),
                    }
                }
            });

            ui.AddElement(new Label("theme: t_LucideRed", new(150, 20))
            {
                Name = "theme",
                StyleMap = new(true)
            });

            ui.GetElement("btn")!.OnClicked += ThemeGenTest_OnClicked;
            ((Toggle)ui.GetElement("tgl")!).OnToggleStateChanged += ThemeGenTest_OnToggleStateChanged;

            return ui;
        }

        private static void ThemeGenTest_OnClicked(UIElement sender, AbysmalCore.Vector2Int mouse, int frame)
        {
            List<(Color c, string n)> colors = new()
            {
                (Color.Blue, "t_Blue"),
                (Color.DarkBlue, "t_DarkBlue"),
                (Color.DarkGreen, "t_DarkGreen"),
                (Color.DarkPurple, "t_DarkPurple"),
                (Color.Gold, "t_Gold"),
                (Color.Green, "t_Green"),
                (Color.Lime, "t_Lime"),
                (Color.Maroon, "t_Maroon"),
                (Color.Orange, "t_Orange"),
                (Color.Pink, "t_Pink"),
                (Color.Purple, "t_Purple"),
                (Color.Red, "t_Red"),
                (Color.SkyBlue, "t_SkyBlue"),
                (Color.Violet, "t_Violet"),
                (Color.Yellow, "t_Yellow"),
                (new Color(245, 101, 101), "t_LucideRed")
            };

            int idx = Random.Shared.Next(0, colors.Count);

            Window.GlobalTheme = new AbysmalCore.UI.Styling.Theme(colors[idx].c, Color.White);
            foreach (UIElement e in ui!.GetElements()) e.StyleMap = new(true);

            ((Label)ui.GetElement("theme")!).Text = "theme: " + colors[idx].n;
        }

        private static void ThemeGenTest_OnToggleStateChanged(UIElement sender, bool state, AbysmalCore.Vector2Int mouse, int frame)
        {
            if (state) w!.State = Window.WindowState.Fullscreen;
            else w!.State = Window.WindowState.Normal;
        }
    }
}
