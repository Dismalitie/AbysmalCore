using AbysmalCore.Debugging;
using AbysmalCore.Extensibility;
using AbysmalCore.UI;
using AbysmalCore.UI.Controls;
using System.Reflection;

namespace AbsymalCoreTest.Tests
{
    internal class ExtensibilityTest : ITest
    {
        public static UserInterface GetUserInterface(Window ctx, Dictionary<string, object>? args)
        {
            args ??= new()
            {
                ["class"] = "Tests.ExtensibilityTest",
                ["method"] = "TestWith1Arg",
                ["input"] = "Hello!",
            };

            string className = (string)args["class"];
            string methodName = (string)args["method"];
            string input = (string)args["input"];

            string code = @"
namespace Tests
{
    public class ExtensibilityTest
    {
        public static string TestWith1Arg(string arg1)
        {
            return arg1.ToUpper();
        }

        public static string TestWith2Args(string arg1, string arg2)
        {
            return arg1.ToUpper() + "" "" + arg2.ToUpper();
        }
    }
}";

            UserInterface ui = new();

            // ----- test bit -----
            Assembly testAssembly = ExtensibilityHelper.CompileAssembly(code);
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
