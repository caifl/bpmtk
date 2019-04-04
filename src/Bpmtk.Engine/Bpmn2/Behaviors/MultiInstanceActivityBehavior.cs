using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    abstract class MultiInstanceActivityBehavior : BpmnActivityBehavior
        //ICompositeActivityBehavior,
        //ISubProcessBehavior
    {
        protected Activity activity;
        protected BpmnActivityBehavior innerActivityBehavior;

        public MultiInstanceActivityBehavior(Activity activity, BpmnActivityBehavior innerActivityBehavior)
        {
            this.activity = activity;
            this.innerActivityBehavior = innerActivityBehavior;
            this.innerActivityBehavior.MultiInstanceActivityBehavior = this;
        }

        //protected virtual async Task<int> ResolveNrOfInstancesAsync(ExecutionContext executionContext)
        //{
        //    var value = await executionContext.GetVariableAsync("numberOfInstances");

        //    return value ?? 1;
        //}

        //protected virtual int? GetLoopVariable(ExecutionContext executionContext, string variableName)
        //{
        //    return (int?)executionContext.GetVariable(variableName);
        //}
        //protected override async Task CreateInstanceAsync(ExecutionContext executionContext)
        //{
        //    //标记为多实例
        //    var token = executionContext.Token;
        //    token.IsMIRoot = true;
        //    await executionContext.FlushAsync();

        //    await base.CreateInstanceAsync(executionContext);
        //}
        //public override Task EnterAsync(ExecutionContext executionContext)
        //{
        //    //Set IsMIRoot
        //    var token = executionContext.Token;
        //    token.IsMIRoot = true; 

        //    return base.EnterAsync(executionContext);
        //}

        //protected override Task CreateAsync(ExecutionContext executionContext)
        //{
        //    return base.CreateAsync(executionContext);
        //}

        public override void Execute(ExecutionContext executionContext)
        {
            this.CreateInstances(executionContext);
        }

        //public override void Execute(ExecutionContext executionContext)
        //{
        //    throw new NotImplementedException(nameof(MultiInstanceActivityBehavior));
        //    //var loopCounter = (int?)executionContext.GetVariable("loopCounter");
        //    //if (loopCounter == null)
        //    //{

        //    //}
        //    //else
        //    //    await this.innerActivityBehavior.ExecuteAsync(executionContext);
        //    //var token = executionContext.Token;
        //    //var node = token.Node;

        //    //var store = executionContext.InstanceStore;

        //    //int count = 3;
        //    //var list = new List<Token>();

        //    //for (int i = 0; i < count; i++)
        //    //{
        //    //    var child = token.CreateToken();
        //    //    child.Node = node;

        //    //    await child.SaveAsync(store);

        //    //    list.Add(child);
        //    //}

        //    //foreach (var item in list)
        //    //{
        //    //    var context = new ExecutionContext(item);
        //    //    await this.innerActivityBehavior.EnterAsync(context);
        //    //}
        //}

        protected virtual bool IsCompleted(ExecutionContext executionContext)
        {
            throw new NotImplementedException();
        }

        protected abstract int CreateInstances(ExecutionContext executionContext);

        public virtual BpmnActivityBehavior InnerActivityBehavior
        {
            get => this.innerActivityBehavior;
        }

        //public Task Completed(ExecutionContext context)
        //{
        //    return this.Leave(context);
        //}

        //public Task CompletingAsync(ExecutionContext context, ExecutionContext subProcessInstanceContext)
        //{
        //    return System.Threading.Tasks.Task.CompletedTask;
        //}

        //public override void Signal(ExecutionContext executionContext, string signalEvent, object signalData)
        //{
        //    return this.innerActivityBehavior.SignalAsync(executionContext, signalEvent, signalData);
        //}

        //public virtual Task LastActivityEndedAsync(ExecutionContext context)
        //{
        //    return this.Leave(context);
        //}

        //Task ICompositeActivityBehavior.LastActivityEndedAsync(ExecutionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //Task ISubProcessBehavior.CompletingAsync(ExecutionContext executionContext, ExecutionContext subProcessInstanceContext)
        //{
        //    throw new NotImplementedException();
        //}

        //Task ISubProcessBehavior.CompletedAsync(ExecutionContext executionContext)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
