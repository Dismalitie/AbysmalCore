using AbysmalCore.UI;

namespace AbsymalCoreTest
{
    internal interface ITest
    {
        public static abstract UserInterface GetUserInterface(Window ctx, params string[]? args);
    }
}
