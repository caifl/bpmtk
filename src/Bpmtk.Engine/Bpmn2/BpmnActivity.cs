using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    public class BpmnActivity
    {
        private readonly FlowNode node;
        private IBpmnActivityBehavior behavior;

        public BpmnActivity(FlowNode node)
        {
            this.node = node;
        }

       
        public virtual BpmnTransition GetOutgoingById(string id)
        {
            return null;
        }

        public virtual IEnumerable<BpmnTransition> Outgoings
        {
            get;
        }

        public virtual string Id => this.node.Id;

        public virtual string Name => this.node.Name;

        public virtual string Description => null;

        public virtual void Enter(ExecutionContext executionContext)
        {
            var token = executionContext.Token;

            // update the runtime context information
            token.Activity = this;

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
            {
                this.Execute(executionContext);
            }
        }

        public virtual void Execute(ExecutionContext executionContext)
        {
            if (this.behavior != null)
            {
                this.behavior.Execute(executionContext);
                return;
            }

            this.Leave(executionContext);
        }

        public virtual void Signal(ExecutionContext executionContext,
            string signalName, object signalData)
        {
            var token = executionContext.Token;
            token.Activity = this;

            if (this.behavior is ISignallableActivityBehavior)
            {
                ((ISignallableActivityBehavior)this.behavior).Signal(executionContext,
                    signalName, signalData);
                return;
            }
        }

        public virtual void Leave(ExecutionContext executionContext, 
            BpmnTransition transition = null)
        {
            if (transition == null)
                throw new Exception("transition is null");

            Token token = executionContext.Token;
            token.Activity = this;
            executionContext.Transition = transition;

            this.behavior.Leave(executionContext, transition);

            // fire the leave-node event for this node
            //fireEvent(Event.EVENTTYPE_NODE_LEAVE, executionContext);

            // log this node
            //if (token.getNodeEnter() != null)
            //{
            //    addNodeLog(token);
            //}

            // update the runtime information for taking the transition
            // the transitionSource is used to calculate events on superstates
            //executionContext.TransitionSource = this;

            // take the transition
            //transition.Take(executionContext);
        }
    }
}
