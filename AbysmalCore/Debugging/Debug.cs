using System.Reflection;

namespace AbysmalCore.Debugging
{
    [DebugInfo("AbysmalCore standard debug lib")]
    public class Debug
    {
        public static bool Enabled = false;

        public static void Error(object @this, string msg, bool fatal = false)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            ConsoleColor cc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            if (info != null) Console.WriteLine($"[{t.Name}:({info.Name})] (!) {msg.Replace("{name}", info.Name)}");
            else Console.WriteLine($"[{t.Name}] (!) {msg.Replace("{name}", t.Name)}");

            Console.ForegroundColor = cc;

            if (fatal) throw new Exception();
        }

        public static void Log(object @this, string msg)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            ConsoleColor cc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (info != null) Console.WriteLine($"[{t.Name}:({info.Name})] (i) {msg.Replace("{name}", info.Name)}");
            else Console.WriteLine($"[{t.Name}] (i) {msg.Replace("{name}", t.Name)}");

            Console.ForegroundColor = cc;
        }

        public static void Warn(object @this, string msg)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            ConsoleColor cc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            if (info != null) Console.WriteLine($"[{t.Name}:({info.Name})] /!\\ {msg.Replace("{name}", info.Name)}");
            else Console.WriteLine($"[{t.Name}] /!\\ {msg.Replace("{name}", t.Name)}");

            Console.ForegroundColor = cc;
        }

        public static void Success(object @this, string msg)
        {
            if (!Enabled) return;

            Type t = @this.GetType();
            DebugInfoAttribute? info = t.GetCustomAttribute<DebugInfoAttribute>();

            ConsoleColor cc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            if (info != null) Console.WriteLine($"[{t.Name}:({info.Name})] (+) {msg.Replace("{name}", info.Name)}");
            else Console.WriteLine($"[{t.Name}] (+) {msg.Replace("{name}", t.Name)}");

            Console.ForegroundColor = cc;
        }

        public static void Pause(bool value = false, bool expected = true, string msg = "manually invoked")
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

        public static void Stop(bool value = false, bool expected = true, string msg = "manually invoked")
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
