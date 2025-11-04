using AbsymalCoreTest.Tests;
using AbysmalCore.UI;

internal class Program
{
    static Window w = new(new(500, 500), typeof(Window).FullName!);

    private static void Main(string[] args)
    {
        // uncomment to test NullCheckAnalyzer
        //new NullCheckAnalyzerTest().EmitLogs();
        UserInterface ui = ThemeGenTest.GetUserInterface(w);
        w.Init(ui);
    }
}