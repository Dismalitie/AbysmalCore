using AbysmalCore.UI;
using AbysmalCore.UI.Controls;
using AbysmalCore.UI.Styling.Brushes;
using static Raylib_cs.Raylib;

internal class Program
{
    private static void Main(string[] args)
    {
        UserInterface ui = new();

        ui.AddElement(new Button("AbysmalCore.UI.Controls.Button", new(10), new(400, 50))
        {
            Name = "btn",
            StyleMap = new(true)
        });

        ui.AddElement(new Toggle(new(10, 70), new(20), new()
        {
            FillColor = new SolidBrush(new Raylib_cs.Color(245, 101, 101)),
            BorderColor = new SolidBrush(new Raylib_cs.Color(129, 52, 52)),
            BorderWeight = 2,
            BorderRadius = 5
        }, false, "AbysmalCore.UI.Controls.Toggle")
        {
            Name = "tgl",
            StyleMap = new(true)
        });

        ui.GetElement("radialSwatch")?.AddChild(new Label("radialSwatch", new(10), 15));
        ui.GetElement("linearSwatch")?.AddChild(new Label("linearSwatch", new(10), 15));
        ui.GetElement("solidSwatch")?.AddChild(new Label("solidSwatch", new(10), 15));

        ui.BootstrapWindow(new(500), "AbysmalCore.UI.Window");
        ui.Init();
    }
}