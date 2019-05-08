using System;
using System.Linq;
using System.Collections.Generic;
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

        public virtual void ActivityStart(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            var historyManager = executionContext.Context.HistoryManager;
            historyManager.RecordActivityStart(contextImpl);

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "start");

            foreach (var item in this.processEventListeners)
                item.ActivityStart(contextImpl);
        }

        public virtual void ActivityEnd(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            var historyManager = executionContext.Context.HistoryManager;
            historyManager.RecordActivityEnd(contextImpl);

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "end");

            foreach (var item in this.processEventListeners)
                item.ActivityEnd(contextImpl);
        }

        public virtual void ProcessEnd(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "end");

            foreach (var item in this.processEventListeners)
                item.ProcessEnd(contextImpl);
        }

        public virtual void ActivityReady(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            var historyManager = executionContext.Context.HistoryManager;
            historyManager.RecordActivityReady((ExecutionContext)executionContext);

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "ready");

            foreach (var item in this.processEventListeners)
                item.ActivityReady(contextImpl);
        }

        public void ProcessStart(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            var historyManager = executionContext.Context.HistoryManager;
            historyManager.RecordProcessStart((ExecutionContext)executionContext);

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "start");

            foreach (var item in this.processEventListeners)
                item.ProcessStart(contextImpl);
        }

        public void TakeTransition(IExecutionContext executionContext)
        {
            var contextImpl = executionContext as ExecutionContext;

            this.ExecuteBpmnInlineScriptsOn(contextImpl, "taken");

            foreach (var item in this.processEventListeners)
                item.TakeTransition(contextImpl);
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
                        evaluator.Evaluate(item.Text);
                }
            }
        }
    }
}
