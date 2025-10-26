using AbysmalCore.Debugging;

namespace AbysmalCore.Console
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
            System.Console.Clear();
        }
        /// <summary>
        /// Writes the console output buffer to a file
        /// </summary>
        /// <param name="path">Output filepath</param>
        public void WriteOutput(string path) => File.WriteAllText(path, GetOutput());

        /// <summary>
        /// Writes an unstyled string to the console
        /// </summary>
        /// <param name="s">String to write</param>
        public void WriteLn(string s)
        {
            _output.Add(s);
            System.Console.WriteLine(s);
        }
        /// <summary>
        /// Writes an empty line
        /// </summary>
        public void WriteLn()
        {
            _output.Add("\n");
            System.Console.WriteLine();
        }

        /// <summary>
        /// Writes string to the console without an ending newline
        /// </summary>
        /// <param name="s">String to write</param>
        public void Write(string s)
        {
            _output.Add(s);
            System.Console.Write(s);
        } 

        /// <summary>
        /// Writes a colored string to the console
        /// </summary>
        /// <param name="s">The text to write</param>
        /// <param name="fore">The foreground color</param>
        /// <param name="back">The background color (optional; uses current color if null)</param>
        public void WriteColor(string s, ConsoleColor fore, ConsoleColor? back = null)
        {
            ConsoleColor oldFore = System.Console.ForegroundColor;
            ConsoleColor oldBack = System.Console.BackgroundColor;
            System.Console.ForegroundColor = fore;
            System.Console.BackgroundColor = back ?? System.Console.BackgroundColor;
            System.Console.Write(s);
            System.Console.ForegroundColor = oldFore;
            System.Console.BackgroundColor = oldBack;

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
            ConsoleColor oldFore = System.Console.ForegroundColor;
            ConsoleColor oldBack = System.Console.BackgroundColor;
            System.Console.ForegroundColor = fore;
            System.Console.BackgroundColor = back ?? System.Console.BackgroundColor;
            System.Console.WriteLine(s);
            System.Console.ForegroundColor = oldFore;
            System.Console.BackgroundColor = oldBack;

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
                (msg + " ", System.Console.ForegroundColor, null),
                ("(", ConsoleColor.DarkGray, null),
                ("y", ConsoleColor.Green, null),
                ("/", ConsoleColor.DarkGray, null),
                ("n", ConsoleColor.Red, null),
                (")", ConsoleColor.DarkGray, null)
            ]);

            System.Console.Write("\n> ");
            ConsoleKeyInfo keyInf = System.Console.ReadKey();

            // normalize it
            char k = keyInf.KeyChar.ToString().ToLower()[0];
            _output.Add($"\n> {k}");

            System.Console.WriteLine(); // new line after input

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
                (msg + " ", System.Console.ForegroundColor, null),
                ("(", ConsoleColor.DarkGray, null),
                (type, ConsoleColor.White, null),
                (")", ConsoleColor.DarkGray, null)
            ]);

            System.Console.Write("\n> ");
            string response = System.Console.ReadLine() ?? "";
            _output.Add($"\n> {response}");

            System.Console.WriteLine(); // new line after input

            if (response == "") return default;

            if (converter != null) return converter(response);
            else return (T)Convert.ChangeType(response, typeof(T));
        }
    }
}
