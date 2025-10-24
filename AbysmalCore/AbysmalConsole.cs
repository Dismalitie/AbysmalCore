using AbysmalCore.Debugging;

namespace AbysmalCore
{
    /// <summary>
    /// Standard formatted console input and output handler
    /// </summary>
    [DebugInfo("standard fmtd output and input")]
    public class AbysmalConsole
    {
        private List<string> _output;

        /// <summary>
        /// Creates a new AbysmalConsole instance
        /// </summary>
        public AbysmalConsole() => _output = new();

        /// <summary>
        /// Returns console messages and inputs created and recieved by this instance
        /// </summary>
        public string GetOutput() => string.Join("", _output);
        /// <summary>
        /// Clears the console output buffer and the console screen
        /// </summary>
        public void Clear()
        {
            _output.Clear();
            Console.Clear();
        }
        /// <summary>
        /// Writes the console output buffer to a file
        /// </summary>
        /// <param name="path">Output filepath</param>
        public void WriteOutput(string path) => File.WriteAllText(path, GetOutput());

        /// <summary>
        /// Writes a colored string to the console
        /// </summary>
        /// <param name="s">The text to write</param>
        /// <param name="fore">The foreground color</param>
        /// <param name="back">The background color (optional; uses current color if null)</param>
        public void WriteColor(string s, ConsoleColor fore, ConsoleColor? back = null)
        {
            ConsoleColor oldFore = Console.ForegroundColor;
            ConsoleColor oldBack = Console.BackgroundColor;
            Console.ForegroundColor = fore;
            Console.BackgroundColor = back ?? Console.BackgroundColor;
            Console.Write(s);
            Console.ForegroundColor = oldFore;
            Console.BackgroundColor = oldBack;

            _output.Add(s);
        }

        /// <summary>
        /// Writes multiple colors in the same string to the console
        /// </summary>
        /// <param name="colors">Tuple array of (string, foreground color, background color) (optional; uses current color if null)</param>
        public void WriteColors((string s, ConsoleColor fore, ConsoleColor? back)[] colors)
        {
            foreach (var c in colors)
                WriteColor(c.s, c.fore, c.back);
        }

        /// <summary>
        /// Writes a colored string to the console with a newline
        /// </summary>
        /// <param name="s">The text to write</param>
        /// <param name="fore">The foreground color</param>
        /// <param name="back">The background color (optional; uses current color if null)</param>
        public void WriteColorLn(string s, ConsoleColor fore, ConsoleColor? back = null)
        {
            ConsoleColor oldFore = Console.ForegroundColor;
            ConsoleColor oldBack = Console.BackgroundColor;
            Console.ForegroundColor = fore;
            Console.BackgroundColor = back ?? Console.BackgroundColor;
            Console.WriteLine(s);
            Console.ForegroundColor = oldFore;
            Console.BackgroundColor = oldBack;

            _output.Add(s);
        }

        /// <summary>
        /// Writes multiple single-colored lines to the console
        /// </summary>
        /// <param name="lines">Tuple array of (string, foreground color, background color) (optional; uses current color if null)</param>
        public void WriteColorLns((string s, ConsoleColor fore, ConsoleColor? back)[] lines)
        {
            foreach (var ln in lines)
                WriteColorLn(ln.s, ln.fore, ln.back);
        }

        /// <summary>
        /// Prompts the user with a yes/no question and returns their response
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>null if key is not y/n</returns>
        public bool? Ask(string msg)
        {
            WriteColors([
                (msg + " ", Console.ForegroundColor, null),
                ("(", ConsoleColor.DarkGray, null),
                ("y", ConsoleColor.Green, null),
                ("/", ConsoleColor.DarkGray, null),
                ("n", ConsoleColor.Red, null),
                (")", ConsoleColor.DarkGray, null)
            ]);

            Console.Write("\n> ");
            ConsoleKeyInfo keyInf = Console.ReadKey();

            // normalize it
            char k = keyInf.KeyChar.ToString().ToLower()[0];
            _output.Add($"\n> {k}");

            Console.WriteLine(); // new line after input

            switch (k)
            {
                case 'y':
                    return true;
                case 'n':
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Asks the user for input and converts it to the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The prompt message</param>
        /// <param name="type">The string representation of the type (optional; defaults to typeof(T).Name.ToLower())</param>
        /// <param name="converter">An optional converter function to convert the string input to type T</param>
        public T? Prompt<T>(string msg, string? type = null, Func<string, T>? converter = null)
        {
            type ??= typeof(T).Name.ToLower();

            WriteColors([
                (msg + " ", Console.ForegroundColor, null),
                ("(", ConsoleColor.DarkGray, null),
                (type, ConsoleColor.White, null),
                (")", ConsoleColor.DarkGray, null)
            ]);

            Console.Write("\n> ");
            string response = Console.ReadLine() ?? "";
            _output.Add($"\n> {response}");

            Console.WriteLine(); // new line after input

            if (response == "") return default;

            if (converter != null) return converter(response);
            else return (T)Convert.ChangeType(response, typeof(T));
        }
    }
}
