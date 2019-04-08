
using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    public class BpmnActivityBehavior
    {
        public static void Enter(ExecutionContext executionContext)
        {
            //var token = executionContext.Token;

            // register entrance time so that a node-log can be generated upon leaving
            //token.setNodeEnter(Clock.getCurrentTime());

            // fire the leave-node event for this node
            Activating(executionContext);

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
            Execute(executionContext);
        }

        public static void Activating(ExecutionContext executionContext)
        {

        }

        public static void Execute(ExecutionContext executionContext)
        {

        }
    }
}
