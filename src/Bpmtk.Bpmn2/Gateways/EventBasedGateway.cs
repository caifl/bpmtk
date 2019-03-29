using System;

namespace Bpmtk.Bpmn2
{
    public class EventBasedGateway : Gateway
    {
        public EventBasedGateway()
        {
            this.EventGatewayType = EventBasedGatewayType.Exclusive;
        }

        public virtual bool Instantiate
        {
            get;
            set;
        }

        public virtual EventBasedGatewayType EventGatewayType
        {
            get;
            set;
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public enum EventBasedGatewayType
    {
        /// <remarks/>
        Exclusive,

        /// <remarks/>
        Parallel,
    }
}
