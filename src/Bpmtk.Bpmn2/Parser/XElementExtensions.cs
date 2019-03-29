using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    public static class XElementExtensions
    {
        public static readonly string BPMTK_NS = "http://www.bpmtk.com/bpmn/extensions";

        public static readonly string BPMN20_NS = "http://www.omg.org/spec/BPMN/20100524/MODEL";

        public static string GetAttribute(this XElement element, string localName)
        {
            var name = XName.Get(localName);
            return element.Attribute(name)?.Value;
        }

        public static bool GetBoolean(this XElement element, string localName, bool defaultValue = false)
        {
            var name = XName.Get(localName);
            var value = element.Attribute(name)?.Value;
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return bool.Parse(value);
        }

        public static int GetInt32(this XElement element, string localName, int defaultValue = 0)
        {
            var name = XName.Get(localName);
            var value = element.Attribute(name)?.Value;
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return int.Parse(value);
        }

        public static T GetEnum<T>(this XElement element, string localName, T defaultValue)
        {
            var name = XName.Get(localName);
            var value = element.Attribute(name)?.Value;
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return (T)Enum.Parse(typeof(T), value);
        }

        public static string GetExtendedAttribute(this XElement element, string localName)
        {
            var name = XName.Get(localName, BPMTK_NS);
            return element.Attribute(name)?.Value;
        }

        public static XElement GetElement(this XElement element, string localName)
        {
            var name = XName.Get(localName, BPMN20_NS);
            return element.Element(name);
        }

        public static IEnumerable<XElement> GetElements(this XElement element, string localName)
        {
            var name = XName.Get(localName, BPMN20_NS);
            return element.Elements(name);
        }

        public static IEnumerable<XElement> GetExtendedElements(this XElement element, string localName)
        {
            var name = XName.Get(localName, BPMTK_NS);
            return element.Elements(name);
        }
    }
}
