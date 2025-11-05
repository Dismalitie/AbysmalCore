using AbsymalCoreTest.Tests;
using AbysmalCore.Debugging;
using AbysmalCore.Debugging.Analyzers;
using AbysmalCore.UI;

internal class Program
{
    static Window w = new(new(500, 500), typeof(Window).FullName!);

    private static void Main(string[] args)
     {
        // uncomment to test NullCheckAnalyzer
        //new NullCheckAnalyzerTest().EmitLogs();
        Analyzer<TestClass, MemberOutputAnalyzer>.AnalyzeMembers();
        Analyzer<TestClass, MemberOutputAnalyzer>.Emit();
        UserInterface ui = ThemeGenTest.GetUserInterface(w);
        w.Init(ui);
    }
}