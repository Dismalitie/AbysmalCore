using AbysmalCore;
using AbysmalCore.Debugging;
using AbysmalCore.UI;
using AbysmalCore.UI.Controls;

internal class Program
{
    private static void Main(string[] args)
    {
        UserInterface ui = new();

        //ui.AddElement(new Button("button", new(10))
        //{
        //    Name = "btn",
        //    StyleMap = new(true)
        //});

        ui.AddElement(new Toggle(new(0, 0), new(20), false)
        {
            Name = "tgl",
            StyleMap = new(true)
        });

        ui.GetElement("radialSwatch")?.AddChild(new Label("radialSwatch", new(10), 15));
        ui.GetElement("linearSwatch")?.AddChild(new Label("linearSwatch", new(10), 15));
        ui.GetElement("solidSwatch")?.AddChild(new Label("solidSwatch", new(10), 15));

        ui.BootstrapWindow(new(500), "AbysmalCore.UI.Window");

        ui.GetElement("tgl")!.OnClicked += Program_OnClicked;

        ui.BeginDrawingWindowIcon(new(20));
        ui.GetElement("tgl")?.Draw();
        ui.EndDrawingWindowIcon();

        ui.Init();
    }

    private static void Program_OnClicked(UIElement sender, Vector2Int mouse, int frame)
    {
        UserInterface.Instance?.BeginDrawingWindowIcon(new(20));
        sender.Draw();
        UserInterface.Instance?.EndDrawingWindowIcon();
    }
}