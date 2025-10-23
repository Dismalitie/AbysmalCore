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
            return arg1.ToUpper() + " " + arg2.ToUpper();
        }
    }
}