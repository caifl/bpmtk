using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Bpmn2
{
    public interface IHasDataOutputs
    {
        IList<DataOutput> DataOutputs
        {
            get;
        }
    }
}
