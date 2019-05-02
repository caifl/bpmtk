using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Events
{
    public class CompositeProcessEventListener : IProcessEventListener
    {
        private readonly IEnumerable<IProcessEventListener> processEventListeners;

        public CompositeProcessEventListener(IEnumerable<IProcessEventListener> processEventListeners)
        {
            this.processEventListeners = processEventListeners;
        }

        public virtual async Task ActivityStartAsync(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            var historyManager = executionContext.Context.HistoryManager;
            await historyManager.RecordActivityStartAsync(contextImpl);

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "start");

            foreach (var item in this.processEventListeners)
                await item.ActivityStartAsync(contextImpl);
        }

        public virtual async Task ActivityEndAsync(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            var historyManager = executionContext.Context.HistoryManager;
            await historyManager.RecordActivityEndAsync(contextImpl);

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "end");

            foreach (var item in this.processEventListeners)
                await item.ActivityEndAsync(contextImpl);
        }

        public virtual async Task ProcessEndAsync(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "end");

            foreach (var item in this.processEventListeners)
                await item.ProcessEndAsync(contextImpl);
        }

        public virtual async Task ActivityReadyAsync(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            var historyManager = executionContext.Context.HistoryManager;
            await historyManager.RecordActivityReadyAsync((ExecutionContext)executionContext);

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "ready");

            foreach (var item in this.processEventListeners)
                await item.ActivityReadyAsync(contextImpl);
        }

        public async Task ProcessStartAsync(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "start");

            foreach (var item in this.processEventListeners)
                await item.ProcessStartAsync(contextImpl);
        }

        public async Task TakeTransitionAsync(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "taken");

            foreach (var item in this.processEventListeners)
                await item.TakeTransitionAsync(contextImpl);
        }

        protected virtual void ExecuteBpmnInlineScriptsOn(ExecutionContext executionContext, string eventName)
        {
            IList<Bpmtk.Bpmn2.Extensions.Script> scripts = null;

            var node = executionContext.Node;
            if (node != null)
                scripts = node.Scripts;
            else
                scripts = executionContext.Transition?.Scripts;

            if (scripts != null && scripts.Count > 0)
            {
                var list = scripts.Where(x => x.On.Equals(eventName)).ToList();
                if (list.Count > 0)
                {
                    var evaluator = executionContext.GetEvaluator();
                    foreach (var item in list)
                        evaluator.Evalute(item.Text);
                }
            }
        }
    }
}
