using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public interface IHasDataOutputAssociations
    {
        IList<DataOutputAssociation> DataOutputAssociations
        {
            get;
        }
    }
}
