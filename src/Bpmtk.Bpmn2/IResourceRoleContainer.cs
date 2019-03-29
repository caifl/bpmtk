using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public interface IResourceRoleContainer
    {
        IList<ResourceRole> ResourceRoles
        {
            get;
        }
    }
}
