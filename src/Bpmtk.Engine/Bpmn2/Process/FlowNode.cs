using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Bpmn2.Extensions;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    public abstract class FlowNode : FlowElement
    {
        protected List<SequenceFlow> incomings = new List<SequenceFlow>();
        protected List<SequenceFlow> outgoings = new List<SequenceFlow>();

        protected readonly Dictionary<string, string> attributes = new Dictionary<string, string>();
        protected readonly List<EventListener> eventListeners = new List<EventListener>();

        public virtual IList<SequenceFlow> Incomings => this.incomings;

        public virtual IList<SequenceFlow> Outgoings => this.outgoings;

        /// <summary>
        /// Get extended attributes of flow node.
        /// </summary>
        public virtual IDictionary<string, string> Attributes
        {
            get
            {
                return this.attributes;
            }
        }

        /// <summary>
        /// Gets the event listeners of flow node.
        /// </summary>
        public virtual IList<EventListener> EventListeners => this.eventListeners;

        public override string ToString()
        {
            return $"{this.GetType().Name}, {this.Id}, {this.Name}";
        }

        public abstract void Accept(IFlowNodeVisitor visitor);

        #region Runtime Support

        public virtual void Enter(ExecutionContext executionContext)
        {
            var token = executionContext.Token;

            // update the runtime context information
            token.Node = this;

            // register entrance time so that a node-log can be generated upon leaving
            //token.setNodeEnter(Clock.getCurrentTime());

            // fire the leave-node event for this node
            //fireEvent(Event.EVENTTYPE_NODE_ENTER, executionContext);

            // remove the transition references from the runtime context
            executionContext.Transition = null;
            executionContext.TransitionSource = null;

            // execute the node
            //if (isAsync)
            //{
            //    ExecuteNodeJob job = createAsyncContinuationJob(token);
            //    executionContext.getJbpmContext().getServices().getMessageService().send(job);
            //    token.lock (job.toString()) ;
            //}
            //else
            this.Execute(executionContext);
        }

        public virtual void Execute(ExecutionContext executionContext)
        {
            this.Leave(executionContext);
        }

        public virtual void Signal(ExecutionContext executionContext,
            string signalName,
            string signalData)
        {
            throw new NotSupportedException("The activity does not support signal event.");
        }

        public virtual void Leave(ExecutionContext executionContext, string transitionId = null)
        {
            //if (transition == null)
            //    throw new Exception("transition is null");
            var transition = this.Outgoings.Where(x => x.Id == transitionId)
                .SingleOrDefault();
            if (transition == null)
                transition = this.outgoings.FirstOrDefault();

            Token token = executionContext.Token;
            token.Node = this;
            executionContext.Transition = transition;

            // fire the leave-node event for this node
            //fireEvent(Event.EVENTTYPE_NODE_LEAVE, executionContext);

            // log this node
            //if (token.getNodeEnter() != null)
            //{
            //    addNodeLog(token);
            //}

            // update the runtime information for taking the transition
            // the transitionSource is used to calculate events on superstates
            executionContext.TransitionSource = this;

            // take the transition
            transition.Take(executionContext);
        }

        #endregion

        public override int GetHashCode()
        {
            if (this.Id != null)
                return this.GetType().Name.GetHashCode() ^ this.Id.GetHashCode();

            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var flowNode = obj as FlowNode;
            if (flowNode != null)
                return string.Compare(flowNode.Id, this.Id) == 0;

            return base.Equals(obj);
        }
    }
}
