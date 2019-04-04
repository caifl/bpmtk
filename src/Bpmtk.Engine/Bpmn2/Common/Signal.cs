using System;

namespace Bpmtk.Engine.Bpmn2
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
