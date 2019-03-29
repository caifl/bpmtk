using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public interface IPropertyContainer
    {
        IList<Property> Properties
        {
            get;
        }
    }
}
