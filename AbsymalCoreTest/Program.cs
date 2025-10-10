using AbysmalCore.UI;
using AbysmalCore.UI.Controls;
using AbysmalCore.UI.Styling.Brushes;
using AbysmalCore;
using Raylib_cs;
using System.Security.Cryptography;

internal class Program
{
    private static void Main(string[] args)
    {
        UserInterface ui = new();

        //ui.AddElement(new Panel(new(10), new(100))
        //{
        //    Name = "radialSwatch",
        //    StyleMap =
        //    {
        //        Hovered =
        //        {
        //            FillColor = new RadialGradientBrush(1.4f, Color.Blue, Color.Red),
        //            BorderWeight = 0
        //        },
        //        Normal =
        //        {
        //            FillColor = new RadialGradientBrush(1.4f, Color.Red, Color.Blue),
        //            BorderWeight = 0
        //        }
        //    }
        //});

        //ui.AddElement(new Panel(new(120, 10), new(100))
        //{
        //    Name = "linearSwatch",
        //    StyleMap =
        //    {
        //        Hovered =
        //        {
        //            FillColor = new LinearGradientBrush(LinearGradientBrush.GradientDirection.DiagonalRight, Color.Blue, Color.Red),
        //            BorderWeight = 0
        //        },
        //        Normal =
        //        {
        //            FillColor = new LinearGradientBrush(LinearGradientBrush.GradientDirection.DiagonalRight, Color.Red, Color.Blue),
        //            BorderWeight = 0
        //        }
        //    }
        //});

        //ui.AddElement(new Panel(new(230, 10), new(100))
        //{
        //    Name = "solidSwatch",
        //    StyleMap =
        //        {
        //            Hovered =
        //            {
        //                FillColor = new SolidBrush(Color.Red),
        //                BorderWeight = 0
        //            },
        //            Normal =
        //            {
        //                FillColor = new SolidBrush(Color.Blue),
        //                BorderWeight = 0
        //            }
        //        }
        //});

        ui.AddElement(new Button("text", new(10))
        {
            Name = "btn",
            StyleMap =
            {
                ControlStyle = AbysmalCore.UI.Styling.StyleMap.ControlStyleType.Rounded,
                Normal =
                {
                    BorderRadius = 20,
                    FillColor = new SolidBrush(Color.Red)
                }
            }
        });

        ui.GetElement("radialSwatch")?.AddChild(new Label("radialSwatch", new(10), 15));
        ui.GetElement("linearSwatch")?.AddChild(new Label("linearSwatch", new(10), 15));
        ui.GetElement("solidSwatch")?.AddChild(new Label("solidSwatch", new(10), 15));

        ui.BootstrapWindow(new(500), "title");
        ui.Init();
    }
}