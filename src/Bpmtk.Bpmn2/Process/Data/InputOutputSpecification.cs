using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public class InputOutputSpecification : BaseElement
    {
        public InputOutputSpecification()
        {
            this.DataInputs = new List<DataInput>();
            this.DataOutputs = new List<DataOutput>();
            this.InputSets = new List<InputSet>();
            this.OutputSets = new List<OutputSet>();
        }

        public virtual IList<DataInput> DataInputs
        {
            get;
        }

        public virtual IList<DataOutput> DataOutputs
        {
            get;
        }

        public virtual List<InputSet> InputSets
        {
            get;
        }

        public virtual List<OutputSet> OutputSets
        {
            get;
        }
    }
}
