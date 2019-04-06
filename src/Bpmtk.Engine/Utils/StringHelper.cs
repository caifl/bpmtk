using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Bpmtk.Engine.Utils
{
    static class StringHelper
    {
        private const string JuelSearchPattern = "(?:\\${)(.*?)(?:})";

        private readonly static Regex juelRegex = new Regex(JuelSearchPattern, RegexOptions.IgnoreCase);

        public static string Get(string value, int maxLength, string defaultValue = null)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            if (value.Length <= maxLength)
                return value;

            return value.Substring(0, maxLength);
        }

        public static string Replace(string text)
        {
            return juelRegex.Replace(text, new MatchEvaluator((m) =>
            {
                var len = m.Length;
                var a = m.Value;
                var expression = a.Substring(2).TrimEnd('}').Trim();

                //var value = this.engine.Execute(expression, scope);
                //if (value != null)
                //    return value.ToString();

                return string.Empty;
            }));
        }

        public static string ExtractExpression(string text)
        {
            var match = juelRegex.Match(text);
            if(match.Success)
            {
                return match.Value.Substring(2).TrimEnd('}').Trim();

            }

            return text;
        }

        public static bool HasExpressions(string text)
        {
            return juelRegex.IsMatch(text);
        }

        public static string Join(string[] array, string sperator, int maxlength)
        {
            if (array == null)
                return null;

            if (array.Length == 0)
                return null;

            var text = string.Join(sperator, array);
            return Get(text, maxlength);
        }
    }
}
