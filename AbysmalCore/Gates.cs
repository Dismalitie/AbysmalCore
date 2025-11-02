namespace AbysmalCore
{
    /// <summary>
    /// The 7 standard logic gates for convenience
    /// </summary>
    public class Gates
    {
        private static void check(bool[] inputs)
        {
            if (inputs.Length < 2)
                throw new ArgumentException("Requires at least 2 inputs");
        }

        /// <summary>
        /// Standard AND logic gate
        /// </summary>
        /// <returns>true if all <paramref name="inputs"/> are true, else false</returns>
        public static bool AND(params bool[] inputs)
        {
            check(inputs);
            if (inputs.Contains(false)) return false;
            else return true;
        }

        /// <summary>
        /// Standard OR logic gate
        /// </summary>
        /// <returns>true if <paramref name="inputs"/> contains at least one true value, else false</returns>
        public static bool OR(params bool[] inputs)
        {
            check(inputs);
            if (inputs.Contains(true)) return true;
            else return false;
        }

        /// <summary>
        /// Standard NOT gate
        /// </summary>
        /// <returns>Inverts <paramref name="input"/></returns>
        /// <remarks>
        /// Equivalent to <c>!input</c>
        /// </remarks>
        public static bool NOT(bool input) => !input;
        /// <summary>
        /// Standard NAND (Not AND) logic gate
        /// </summary>
        /// <returns>Inverse of AND; false if all <paramref name="inputs"/> are true, else true</returns>
        /// <remarks>
        /// Equivalent to <c>NOT(AND(inputs))</c>
        /// </remarks>
        public static bool NAND(params bool[] inputs) => NOT(AND(inputs));
        /// <summary>
        /// Standard NOR (Not OR) logic gate
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns>Inverse of OR; false if <paramref name="inputs"/> contains at least one true value, else true</returns>
        /// <remarks>
        /// Equivalent to <c>NOT(OR(inputs)</c>
        /// </remarks>
        public static bool NOR(params bool[] inputs) => NOT(OR(inputs));
        /// <summary>
        /// Standard XOR (eXclusive OR)
        /// </summary>
        /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>, else false</returns>
        /// <remarks>
        /// Equivalent to <c>a != b</c>
        /// </remarks>
        public static bool XOR(bool a, bool b) => a != b;
        /// <summary>
        /// Standard XNOR (eXclusive Not OR)
        /// </summary>
        /// <returns>false if <paramref name="a"/> does not equal <paramref name="b"/>, else true</returns>
        /// <remarks>
        /// Equivalent to <c>NOT(XOR(a, b))</c>
        /// </remarks>
        public static bool XNOR(bool a, bool b) => NOT(XOR(a, b));
    }
}
