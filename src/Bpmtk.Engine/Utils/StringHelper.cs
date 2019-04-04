using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Utils
{
    static class StringHelper
    {
        public static string Get(string value, int maxLength, string defaultValue = null)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            if (value.Length <= maxLength)
                return value;

            return value.Substring(0, maxLength);
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
