using AbysmalCore.UI;
using AbysmalCore.UI.Controls;
using AbysmalCore.UI.Styling.Brushes;
using Raylib_cs;
using static Raylib_cs.Raylib;

internal class Program
{
    private static void Main(string[] args)
    {
        UserInterface ui = new();

        ui.AddElement(new Button("button", new(10))
        {
            Name = "btn",
            StyleMap = new(true)
        });

        ui.AddElement(new Toggle(new(10, 70), new(20), new()
        {
            FillColor = new SolidBrush(new Color(245, 101, 101)),
            BorderColor = new SolidBrush(new Color(129, 52, 52)),
            BorderWeight = 2,
            BorderRadius = 5
        }, false)
        {
            Name = "tgl",
            StyleMap = new(true)
        });

        ui.GetElement("radialSwatch")?.AddChild(new Label("radialSwatch", new(10), 15));
        ui.GetElement("linearSwatch")?.AddChild(new Label("linearSwatch", new(10), 15));
        ui.GetElement("solidSwatch")?.AddChild(new Label("solidSwatch", new(10), 15));

        ui.BootstrapWindow(new(500), "AbysmalCore.UI.Window");

        UserInterface.BeginDrawingWindowIcon(new(20));
        UserInterface.GlobalTheme.DefaultStyleMap.Normal.FillColor.DrawRectangleRounded(new(0), new(20), 5);
        UserInterface.EndDrawingWindowIcon();

        ui.Init();
    }
}