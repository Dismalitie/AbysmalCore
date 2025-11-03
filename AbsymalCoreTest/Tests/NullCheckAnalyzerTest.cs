using AbysmalCore.Debugging.Analyzers;

namespace AbsymalCoreTest.Tests
{
    internal class NullCheckAnalyzerTest : NullCheckAnalyzer<NullCheckAnalyzerTest>
    {
        public static object? StaticNullableReturn() => null;
        public object? NullableReturn() => null;
        public static object StaticNonNullableReturn() => new();
        public object NonNullableReturn() => new();
    }
}
