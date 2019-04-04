using System;

namespace Bpmtk.Engine.Bpmn2
{  
    public abstract class Gateway : FlowNode
    {
        public Gateway()
        {
            this.GatewayDirection = GatewayDirection.Unspecified;
        }

        public virtual GatewayDirection GatewayDirection
        {
            get;
            set;
        }

        public virtual SequenceFlow Default
        {
            get;
            set;
        }
    }

    public enum GatewayDirection
    {
        /// <remarks/>
        Unspecified,

        /// <remarks/>
        Converging,

        /// <remarks/>
        Diverging,

        /// <remarks/>
        Mixed,
    }
}
