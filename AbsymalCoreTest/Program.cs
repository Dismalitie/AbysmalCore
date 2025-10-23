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

        /// compile the assembly and get the test class
        Assembly testAssembly = AbysmalExtensibility.CompileAssembly(File.ReadAllText(".\\ExtensibilityTest.cs"));
        AbysmalExtensibilityClass testClass = AbysmalExtensibility.GetClass(testAssembly, "Tests.ExtensibilityTest", true);

        string output = "";
        /// check if the method exists, then invoke it
        if (testClass.HasMethod("TestWith1Arg"))
            output = testClass.Methods["TestWith1Arg"].Invoke<string>("Hello!");

        AbysmalDebug.Log(testClass.New(), output, true);
    }
}