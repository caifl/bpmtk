using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    public class BpmnTransition
    {
        private readonly SequenceFlow sequenceFlow;

        public BpmnTransition(SequenceFlow sequenceFlow)
        {
            this.sequenceFlow = sequenceFlow;
        }

        public virtual string Id => this.sequenceFlow.Id;

        public virtual BpmnActivity Target
        {
            get => new BpmnActivity(this.sequenceFlow.TargetRef);
        }

        public virtual void Take(ExecutionContext executionContext)
        {
            // update the runtime context information
            Token token = executionContext.Token;
            token.Activity = null;

            // start the transition log
            //TransitionLog transitionLog = new TransitionLog(this,
            //  executionContext.getTransitionSource());
            //token.startCompositeLog(transitionLog);
            //try
            //{
            //    // fire leave events for superstates (if any)
            //    fireSuperStateLeaveEvents(executionContext);

            //    // fire the transition event (if any)
            //    fireEvent(Event.EVENTTYPE_TRANSITION, executionContext);

            //    // fire enter events for superstates (if any)
            //    Node destination = fireSuperStateEnterEvents(executionContext);
            //    // update the ultimate destinationNode of this transition
            //    transitionLog.setDestinationNode(destination);
            //}
            //finally
            //{
            //    // end the transition log
            //    token.endCompositeLog();
            //}

            // pass the token to the destinationNode node
            this.Target.Enter(executionContext);
        }
    }
}
