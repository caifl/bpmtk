using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2
{
    public class ExtensionElements
    {
        protected List<XElement> items = new List<XElement>();

        public virtual IList<XElement> Items => items;
    }
}
