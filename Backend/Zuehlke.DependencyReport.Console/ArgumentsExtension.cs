using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Zuehlke.DependencyReport.Console
{
    public static class ArgumentsExtension
    {
        private const string ArgumentIdentifier = "--";
        private const char ArgumentValueDelimiter = '=';

        public static bool IsArgumentSet(this IEnumerable<string> args, string argumentName)
        {
            return args != null && args.Any(a => a.StartsWith(ArgumentIdentifier + argumentName));
        }

        public static string GetArgumentValue(this IEnumerable<string> args, string argumentName)
        {
            return args.GetArgumentValue<string>(argumentName);
        }

        public static T GetArgumentValue<T>(this IEnumerable<string> args, string argumentName, T defaultValue = default(T))
        {
            var arg = args.FirstOrDefault(a => a.StartsWith(ArgumentIdentifier + argumentName + ArgumentValueDelimiter));
            return arg.GetArgumentValue(defaultValue);
        }

        [CanBeNull]
        public static T GetArgumentValue<T>([CanBeNull] this string arg, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(arg) || !arg.Contains(ArgumentValueDelimiter))
            {
                return defaultValue;
            }

            var delimiterPosition = arg.IndexOf(ArgumentValueDelimiter);
            if (delimiterPosition == -1)
            {
                return defaultValue;
            }

            var value = arg.Substring(delimiterPosition + 1).Trim(' ', '\"');
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
