using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class Participant : BaseElement
    {
        public virtual string Name
        {
            get;
            set;
        }

        public virtual String processRef
        {
            get;
            set;
        }

        public virtual string InterfaceRef
        {
            get;
            set;
        }

        public virtual string EndPointRef
        {
            get;
            set;
        }

        public virtual ParticipantMultiplicity ParticipantMultiplicity
        {
            get;
            set;
        }
    }

    public class ParticipantMultiplicity
    {
        public ParticipantMultiplicity()
        {
            this.Minimum = 0;
            this.Maximum = 1;
        }

        public virtual int Minimum
        {
            get;
            set;
        }

        public virtual int Maximum
        {
            get;
            set;
        }
    }
}
