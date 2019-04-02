using System;

namespace Bpmtk.Bpmn2
{    
    public class Error : RootElement
    {
        public virtual string Name
        {
            get;
            set;
        }

        public virtual string ErrorCode
        {
            get;
            set;
        }

        public virtual ItemDefinition StructureRef
        {
            get;
            set;
        }
    }
}
