using AbysmalCore;
using AbysmalCore.Debugging;
using AbysmalCore.UI;
using AbysmalCore.UI.Controls;
using AbysmalCore.UI.Styling.Brushes;
using Raylib_cs;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        Debug.Enabled = true;
        UserInterface ui = new();

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

        ui.AddElement(new Label("theme: ct_LucideRed", new(150, 20))
        {
            Name = "theme",
            StyleMap = new(true)
        });

        ui.BootstrapWindow(new(500), "AbysmalCore.UI.Window");

        ui.GetElement("btn")!.OnClicked += Program_OnClicked;

        Debug.WriteLogs(".\\test");

        ui.Init();
    }

    private static void Program_OnClicked(UIElement sender, Vector2Int mouse, int frame)
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
            (new Color(245, 101, 101), "ct_LucideRed")
        };

        int idx = Random.Shared.Next(0, colors.Count);

        UserInterface.GlobalTheme = new AbysmalCore.UI.Styling.Theme(colors[idx].c, Color.White);
        foreach (UIElement e in UserInterface.Instance!.GetElements()) e.StyleMap = new(true);

        ((Label)UserInterface.Instance!.GetElement("theme")!).Text = "theme: " + colors[idx].n;
    }
}