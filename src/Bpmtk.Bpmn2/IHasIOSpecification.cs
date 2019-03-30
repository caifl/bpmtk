using System;

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
