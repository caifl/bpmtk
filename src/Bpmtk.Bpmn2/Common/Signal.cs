using System;

namespace Bpmtk.Bpmn2
{
    public class Signal : RootElement
    {
        public virtual string Name
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
