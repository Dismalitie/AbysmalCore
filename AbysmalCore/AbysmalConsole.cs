namespace AbysmalCore
{
    public class AbysmalConsole
    {
        private List<string> _output;

        public AbysmalConsole()
        {
            _output = new();
        }

        /// <summary>
        /// Writes a colored string to the console
        /// </summary>
        /// <param name="s"></param>The text to write
        /// <param name="fore"></param>The foreground color
        /// <param name="back"></param>The background color (optional; uses current color if null)
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
        /// <param name="colors"></param>Tuple array of - string, foreground color, background color (optional; uses current color if null)
        public void WriteColors((string s, ConsoleColor fore, ConsoleColor? back)[] colors)
        {
            foreach (var c in colors)
                WriteColor(c.s, c.fore, c.back);
        }

        /// <summary>
        /// Writes a colored string to the console with a newline
        /// </summary>
        /// <param name="s"></param>
        /// <param name="fore"></param>
        /// <param name="back"></param>
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
        /// <param name="lines"></param>Tuple array of - string, foreground color, background color (optional; uses current color if null)
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

            /// normalize it
            char k = keyInf.KeyChar.ToString().ToLower()[0];
            _output.Add($"\n> {k}");

            Console.WriteLine(); /// new line after input

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
    }
}
