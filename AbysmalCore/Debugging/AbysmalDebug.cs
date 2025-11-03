using AbysmalCore.Console;
using System.Reflection;
using System.Xml.Linq;

namespace AbysmalCore.Debugging
{
    /// <summary>
    /// Standard debugging class for logging messages, warnings and errors to the console
    /// </summary>
    [DebugInfo("standard debug lib", true)]
    public class AbysmalDebug
    {
        /// <summary>
        /// Determines whether to write logs to the console
        /// </summary>
        public static bool Enabled = true;
        private static AbysmalConsole _c = new();

        /// <summary>
        /// Gets the underlying <see cref="AbysmalConsole"/> instance
        /// </summary>
        public static AbysmalConsole Console { get =>  _c; }

        /// <summary>
        /// Writes the current logs to a file
        /// </summary>
        /// <param name="path">Path to write logs to</param>
        public static void WriteLogs(string path)
        {
            Log(new AbysmalDebug(), $"Writing logs to {path}");

            File.WriteAllText(path, _c.GetOutput());
        }

        /// <summary>
        /// Sets an optional sub-category source when logging
        /// </summary>
        public static void SetLogRegion(string name) => _c.WriteColorLn($"<--#-- Log region start, subsource: {name}", ConsoleColor.Green);
        /// <summary>
        /// Removes the sub-category
        /// </summary>
        public static void UnsetLogRegion() => _c.WriteColorLn($"<--x-- Log region end", ConsoleColor.Green);

        /// <summary>
        /// Logs an error message to the console
        /// </summary>
        /// <param name="this">Instance of the calling class</param>
        /// <param name="msg">Message to log</param>
        /// <param name="fatal">Determines whether to throw an exception after logging</param>
        /// <param name="isType">Whether <paramref name="this"/> is a <see cref="Type"/></param>
        public static void Error(object @this, string msg, bool fatal = false, bool isType = false)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            if (isType) t = (Type)@this;
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            string message = $"[{DateTime.Now.ToString()}][{t.Name}] (!) {msg.Replace("{name}", t.Name)}";
            if (info != null) message = $"[{DateTime.Now.ToString()}][{t.Name}:({info.Description})] (!) {msg.Replace("{name}", info.Description)}";

            _c.WriteColorLn(message, ConsoleColor.Red);

            if (fatal) throw new Exception(message);
        }

        /// <summary>
        /// Logs a standard message to the console
        /// </summary>
        /// <param name="this">Instance of the calling class</param>
        /// <param name="msg">Message to log</param>
        /// <param name="important">Whether to highlight this message in the output</param>
        /// <param name="isType">Whether <paramref name="this"/> is a <see cref="Type"/></param>
        public static void Log(object @this, string msg, bool important = false, bool isType = false)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            if (isType) t = (Type)@this;
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            ConsoleColor cc = ConsoleColor.DarkGray;
            string message = $"[{DateTime.Now.ToString()}][{t.Name}] (!) {msg.Replace("{name}", t.Name)}";
            if (important) cc = ConsoleColor.Magenta;
            if (info != null)
            {
                message = $"[{DateTime.Now.ToString()}][{t.Name}:({info.Description})] (i) {msg.Replace("{name}", info.Description)}";
                if (info.Important) important = true;
            }

            _c.WriteColorLn(message, cc);

            if (DebugInfoAttribute.ImportanceAction == DebugInfoAttribute.ImportanceActionType.Pause && important)
                Pause(false, true, "paused by important log");
        }

        /// <summary>
        /// Logs a standard warning to the console
        /// </summary>
        /// <param name="this">Instance of the calling class</param>
        /// <param name="msg">Message to log</param>
        /// <param name="isType">Whether <paramref name="this"/> is a <see cref="Type"/></param>
        public static void Warn(object @this, string msg, bool isType = false)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            if (isType) t = (Type)@this;
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            string message = $"[{DateTime.Now.ToString()}][{t.Name}] (!) {msg.Replace("{name}", t.Name)}";
            if (info != null) message = $"[{DateTime.Now.ToString()}][{t.Name}:({info.Description})] /!\\ {msg.Replace("{name}", info.Description)}";

            _c.WriteColorLn(message, ConsoleColor.Yellow);
        }

        /// <summary>
        /// Pauses execution until a key is pressed if <paramref name="value"/> does not equal <paramref name="expected"/>
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <param name="expected">The expected value</param>
        /// <param name="reason">The pause reason</param>
        public static void Pause(bool value = false, bool expected = true, string reason = "unconditional")
        {
            if (!Enabled) return;

            if (value != expected)
            {
                _c.WriteColorLns([
                    ($"[Debug] Execution paused ({reason})", ConsoleColor.Yellow, null),
                    ("(press any key to continue)", ConsoleColor.Yellow, null)
                ]);

                System.Console.ReadKey();
                _c.WriteColorLn("[Debug] Execution resumed", ConsoleColor.Yellow);
            }
        }

        /// <summary>
        /// Pauses execution until a key is pressed if <paramref name="value"/> does not equal <paramref name="expected"/>
        /// </summary>
        /// <param name="value">The object to check</param>
        /// <param name="expected">The expected object</param>
        /// <param name="reason">The pause reason</param>
        /// <remarks>
        /// Uses the default <see cref="object.Equals(object?, object?)"/> equality comparison
        /// </remarks>
        public static void Pause(object? value, object? expected, string reason = "unconditional")
        {
            if (!Enabled) return;
            if (!Equals(value, expected)) Pause(reason: reason);
        }

        /// <summary>
        /// Pauses execution until a key is pressed if <paramref name="value"/> does not equal <paramref name="expected"/>
        /// </summary>
        /// <param name="value">The object to check</param>
        /// <param name="expected">The expected object</param>
        /// <param name="comparer">A custom lambda function that takes both objects</param>
        /// <param name="reason">The pause reason</param>
        /// <remarks>
        /// Uses the default <see cref="object.Equals(object?, object?)"/> equality comparison
        /// </remarks>
        public static void Pause(object? value, object? expected, Func<object?, object?, bool> comparer, string reason = "unconditional")
        {
            if (!Enabled) return;
            if (comparer(value, expected) == false) Pause(reason: reason);
        }

        /// <summary>
        /// Stops execution indefinitely if <paramref name="value"/> does not equal <paramref name="expected"/>
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <param name="expected">The expected value</param>
        /// <param name="reason">The reason for stopping</param>
        public static void Stop(bool value = false, bool expected = true, string reason = "unconditional")
        {
            if (!Enabled) return;

            if (value != expected)
            {
                _c.WriteColorLn($"[Debug] Execution stopped ({reason})", ConsoleColor.Red);
                while (true) { }
            }
        }

        /// <summary>
        /// Stops execution indefinitely if <paramref name="value"/> does not equal <paramref name="expected"/>
        /// </summary>
        /// <param name="value">The object to check</param>
        /// <param name="expected">The expected object</param>
        /// <param name="reason">The reason for stopping</param>
        /// <remarks>
        /// Uses the default <see cref="object.Equals(object?, object?)"/> equality comparison
        /// </remarks>
        public static void Stop(object? value, object? expected, string reason = "unconditional")
        {
            if (!Enabled) return;
            if (!Equals(value, expected)) Stop(reason: reason);
        }

        /// <summary>
        /// Stops execution indefinitely if <paramref name="value"/> does not equal <paramref name="expected"/> using a custom comparer
        /// </summary>
        /// <param name="value">The object to check</param>
        /// <param name="expected">The expected object</param>
        /// <param name="comparer">A custom lambda function that takes both objects</param>
        /// <param name="reason">The reason for stopping</param>
        /// <remarks>
        /// Evaluates equality based on <paramref name="comparer"/>'s return
        /// </remarks>
        public static void Stop(object? value, object? expected, Func<object?, object?, bool> comparer, string reason = "unconditional")
        {
            if (!Enabled) return;
            if (comparer(value, expected) == false) Stop(reason: reason);
        }
    }
}
