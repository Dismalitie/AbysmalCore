using AbysmalCore;
using AbysmalCore.Debugging;
using AbysmalCore.UI;
using AbysmalCore.UI.Controls;
using AbysmalCore.UI.Styling.Brushes;
using Raylib_cs;

internal class Program
{
    private static void Main(string[] args)
    {
        UserInterface ui = new();
        Debug.Enabled = true;

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

        ui.AddElement(new Panel(new(100), new(100), null)
        {
            Name = "radialSwatch",
            StyleMap = new(false)
            {
                Normal = new()
                {
                    FillColor = new RadialGradientBrush(3, Color.Red, Color.Blue),
                }
            }
        });

        ui.BootstrapWindow(new(500), "AbysmalCore.UI.Window");

        ui.GetElement("tgl")!.OnStateChanged += Program_OnStateChanged;

        ui.BeginDrawingWindowIcon(new(20));
        ui.GetElement("tgl")?.Draw();
        ui.EndDrawingWindowIcon();

        ui.Init();
    }

    private static void Program_OnStateChanged(UIElement sender, string state, object newState)
    {
        UserInterface.Instance?.BeginDrawingWindowIcon(new(20));
        sender.Draw();
        UserInterface.Instance?.EndDrawingWindowIcon();
    }
}