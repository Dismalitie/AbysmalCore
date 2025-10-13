using System.Reflection;

namespace AbysmalCore.Debugging
{
    [DebugInfo("AbysmalCore standard debug lib")]
    public class Debug
    {
        public static bool Enabled = false;
        private static List<string> _logs = new();
        private static List<Trace> _traces = new();

        public static void WriteLog(string path)
        {
            if (_logs.Count == 0) return;

            File.WriteAllText(path, string.Join('\n', _logs));
        }

        public static void ClearLog() => _logs.Clear();

        public static void AddTrace(object @this, object obj, string desc = "No description") => _traces.Add(new(@this, obj, desc));
        public static void GetTrace(object obj) => _traces.First(t => t.Object.Equals(obj));

        private static void write(ConsoleColor c, string msg)
        {
            ConsoleColor cc = Console.ForegroundColor;
            Console.ForegroundColor = c;

            Console.WriteLine(msg);

            Console.ForegroundColor = cc;
        }

        public static void Error(object @this, string msg, bool fatal = false)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            string message = $"[{DateTime.Now.ToString()}][{t.Name}] (!) {msg.Replace("{name}", t.Name)}";
            if (info != null) message = $"[{DateTime.Now.ToString()}][{t.Name}:({info.Name})] (!) {msg.Replace("{name}", info.Name)}";

            write(ConsoleColor.Red, message);
            _logs.Add(message);

            if (fatal) throw new Exception();
        }

        public static void Log(object @this, string msg)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            string message = $"[{DateTime.Now.ToString()}][{t.Name}] (!) {msg.Replace("{name}", t.Name)}";
            if (info != null) message = $"[{DateTime.Now.ToString()}][{t.Name}:({info.Name})] (i) {msg.Replace("{name}", info.Name)}";

            write(ConsoleColor.DarkGray, message);
            _logs.Add(message);
        }

        public static void Warn(object @this, string msg)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            string message = $"[{DateTime.Now.ToString()}][{t.Name}] (!) {msg.Replace("{name}", t.Name)}";
            if (info != null) message = $"[{DateTime.Now.ToString()}][{t.Name}:({info.Name})] /!\\ {msg.Replace("{name}", info.Name)}";

            write(ConsoleColor.Yellow, message);
            _logs.Add(message);
        }

        public static void Pause(bool value = false, bool expected = true, string msg = "unconditional")
        {
            if (!Enabled) return;

            if (value != expected)
            {
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine($"[Debug] Execution paused ({msg})");
                Console.WriteLine($"(press any key to continue)");
                Console.ReadKey();
                Console.WriteLine("[Debug] Execution resumed");

                Console.ForegroundColor = cc;
            }
        }

        public static void Stop(bool value = false, bool expected = true, string msg = "unconditional")
        {
            if (!Enabled) return;

            if (value != expected)
            {
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"[Debug] Execution stopped ({msg})");
                while (true) { }
            }
        }
    }
}
