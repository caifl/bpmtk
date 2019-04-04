using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public class ConversationNode : BaseElement
    {
        private List<CorrelationKey> correlationKeys = new List<CorrelationKey>();

        public virtual string Name { get; set; }

        public virtual Participant ParticipantRef { get; set; }

        public virtual MessageFlow MessageFlowRef { get; set; }

        public virtual IList<CorrelationKey> CorrelationKeys => this.correlationKeys;
    }

    public class CorrelationKey : BaseElement
    {
        private List<string> correlationPropertyRefs = new List<string>();

        public virtual string Name { get; set; }

        public virtual IList<string> CorrelationPropertyRefs => this.correlationPropertyRefs;
    }
}
