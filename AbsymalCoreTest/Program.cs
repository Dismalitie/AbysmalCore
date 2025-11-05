using AbsymalCoreTest.Tests;
using AbysmalCore.Debugging;
using AbysmalCore.Debugging.Analyzers;
using AbysmalCore.UI;

internal class Program
{
    static Window w = new(new(500, 500), typeof(Window).FullName!);

    private static void Main(string[] args)
    {
        Analyzer<TestClass, NullValueAnalyzer>.AnalyzeMembers();
        Analyzer<TestClass, NullValueAnalyzer>.Emit();

        UserInterface ui = ThemeGenTest.GetUserInterface(w);
        w.Init(ui);
    }
}