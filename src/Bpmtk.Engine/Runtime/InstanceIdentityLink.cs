using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Runtime
{
    public class InstanceIdentityLink : IdentityLink
    {
        public virtual ProcessInstance ProcessInstance
        {
            get;
            set;
        }
    }
}
