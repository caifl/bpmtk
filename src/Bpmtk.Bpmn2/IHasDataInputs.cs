using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Bpmn2
{
    public interface IHasDataInputs
    {
        IList<DataInput> DataInputs
        {
            get;
        }
    }
}
