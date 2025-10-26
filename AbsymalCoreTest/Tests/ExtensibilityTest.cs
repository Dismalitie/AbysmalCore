using AbysmalCore.Debugging;
using AbysmalCore.Extensibility;
using AbysmalCore.UI;
using AbysmalCore.UI.Controls;
using System.Reflection;

namespace AbsymalCoreTest.Tests
{
    internal class ExtensibilityTest : ITest
    {
        public static UserInterface GetUserInterface(Window ctx, params string[]? args)
        {
            // for some reason args is never null even though its nullable?
            if (args!.Length == 0) args = ["Tests.ExtensibilityTest", "TestWith1Arg", "Hello!"];
            string className = args[0];
            string methodName = args[1];
            string input = args[2];


            UserInterface ui = new();

            // ----- test bit -----
            Assembly testAssembly = ExtensibilityHelper.CompileAssembly(File.ReadAllText(".\\ExtensibilityTest.cs"));
            var asm = new UniformAssembly(testAssembly, true);

            if (asm.HasClass(className))
            {
                var cls = asm.GetClass(className)!;
                string? output = null;

                if (cls.HasMethod(methodName))
                    output = cls.GetMethod(methodName)!.Invoke<string>(input);

                AbysmalDebug.Log(cls, output ?? "error!", true);
            }

            // ----- ui bit -----
            ui.AddElement(new Label("check console!", new(10, 10))
            {
                StyleMap = new(true)
            });
            return ui;
        }
    }
}
