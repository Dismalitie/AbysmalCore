using System.Reflection;

namespace AbysmalCore.Debugging
{
    /// <summary>
    /// Standard debugging class for logging messages, warnings and errors to the console
    /// </summary>
    [DebugInfo("standard debug lib", true)]
    public class Debug
    {
        public static bool Enabled = false;
        private static List<string> _logs = new();

        /// <summary>
        /// Writes the current logs to a file
        /// </summary>
        /// <param name="path"></param>Path to write logs to
        public static void WriteLogs(string path)
        {
            Log(new Debug(), $"Writing logs to {path}");
            if (_logs.Count == 0) return;

            File.WriteAllText(path, string.Join('\n', _logs));
        }

        /// <summary>
        /// Clears the current log buffer
        /// </summary>
        public static void ClearLog() => _logs.Clear();

        private static void write(ConsoleColor c, string msg)
        {
            ConsoleColor cc = Console.ForegroundColor;
            Console.ForegroundColor = c;

            Console.WriteLine(msg);

            Console.ForegroundColor = cc;
        }

        /// <summary>
        /// Logs an error message to the console
        /// </summary>
        /// <param name="this"></param>Instance of the calling class
        /// <param name="msg"></param>Message to log
        /// <param name="fatal"></param>Determines whether to throw an exception after logging
        public static void Error(object @this, string msg, bool fatal = false)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            string message = $"[{DateTime.Now.ToString()}][{t.Name}] (!) {msg.Replace("{name}", t.Name)}";
            if (info != null) message = $"[{DateTime.Now.ToString()}][{t.Name}:({info.Description})] (!) {msg.Replace("{name}", info.Description)}";

            write(ConsoleColor.Red, message);
            _logs.Add(message);

            if (fatal) throw new Exception();
        }

        /// <summary>
        /// Logs a standard message to the console
        /// </summary>
        /// <param name="this"></param>Instance of the calling class
        /// <param name="msg"></param>Message to log
        public static void Log(object @this, string msg)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            ConsoleColor cc = ConsoleColor.DarkGray;
            string message = $"[{DateTime.Now.ToString()}][{t.Name}] (!) {msg.Replace("{name}", t.Name)}";
            bool important = false;
            if (info != null)
            {
                message = $"[{DateTime.Now.ToString()}][{t.Name}:({info.Description})] (i) {msg.Replace("{name}", info.Description)}";
                if (info.Important)
                {
                    important = true;
                    cc = ConsoleColor.Magenta;
                }
            }

            write(cc, message);
            _logs.Add(message);

            if (DebugInfoAttribute.ImportanceAction == DebugInfoAttribute.ImportanceActionType.Pause && important)
                Pause(false, true, "paused by important log");
        }

        /// <summary>
        /// Logs a standard warning to the console
        /// </summary>
        /// <param name="this"></param>Instance of the calling class
        /// <param name="msg"></param>Message to log
        public static void Warn(object @this, string msg)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            string message = $"[{DateTime.Now.ToString()}][{t.Name}] (!) {msg.Replace("{name}", t.Name)}";
            if (info != null) message = $"[{DateTime.Now.ToString()}][{t.Name}:({info.Description})] /!\\ {msg.Replace("{name}", info.Description)}";

            write(ConsoleColor.Yellow, message);
            _logs.Add(message);
        }

        /// <summary>
        /// Pauses execution until a key is pressed if <paramref name="value"/> does not equal <paramref name="expected"/>
        /// </summary>
        /// <param name="value"></param>The value to check
        /// <param name="expected"></param>The expected value
        /// <param name="reason"></param>The pause reason
        public static void Pause(bool value = false, bool expected = true, string reason = "unconditional")
        {
            if (!Enabled) return;

            if (value != expected)
            {
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine($"[Debug] Execution paused ({reason})");
                Console.WriteLine($"(press any key to continue)");
                Console.ReadKey();
                Console.WriteLine("[Debug] Execution resumed");

                Console.ForegroundColor = cc;
            }
        }

        /// <summary>
        /// Stops execution indefinitely if <paramref name="value"/> does not equal <paramref name="expected"/>
        /// </summary>
        /// <param name="value"></param>The value to check
        /// <param name="expected"></param>The expected value
        /// <param name="reason"></param>The reason for stopping
        public static void Stop(bool value = false, bool expected = true, string reason = "unconditional")
        {
            if (!Enabled) return;

            if (value != expected)
            {
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"[Debug] Execution stopped ({reason})");
                while (true) { }
            }
        }
    }
}
