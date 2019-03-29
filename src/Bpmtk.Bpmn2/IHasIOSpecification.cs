using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Bpmn2
{
    public interface IHasIOSpecification
    {
        IOSpecification IOSpecification
        {
            get;
            set;
        }
    }
}
