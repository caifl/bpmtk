using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Repository
{
    public class DefinitionIdentityLink : IdentityLink
    {
        public virtual ProcessDefinition ProcessDefinition
        {
            get;
            set;
        }
    }
}
