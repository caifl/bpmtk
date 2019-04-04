using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public class InputSet : BaseElement
    {
        public InputSet()
        {
            this.DataInputRefs = new List<DataInput>();
            this.OptionalInputRefs = new List<DataInput>();
            this.WhileExecutingInputRefs = new List<DataInput>();
            this.OutputSetRefs = new List<OutputSet>();
        }

        public virtual IList<DataInput> DataInputRefs
        {
            get;
        }

        public virtual IList<DataInput> OptionalInputRefs
        {
            get;
        }

        public virtual IList<DataInput> WhileExecutingInputRefs
        {
            get;
        }

        public virtual IList<OutputSet> OutputSetRefs
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
