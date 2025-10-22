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
        private static AbysmalConsole _c = new();

        /// <summary>
        /// Writes the current logs to a file
        /// </summary>
        /// <param name="path"></param>Path to write logs to
        public static void WriteLogs(string path)
        {
            Log(new Debug(), $"Writing logs to {path}");

            File.WriteAllText(path, _c.GetOutput());
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

            _c.WriteColorLn(message, ConsoleColor.Red);

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

            _c.WriteColorLn(message, cc);

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

            _c.WriteColorLn(message, ConsoleColor.Yellow);
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
                _c.WriteColorLns([
                    ($"[Debug] Execution paused ({reason})", ConsoleColor.Yellow, null),
                    ("(press any key to continue)", ConsoleColor.Yellow, null)
                ]);

                Console.ReadKey();
                _c.WriteColorLn("[Debug] Execution resumed", ConsoleColor.Yellow);
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
                _c.WriteColorLn($"[Debug] Execution stopped ({reason})", ConsoleColor.Red);
                while (true) { }
            }
        }
    }
}
