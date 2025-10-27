using AbysmalCore.Debugging;

namespace AbysmalCore.Console
{
    /// <summary>
    /// Standard formatted console input and output handler
    /// </summary>
    [DebugInfo("standard fmtd output and input")]
    public class AbysmalConsole : IDisposable
    {
        private bool _disposed = false;
        private Stream stdo;

        /// <summary>
        /// Creates a new AbysmalConsole instance
        /// </summary>
        public AbysmalConsole()
        {
            stdo = System.Console.OpenStandardOutput();
        }

        /// <summary>
        /// Returns content read from the console's standard output
        /// </summary>
        public string GetOutput()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);

            StreamReader sr = new(stdo);
            string s = sr.ReadToEnd();
            sr.Dispose();
            return s;
        }
        /// <summary>
        /// Clears the console output buffer and the console screen
        /// </summary>
        public void Clear()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
            System.Console.Clear();
        }
        /// <summary>
        /// Writes the console output buffer to a file
        /// </summary>
        /// <param name="path">Output filepath</param>
        public void WriteOutput(string path)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
            File.WriteAllText(path, GetOutput());
        }

        /// <summary>
        /// Writes an unstyled string to the console ending with a new line
        /// </summary>
        /// <param name="s">String to write</param>
        public void WriteLn(string s)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
            System.Console.WriteLine(s);
        }
        /// <summary>
        /// Writes an empty line
        /// </summary>
        public void WriteLn()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
            System.Console.WriteLine();
        }
        /// <summary>
        /// Writes string to the console without an ending newline
        /// </summary>
        /// <param name="s">String to write</param>
        public void Write(string s)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
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
            if (_disposed) throw new ObjectDisposedException(GetType().Name);

            ConsoleColor oldFore = System.Console.ForegroundColor;
            ConsoleColor oldBack = System.Console.BackgroundColor;
            System.Console.ForegroundColor = fore;
            System.Console.BackgroundColor = back ?? System.Console.BackgroundColor;
            System.Console.Write(s);
            System.Console.ForegroundColor = oldFore;
            System.Console.BackgroundColor = oldBack;
        }

        /// <summary>
        /// Writes multiple colors in the same string to the console
        /// </summary>
        /// <param name="colors">Tuple array of (string, foreground color, background color) (optional; uses current color if null)</param>
        public void WriteColors((string s, ConsoleColor fore, ConsoleColor? back)[] colors)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);

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
            if (_disposed) throw new ObjectDisposedException(GetType().Name);

            ConsoleColor oldFore = System.Console.ForegroundColor;
            ConsoleColor oldBack = System.Console.BackgroundColor;
            System.Console.ForegroundColor = fore;
            System.Console.BackgroundColor = back ?? System.Console.BackgroundColor;
            System.Console.WriteLine(s);
            System.Console.ForegroundColor = oldFore;
            System.Console.BackgroundColor = oldBack;
        }

        /// <summary>
        /// Writes multiple single-colored lines to the console
        /// </summary>
        /// <param name="lines">Tuple array of (string, foreground color, background color) (optional; uses current color if null)</param>
        public void WriteColorLns((string s, ConsoleColor fore, ConsoleColor? back)[] lines)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);

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
            if (_disposed) throw new ObjectDisposedException(GetType().Name);

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
            if (_disposed) throw new ObjectDisposedException(GetType().Name);

            type ??= typeof(T).Name.ToLower();

            WriteColors([
                (msg + " ", System.Console.ForegroundColor, null),
                ("(", ConsoleColor.DarkGray, null),
                (type, ConsoleColor.White, null),
                (")", ConsoleColor.DarkGray, null)
            ]);

            System.Console.Write("\n> ");
            string response = System.Console.ReadLine() ?? "";

            System.Console.WriteLine(); // new line after input

            if (response == "") return default;

            if (converter != null) return converter(response);
            else return (T)Convert.ChangeType(response, typeof(T));
        }

        /// <summary>
        /// Releases the console's standard output stream and makes this instance unusable
        /// </summary>
        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);

            _disposed = true;
            stdo.Dispose();
        }
    }
}
