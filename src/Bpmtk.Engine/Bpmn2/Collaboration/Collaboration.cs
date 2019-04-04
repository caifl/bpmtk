using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public class Collaboration : RootElement
    {
        private List<Participant> participants = new List<Participant>();
        private List<MessageFlow> messageFlows = new List<MessageFlow>();
        private List<Artifact> artifacts = new List<Artifact>();
        private List<ConversationNode> conversationNodes = new List<ConversationNode>();
        private List<CorrelationKey> correlationKeys = new List<CorrelationKey>();

        public virtual string Name
        {
            get;
            set;
        }

        public virtual bool IsClosed
        {
            get;
            set;
        }

        public virtual IList<Participant> Participants => this.participants;

        public virtual IList<MessageFlow> MessageFlows => this.messageFlows;

        public virtual IList<Artifact> Artifacts => this.artifacts;

        public virtual IList<ConversationNode> ConversationNodes => this.conversationNodes;

        public virtual IList<ConversationAssociation> ConversationAssociations
        {
            get;
        }

        public virtual IList<MessageFlowAssociation> MessageFlowAssociations
        {
            get;
        }

        public virtual IList<CorrelationKey> CorrelationKeys => this.correlationKeys;

        public virtual IList<string> ChoreographyRefs
        {
            get;
        }

        public virtual IList<ConversationLink> ConversationLinks
        {
            get;
        }
    }
}
