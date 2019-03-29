using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public class InputSet : BaseElement
    {
        public InputSet()
        {
            this.DataInputRefs = new List<string>();
            this.OptionalInputRefs = new List<string>();
            this.WhileExecutingInputRefs = new List<string>();
            this.OutputSetRefs = new List<string>();
        }

        public virtual IList<string> DataInputRefs
        {
            get;
        }

        public virtual IList<string> OptionalInputRefs
        {
            get;
        }

        public virtual IList<string> WhileExecutingInputRefs
        {
            get;
        }

        public virtual IList<string> OutputSetRefs
        {
            get;
        }

        public virtual string Name
        {
            get;
            set;
        }
    }
}
