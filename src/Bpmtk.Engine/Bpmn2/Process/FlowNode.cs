using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Bpmn2.Extensions;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Scripting;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Utils;

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

        protected virtual void OnEnter(ExecutionContext executionContext)
        {
            //创建活动实例
            //bool canActivate = true;
            //if (joinedTokens != null)
            //{
            //    var token = executionContext.Token;

            //    //CreateActivityInstance
            //    if (joinedTokens.Count == 1)
            //    {
            //        token.ActivityInstance = new ActivityInstance();
            //    }
            //    else
            //    {
            //        //findActivityInstance
            //        var act = joinedTokens.Where(x => x.ActivityInstance != null)
            //            .Select(x => x.ActivityInstance)
            //            .FirstOrDefault();
            //        token.ActivityInstance = act;
            //    }

            //    canActivate = this.incomings.Count == joinedTokens.Count;
            //}
            //Create activity-instance.
            executionContext.ActivityInstance = this.CreateActivityInstance(executionContext);

            var store = executionContext.Context.GetService<IInstanceStore>();
            store.Add(new HistoricToken(executionContext, "enter"));

            //create activity instance
            //if (canActivate)
            this.OnActivate(executionContext);
        }

        protected virtual ActivityInstance CreateActivityInstance(ExecutionContext executionContext)
        {
            return ActivityInstance.Create(executionContext);
        }

        protected virtual void OnActivate(ExecutionContext executionContext)
        {
            //激活活动实例
            var act = executionContext.ActivityInstance;
            if(act != null)
                act.Activate();

            var store = executionContext.Context.GetService<IInstanceStore>();
            store.Add(new HistoricToken(executionContext, "activate"));
        }

        public virtual void Enter(ExecutionContext executionContext)
        {
            var token = executionContext.Token;

            // update the runtime context information
            token.Node = this;
            token.ActivityInstance = null;

            // register entrance time so that a node-log can be generated upon leaving
            //token.setNodeEnter(Clock.getCurrentTime());

            // fire the leave-node event for this node
            this.OnEnter(executionContext);

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
            object signalData)
        {
            throw new NotSupportedException("The activity does not support signal event.");
        }

        //public virtual void Leave(ExecutionContext executionContext, bool ignoreConditions)

        public virtual void Leave(ExecutionContext executionContext)
        {
            Token token = executionContext.Token;
            var context = executionContext.Context;

            if (this.outgoings.Count == 0)
            {
                token.End(context, true);
                return;
            }

            SequenceFlow transition = null;
            if (this.outgoings.Count == 1)
                transition = this.outgoings[0];
            else
            {
                foreach (var outgoing in this.outgoings)
                {
                    var condition = outgoing.ConditionExpression;
                    if (condition == null || string.IsNullOrEmpty(condition.Text)
                        || !this.Evalute(condition.Text, executionContext))
                        continue;

                    transition = outgoing;
                    break;
                }

                if(transition == null)
                {
                    if(this is Activity)
                    {
                        transition = ((Activity)this).Default;
                    }
                    else if(this is ExclusiveGateway)
                    {
                        transition = ((ExclusiveGateway)this).Default;
                    }
                }
            }

            if (transition == null)
                throw new BpmnError("没有满足条件的分支可走");
           
            token.Node = this;
            executionContext.Transition = transition;

            // fire the leave-node event for this node
            //fireEvent(Event.EVENTTYPE_NODE_LEAVE, executionContext);
            this.OnLeave(executionContext);

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

        protected bool Evalute(string condition, ExecutionContext executionContext)
        {
            //extract expression.
            condition = StringHelper.ExtractExpression(condition);
            var engine = new JavascriptEngine();
            var scope = engine.CreateScope(new ScriptingContext(executionContext));

            var result = engine.Execute(condition, scope);
            if (result != null && Convert.ToBoolean(result))
                return true;

            return false;
        }

        protected virtual void OnLeave(ExecutionContext executionContext)
        {
            //activity-instance completed.
            var act = executionContext.ActivityInstance;
            if (act != null)
                act.Finish();

            var store = executionContext.Context.GetService<IInstanceStore>();
            store.Add(new HistoricToken(executionContext, "leave"));
        }

        protected virtual void LeaveAll(ExecutionContext executionContext)
        {
            if (this.outgoings.Count <= 1)
            {
                this.Leave(executionContext);
                return;
            }

            //fork.
            var token = executionContext.Token;
            token.Node = this;
            token.Inactivate();

            var context = executionContext.Context;
            var list = new List<ParallelTransition>();
            foreach (var outgoing in this.outgoings)
            {
                var childToken = token.CreateToken(context);
                childToken.Node = this;
                childToken.ActivityInstance = token.ActivityInstance;
                childToken.Scope = token.Scope;
                list.Add(new ParallelTransition(childToken, outgoing));
            }

            //fire leaveNode event.
            this.OnLeave(executionContext);

            foreach (var transition in list)
                transition.Take(context);
        }

        class ParallelTransition
        {
            private readonly Token token;
            private readonly SequenceFlow transition;

            public ParallelTransition(Token token, SequenceFlow transition)
            {
                this.token = token;
                this.transition = transition;
            }

            public virtual void Take(IContext context)
            {
                var executionContext = ExecutionContext.Create(context, this.token);
                executionContext.TransitionSource = transition.SourceRef;
                executionContext.Transition = transition;

                transition.Take(executionContext);
            }
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
