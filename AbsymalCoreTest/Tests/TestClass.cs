#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace AbsymalCoreTest.Tests
{
    internal class TestClass
    {
        public static object? StaticNullableReturn() => null;
        public object? NullableReturn() => null;
        public static object StaticNonNullableReturn() => new();
        public object NonNullableReturn() => new();

        public object? NullableProperty => null;
        public static object? StaticNullableProperty => null;

        public object NonNullableProperty => null;
        public static object StaticNonNullableProperty => null;

        public object? NullableField = null;
        public static object? StaticNullableField = null;
        public object NonNullableField = null;

        public static object StaticNonNullableField = null;
    }
}
