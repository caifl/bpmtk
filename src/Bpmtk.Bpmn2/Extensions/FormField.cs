using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2.Extensions
{
    public class FormField
    {
        protected readonly List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

        public string Id
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public string Type
        {
            get;
            protected set;
        }

        public bool Readable
        {
            get;
            protected set;
        }

        public bool Required
        {
            get;
            protected set;
        }

        public bool Writable
        {
            get;
            protected set;
        }

        public string Variable
        {
            get;
            protected set;
        }

        public string Format
        {
            get;
            protected set;
        }

        public string Pattern
        {
            get;
            protected set;
        }

        public string Value
        {
            get;
            protected set;
        }

        public virtual IReadOnlyCollection<KeyValuePair<string, string>> Values => this.values.AsReadOnly();
    }
}
