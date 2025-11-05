using AbysmalCore.Debugging;

namespace AbysmalCore
{
    /// <summary>
    /// Takes in input string and converts it to an output string
    /// </summary>
    [DebugInfo("transforms strings")]
    public class StringTransformer
    {
        /// <summary>
        /// The current string that this <see cref="StringTransformer"/> is manipulating
        /// </summary>
        protected string InputString = "";
        /// <summary>
        /// Transforms <paramref name="input"/>
        /// </summary>
        /// <param name="input">String to transform</param>

        public string Transform(string input)
        {
            InputString = input;
            TransformInput();
            return InputString;
        }
        /// <summary>
        /// Transforms <paramref name="input"/>
        /// </summary>
        /// <param name="input">String to transform</param>
        public static string Transform<Transformer>(string input) where Transformer : StringTransformer, new() => new Transformer().Transform(input);

        /// <summary>
        /// Transforms <see cref="InputString"/>
        /// </summary>
        protected void TransformInput() => AbysmalDebug.Log(this, "TransformInput was not overidden");

        /// <summary>
        /// Removes whitespace from the start and end of <see cref="InputString"/>
        /// </summary>
        protected void Trim() => InputString.Trim();
        /// <summary>
        /// Makes all characters in <see cref="InputString"/> lowercase
        /// </summary>
        protected void Lower() => InputString.ToLower();
        /// <summary>
        /// Inserts <paramref name="s"/> at <paramref name="idx"/> in <see cref="InputString"/>
        /// </summary>
        /// <param name="idx">Start index</param>
        /// <param name="s">String to insert</param>
        protected void Insert(int idx, string s) => InputString.Insert(idx, s);
        /// <summary>
        /// Pads <see cref="InputString"/> with <paramref name="c"/> from the left until it is
        /// the length of <paramref name="width"/>
        /// </summary>
        /// <param name="width">Total width of the string</param>
        /// <param name="c">Char to pad</param>
        protected void PadLeft(int width, char c) => InputString = InputString.PadLeft(width, c);
        /// <summary>
        /// Pads <see cref="InputString"/> with <paramref name="c"/> from the right until it is
        /// the length of <paramref name="width"/>
        /// </summary>
        /// <param name="width">Total width of the string</param>
        /// <param name="c">Char to pad</param>
        protected void PadRight(int width, char c) => InputString = InputString.PadRight(width, c);
        /// <summary>
        /// Replaces instances of <paramref name="oldS"/> in <see cref="InputString"/> with <paramref name="newS"/>
        /// </summary>
        /// <param name="oldS"></param>
        /// <param name="newS"></param>
        protected void Replace(string oldS, string newS) => InputString = InputString.Replace(oldS, newS);
        /// <summary>
        /// Removes all <paramref name="count"/> chars starting from <paramref name="idx"/>
        /// </summary>
        /// <param name="idx">Start index</param>
        /// <param name="count">Number of chars to remove</param>
        protected void Remove(int idx, int count) => InputString = InputString.Remove(idx, count);
        /// <summary>
        /// Makes all characters in <see cref="InputString"/> lowercase
        /// </summary>
        protected void Upper() => InputString = InputString.ToUpper();
        /// <summary>
        /// Removes whitespace from the end of <see cref="InputString"/>
        /// </summary>
        protected void TrimEnd() => InputString = InputString.TrimEnd();
        /// <summary>
        /// Removes whitespace from the start of <see cref="InputString"/>
        /// </summary>
        protected void TrimStart() => InputString = InputString.TrimStart();
    }
}
