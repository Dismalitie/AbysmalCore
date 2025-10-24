using AbysmalCore.Debugging;
using AbysmalCore.Extensibility;
using AbysmalCore.UI;
using System.Reflection;
using System.Security.Claims;

internal class Program
{
    static Window w = new(new(500, 500), typeof(Window).FullName!);

    private static void Main(string[] args)
    {
        //UserInterface ui = ThemeGenTest.GetUserInterface(w);
        //w.Init(ui);

        // compile the assembly and get the test class
        Assembly testAssembly = ExtensibilityHelper.CompileAssembly(File.ReadAllText(".\\ExtensibilityTest.cs"));
        var asm = new UniformAssembly(testAssembly, true);

        if (asm.HasClass("Tests.ExtensibilityTest"))
        {
            var cls = asm.GetClass("Tests.ExtensibilityTest")!;
            string? output = null;

            if (cls.HasMethod("TestWith1Arg")) 
                output = cls.GetMethod("TestWith1Arg")!.Invoke<string>("Hello!");

            AbysmalDebug.Log(cls, output ?? "error!", true);
        }
    }
}