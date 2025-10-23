using AbysmalCore.Debugging;
using AbysmalCore.Extensibility;
using AbysmalCore.UI;
using System.Reflection;

internal class Program
{
    static Window w = new(new(500, 500), typeof(Window).FullName!);

    private static void Main(string[] args)
    {
        //UserInterface ui = ThemeGenTest.GetUserInterface(w);
        //w.Init(ui);

        Assembly testAssembly = AbysmalExtensibility.CompileAssembly(File.ReadAllText(".\\ExtensibilityTest.cs"));
        ExtensibilityClass testClass = AbysmalExtensibility.GetClass(testAssembly, "Tests.ExtensibilityTest", true);

        string outp = "";
        if (testClass.HasMethod("TestWith1Arg"))
            outp = testClass.Methods["TestWith1Arg"].Invoke<string>("Hello!");

        AbysmalDebug.Log(new Program(), outp, true);
    }
}