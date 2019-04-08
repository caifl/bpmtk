using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Tasks
{
    public class TaskIdentityLink : IdentityLink
    {
        public virtual TaskInstance Task
        {
            get;
            set;
        }
    }
}
