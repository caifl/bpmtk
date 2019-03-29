using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    static class Helper
    {
        const string XSI_NS = "http://www.w3.org/2001/XMLSchema-instance";

        public static string GetRealLocalName(XElement element)
        {
            var localName = element.Name.LocalName;
            if(element.HasAttributes)
            {
                var typeName = element.Attribute(XName.Get("type", XSI_NS))?.Value;
                if (typeName != null)
                {
                    typeName = Char.ToLowerInvariant(typeName[1]) + typeName.Substring(2);
                    return typeName;
                }
            }

            return localName;
        }

        public static bool IsFormalExpression(XElement element)
        {
            return element.Name.LocalName == "formalExpression"
                || element.Attribute(XName.Get("type", XSI_NS))?.Value == "tFormalExpression";
        }
    }
}
