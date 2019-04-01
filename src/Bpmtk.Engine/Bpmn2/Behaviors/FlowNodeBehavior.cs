using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    abstract class FlowNodeBehavior : IBpmnActivityBehavior, ISignallableActivityBehavior
    {
        public virtual void Execute(ExecutionContext executionContext)
        {
            
        }

        public virtual void Leave(ExecutionContext executionContext)
        {

        }

        public virtual void Leave(ExecutionContext executionContext, BpmnTransition transition)
        {
            //executionContext.LeaveNode();
            this.LeaveDefault(executionContext);
        }

        public virtual void Signal(ExecutionContext executionContext, string signalName, object signalData)
        {
            throw new NotSupportedException("The activity can't support signal method.");
        }

        protected virtual void LeaveDefault(ExecutionContext executionContext)
        {
            Token token = executionContext.Token;

            var activity = token.Activity;
            var transition = activity.GetOutgoingById("");
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
            executionContext.TransitionSource = activity;

            //take the transition
            transition.Take(executionContext);
        }
    }
}
