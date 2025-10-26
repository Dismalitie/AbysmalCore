using AbysmalCore.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.Console
{
    /// <summary>
    /// Parses CLI arguments into flags and parameters
    /// </summary>
    [DebugInfo("standard arg parser")]
    public class ArgumentParser
    {
        private string[] _args;

        /// <summary>
        /// Mandatory index based arguments
        /// </summary>
        public string[] Arguments { get; } = []; 
        /// <summary>
        /// Valueless arguments denoted by a dash
        /// </summary>
        /// <remarks>
        /// Format: <c>-flag</c>
        /// </remarks>
        public string[] Flags { get; } = [];
        /// <summary>
        /// Optional arguments denoted by a flag then a colon and a value
        /// </summary>
        /// <remarks>
        /// Format: <c>-flag:value</c>
        /// </remarks>
        public Dictionary<string, string> OptionalArguments { get; } = new();

        /// <summary>
        /// Creates a new ArgumentParser instance
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="mandatory">How many required arguments are needed at the start</param>
        /// <exception cref="ArgumentException">Throws when required argument count not met</exception>
        public ArgumentParser(string[] args, int mandatory)
        {
            _args = args;

            if (args.Length == 0)
            {
                AbysmalDebug.Warn(this, "Provided args was empty, all values will have a length of 0");
                return;
            }

            Arguments = args.Take(mandatory).Where(s => s[0] != '-').ToArray();
            if (Arguments.Length != mandatory) throw new ArgumentException($"Expected {mandatory} required arguments, got {Arguments.Length}");

            if (mandatory == args.Length) return;

            Flags = args.Skip(mandatory)
                .Where(s => !s.Contains(':')) // if it contains a colon, its an optional arg
                .Where(s => s[0] == '-') // must be denoted with a dash to be a flag
                .Select(s => s.Substring(1)) // skip the first char (the dash)
                .ToArray();

            foreach (string s in args.Skip(mandatory))
            {
                if (s[0] != '-') continue; // not a flag or optional arg
                if (!s.Contains(":")) continue; // not an optional arg

                string flag = s.Split(':')[0].Substring(1);
                string value = flag.Split(':')[1];

                OptionalArguments.Add(flag, value);
            } 
        }

        /// <summary>
        /// Whether flags contains <paramref name="flag"/>
        /// </summary>
        /// <param name="flag">The flag (WITHOUT THE '-' PREFIX)</param>
        public bool HasFlag(string flag) => Flags.Contains(flag);
        /// <summary>
        /// Whether optional arguments contains <paramref name="arg"/>
        /// </summary>
        /// <param name="arg">The optional argument name (WITHOUT THE '-' PREFIX)</param>
        public bool HasOptionalArgument(string arg) => OptionalArguments.Keys.Contains(arg);
        /// <summary>
        /// Gets the value of an optional argument
        /// </summary>
        /// <param name="arg">The name of the argument</param>
        /// <returns>null if not found</returns>
        public string? GetOptionalArgument(string arg)
        {
            if (OptionalArguments.ContainsKey(arg)) return OptionalArguments[arg];
            else return null;
        }

        /// <summary>
        /// Returns the original arguments delimited by a space
        /// </summary>
        public override string ToString() => string.Join(' ', _args);
    }
}
