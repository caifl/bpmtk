using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public interface IHasDataInputAssociations
    {
        IList<DataInputAssociation> DataInputAssociations
        {
            get;
        }
    }
}
