using System;
using System.Collections.Generic;
using Bpmtk.Bpmn2.Extensions;

namespace Bpmtk.Bpmn2
{
    public abstract class FlowNode : FlowElement, IScriptEnabledElement
    {
        protected List<SequenceFlow> incomings = new List<SequenceFlow>();
        protected List<SequenceFlow> outgoings = new List<SequenceFlow>();

        protected List<Script> scripts = new List<Script>();

        public virtual IList<SequenceFlow> Incomings => this.incomings;

        public virtual IList<SequenceFlow> Outgoings => this.outgoings;

        public virtual IList<Script> Scripts => this.scripts;

        public override string ToString()
        {
            return $"{this.GetType().Name}, {this.Id}, {this.Name}";
        }

        public virtual object Tag
        {
            get;
            set;
        }

        public abstract void Accept(IFlowNodeVisitor visitor);

        #region Runtime Support

        //protected virtual void OnActivating(ExecutionContext executionContext)
        //{
        //    var token = executionContext.Token;

        //    var activityInstance = new ActivityInstance();//ActivityInstance.Create(executionContext);
        //    token.ActivityInstance = activityInstance;

        //    // remove the transition references from the runtime context
        //    executionContext.Transition = null;
        //    executionContext.TransitionSource = null;

        //    var store = executionContext.Context.GetService<IInstanceStore>();
        //    store.Add(new HistoricToken(executionContext, "activating"));
        //}

        //protected virtual void OnActivated(ExecutionContext executionContext)
        //{
        //    //fire activated event.
        //    var store = executionContext.Context.GetService<IInstanceStore>();
        //    store.Add(new HistoricToken(executionContext, "activated"));

        //    this.ExecuteScripts("activated", executionContext);
        //}

       
        //protected virtual void Activate(ExecutionContext executionContext, 
        //    IDictionary<string, object> variables = null)
        //{
        //    this.OnActivating(executionContext);

        //    var activityInstance = executionContext.ActivityInstance;
        //    if (activityInstance != null)
        //    {
        //        //activityInstance.InitializeContext(executionContext.Context, variables);
        //        activityInstance.Activate();
        //    }

        //    this.OnActivated(executionContext);

        //    this.Execute(executionContext);
        //}

        //public virtual void Enter(ExecutionContext executionContext)
        //{
        //    var token = executionContext.Token;

        //    // update the runtime context information
        //    token.Node = this;
        //    token.ActivityInstance = null;

        //    // register entrance time so that a node-log can be generated upon leaving
        //    token.EnterTime = Clock.Now;

        //    // fire the enter-node event for this node
        //    this.OnEnter(executionContext);

        //    // Activate
        //    this.Activate(executionContext);
        //}

        //public virtual void LeaveDefault(ExecutionContext executionContext)
        //{
        //    Token token = executionContext.Token;
        //    var context = executionContext.Context;

        //    if (this.outgoings.Count == 0)
        //    {
        //        token.End(context, true);
        //        return;
        //    }

        //    SequenceFlow transition = null;
        //    if (this.outgoings.Count == 1)
        //        transition = this.outgoings[0];
        //    else
        //    {
        //        foreach (var outgoing in this.outgoings)
        //        {
        //            var condition = outgoing.ConditionExpression;
        //            if (condition == null || string.IsNullOrEmpty(condition.Text)
        //                || !executionContext.EvaluteExpression<bool>(condition.Text))
        //                continue;

        //            transition = outgoing;
        //            break;
        //        }

        //        if (transition == null)
        //        {
        //            if (this is Activity)
        //            {
        //                transition = ((Activity)this).Default;
        //            }
        //            else if (this is ExclusiveGateway)
        //            {
        //                transition = ((ExclusiveGateway)this).Default;
        //            }
        //        }
        //    }

        //    if (transition == null)
        //        throw new RuntimeException("没有满足条件的分支可走");

        //    token.Node = this;
        //    executionContext.Transition = transition;

        //    // fire the leave-node event for this node
        //    //fireEvent(Event.EVENTTYPE_NODE_LEAVE, executionContext);
        //    this.OnLeave(executionContext);

        //    // log this node
        //    //if (token.getNodeEnter() != null)
        //    //{
        //    //    addNodeLog(token);
        //    //}

        //    // update the runtime information for taking the transition
        //    // the transitionSource is used to calculate events on superstates
        //    executionContext.TransitionSource = this;

        //    // take the transition
        //    //transition.Take(executionContext);
        //}

        //public virtual void Leave(ExecutionContext executionContext, bool ignoreConditions)
        //{
        //    Token token = executionContext.Token;
        //    var context = executionContext.Context;

        //    if (this.outgoings.Count == 0)
        //    {
        //        token.End(context, true);
        //        return;
        //    }

        //    IList<SequenceFlow> transitions;
        //    SequenceFlow transition = null;
        //    if (!ignoreConditions)
        //    {
        //        transitions = new List<SequenceFlow>();

        //        foreach (var outgoing in this.outgoings)
        //        {
        //            var condition = outgoing.ConditionExpression;
        //            if (condition == null || string.IsNullOrEmpty(condition.Text)
        //                || !executionContext.EvaluteExpression<bool>(condition.Text))
        //                continue;

        //            transitions.Add(outgoing);
        //        }

        //        if(transitions.Count == 0)
        //        {
        //            if (this is Activity)
        //            {
        //                transition = ((Activity)this).Default;
        //                if (transition != null)
        //                    transitions.Add(transition);
        //            }
        //            else if (this is ExclusiveGateway)
        //            {
        //                transition = ((ExclusiveGateway)this).Default;
        //                if (transition != null)
        //                    transitions.Add(transition);
        //            }
        //        }
        //    }
        //    else
        //        transitions = this.outgoings;

        //    if (transitions.Count == 0)
        //        throw new RuntimeException("没有满足条件的分支可走");

        //    if (transitions.Count > 1)
        //    {
        //        var list = new List<ParallelTransition>();
        //        foreach (var outgoing in transitions)
        //        {
        //            var childToken = token.CreateToken(context);
        //            childToken.Node = this;
        //            childToken.ActivityInstance = token.ActivityInstance;
        //            childToken.Scope = token.Scope;
        //            list.Add(new ParallelTransition(childToken, outgoing));
        //        }

        //        //fire leaveNode event.
        //        this.OnLeave(executionContext);

        //        foreach (var parallelTransition in list)
        //            parallelTransition.Take(context);
        //    }
        //    else
        //    {
        //        transition = transitions[0];
        //        token.Node = this;
        //        executionContext.Transition = transition;

        //        // fire the leave-node event for this node
        //        //fireEvent(Event.EVENTTYPE_NODE_LEAVE, executionContext);
        //        this.OnLeave(executionContext);

        //        // log this node
        //        //if (token.getNodeEnter() != null)
        //        //{
        //        //    addNodeLog(token);
        //        //}

        //        // update the runtime information for taking the transition
        //        // the transitionSource is used to calculate events on superstates
        //        executionContext.TransitionSource = this;

        //        // take the transition
        //        transition.Take(executionContext);
        //    }
        //}

        //protected virtual void OnLeave(ExecutionContext executionContext)
        //{
        //    //activity-instance completed.
        //    var act = executionContext.ActivityInstance;
        //    if (act != null)
        //        act.Finish();

        //    var store = executionContext.Context.GetService<IInstanceStore>();
        //    store.Add(new HistoricToken(executionContext, "leave"));

        //    this.ExecuteScripts("leave", executionContext);
        //}

        //protected virtual void LeaveAll(ExecutionContext executionContext)
        //{
        //    if (this.outgoings.Count <= 1)
        //    {
        //        this.Leave(executionContext);
        //        return;
        //    }

        //    //fork.
        //    var token = executionContext.Token;
        //    token.Node = this;
        //    token.Inactivate();

        //    var context = executionContext.Context;
        //    var list = new List<ParallelTransition>();
        //    foreach (var outgoing in this.outgoings)
        //    {
        //        var childToken = token.CreateToken(context);
        //        childToken.Node = this;
        //        childToken.ActivityInstance = token.ActivityInstance;
        //        childToken.Scope = token.Scope;
        //        list.Add(new ParallelTransition(childToken, outgoing));
        //    }

        //    //fire leaveNode event.
        //    this.OnLeave(executionContext);

        //    foreach (var transition in list)
        //        transition.Take(context);
        //}

        //class ParallelTransition
        //{
        //    private readonly Token token;
        //    private readonly SequenceFlow transition;

        //    public ParallelTransition(Token token, SequenceFlow transition)
        //    {
        //        this.token = token;
        //        this.transition = transition;
        //    }

        //    public virtual void Take(IContext context)
        //    {
        //        var executionContext = ExecutionContext.Create(context, this.token);
        //        executionContext.TransitionSource = transition.SourceRef;
        //        executionContext.Transition = transition;

        //        //transition.Take(executionContext);
        //    }
        //}

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
