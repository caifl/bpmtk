using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Scripting;
using Bpmtk.Engine.Utils;
using Bpmtk.Engine.Bpmn2.Behaviors;
using Microsoft.Extensions.Logging;

namespace Bpmtk.Engine.Runtime
{
    public class ExecutionContext : IExecutionContext
    {
        protected ILogger logger;
        private Token token;
        private SequenceFlow transition;

        protected ExecutionContext(Context context, Token token)
        {
            var engine = context.Engine;

            this.Context = context;
            this.token = token;
            this.ProcessInstance = token.ProcessInstance;
            this.logger = engine.LoggerFactory.CreateLogger<ExecutionContext>();
        }

        public virtual ILogger Logger => this.logger;

        public static ExecutionContext Create(Context context, Token token)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return new ExecutionContext(context, token);
        }

        public virtual void Start()
        {
            this.EnterNode(this.Node);
        }

        public virtual void Start(FlowNode initialNode)
        {
            if (initialNode == null)
                throw new ArgumentNullException(nameof(initialNode));

            this.EnterNode(initialNode);
        }

        public virtual void Trigger(string signalEvent, IDictionary<string, object> signalData)
        {
            var behavior = this.Node.Tag as ISignallableActivityBehavior;
            if (behavior != null)
            {
                behavior.Signal(this, signalEvent, signalData);
                return;
            }

            throw new NotSupportedException();
        }

        public virtual IList<ExecutionContext> CreateInnerExecutions(int numberOfInstances)
        {
            var list = new List<ExecutionContext>();

            for (int i = 0; i < numberOfInstances; i++)
            {
                var childToken = this.token.CreateToken();
                childToken.Node = this.Node;

                var innerExecution = ExecutionContext.Create(this.Context, childToken);

                list.Add(innerExecution);
            }

            this.Context.DbSession.Flush();

            return list;
        }

        public virtual ExecutionContext StartSubProcessAsync(FlowNode initialNode,
            IDictionary<string, object> variables)
        {
            //
            this.token.IsScope = true;

            //Initialize scope context.

            //Create sub-token.
            var subToken = this.token.CreateToken();
            subToken.Node = initialNode;

            //Save changes.
            this.Context.DbSession.Flush();

            var subExecution = Create(this.Context, subToken);
            subExecution.EnterNode(initialNode);

            return subExecution;
        }

        public virtual void End()
        {
            this.token.IsActive = false;
            this.token.IsEnded = true;

            //fire activityEndEvent.
            var runtimeManager = this.Context.RuntimeManager;
            var eventListener = runtimeManager.GetCompositeProcessEventListener();
            eventListener.ActivityEnd(this);

            var parentToken = this.token.Parent;
            if (parentToken != null)
            {
                //判断是否在子流程中
                var container = this.Node.Container;
                if (container is SubProcess)
                {
                    this.token.Remove();

                    if (parentToken.Children.Count > 0)
                        return;

                    var subProcess = container as SubProcess;

                    //Try
                    //var p = parentToken;
                    //while (!p.Node.Equals(subProcess))
                    //{
                    //    if (p.Children.Count > 0) //还有未完成的并发执行
                    //        return;

                    //    p.Remove();
                    //    p = p.Parent;
                    //}

                    var subProcessContext = ExecutionContext.Create(this.Context, parentToken);
                    var behavior = subProcess.Tag as IFlowNodeActivityBehavior;
                    behavior.Leave(subProcessContext);
                    return;
                }
            }


            //结束流程实例
            //this.ProcessInstance.End(context, isImplicit, endReason);
            var procInst = this.ProcessInstance;

            this.token.Remove();
            var tokens = procInst.Tokens;
            if (tokens.Count > 0)
                return;

            //
            procInst.State = ExecutionState.Completed;
            procInst.LastStateTime = Clock.Now;

            this.Context.DbSession.Flush();

            //fire processEndEvent.
            eventListener.ProcessEnd(this);

            var superToken = procInst.Super;
            if(superToken != null)
            {
                //to be continued ..
                throw new NotImplementedException("CallActivity not implemented.");
            }
        }

        public virtual int GetActiveTaskCountAsync()
        {
            return this.Context.RuntimeManager.GetActiveTaskCount(this.token.Id);
        }

        public virtual ProcessInstance ProcessInstance
        {
            get;
        }

        public virtual Token Token => this.token;

        public virtual FlowNode Node
        {
            get
            {
                var node = this.token.Node;
                if(node == null && this.token.ActivityId != null)
                {
                    var processDefinition = this.ProcessInstance.ProcessDefinition;
                    var deploymentId = processDefinition.DeploymentId;
                    var model = this.Context.DeploymentManager.GetBpmnModel(deploymentId);

                    node = model.GetFlowElement(this.token.ActivityId) as FlowNode;
                    this.token.Node = node;
                }

                return node;
            }
        }

        public virtual void ReplaceToken(Token token)
        {
            var oldToken = this.token;

            this.token = token;

            this.token.Node = oldToken.Node;
            this.token.TransitionId = oldToken.TransitionId;
            this.token.ActivityInstance = oldToken.ActivityInstance;

            //re-activate.
            this.token.Activate();
        }

        public virtual SequenceFlow Transition
        {
            get => transition;
            set
            {
                this.transition = value;
                this.token.TransitionId = this.transition?.Id;
            }
        }

        public virtual ActivityInstance TransitionSource
        {
            get;
            set;
        }

        public virtual ActivityInstance ActivityInstance
        {
            get => this.token.ActivityInstance;
            set => this.token.ActivityInstance = value;
        }

        protected virtual void EnterNode(FlowNode node)
        {
            this.token.Node = node;
            var behavior = node.Tag as IFlowNodeActivityBehavior;
            if (behavior != null)
            {
                var historyManager = this.Context.HistoryManager;

                //fire activityReadyEvent.
                var eventListener = this.Context.RuntimeManager.GetCompositeProcessEventListener();
                eventListener.ActivityReady(this);

                var isPreConditionsSatisfied = behavior.EvaluatePreConditions(this);
                if (isPreConditionsSatisfied)
                {
                    //Clear
                    this.Transition = null;
                    this.TransitionSource = null;

                    //fire activityStartEvent.
                    eventListener.ActivityStart(this);

                    behavior.Execute(this);
                }
                else
                {
                    this.logger.LogInformation($"The activity '{this.Node.Id}' pre-conditions is not satisfied.");
                }

                return;
            }

            throw new NotSupportedException();
        }

        public virtual void LeaveNode(SequenceFlow transition)
        {
            if (transition == null)
                throw new ArgumentNullException(nameof(transition));

            //fire activityEndEvent.
            var eventListener = this.Context.RuntimeManager.GetCompositeProcessEventListener();
            eventListener.ActivityEnd(this);

            this.Take(transition);
        }

        public virtual void LeaveNode(IEnumerable<SequenceFlow> transitions)
        {
            if (transitions == null)
                throw new ArgumentNullException(nameof(transitions));

            //fire activityEndEvent.
            var eventListener = this.Context.RuntimeManager.GetCompositeProcessEventListener();
            eventListener.ActivityEnd(this);

            if (transitions.Count() > 1)
            {
                //Set current token inactive.
                this.token.Inactivate();

                var outgoingExecutions = new List<OutgoingExecution>();
                foreach (var transition in transitions)
                {
                    var childToken = this.token.CreateToken();

                    var outgoingContext = new ExecutionContext(this.Context, childToken);
                    outgoingContext.Transition = transition;

                    outgoingExecutions.Add(new OutgoingExecution(outgoingContext, transition));
                }

                //Ensure all concurrent tokens persisted.
                var db = this.Context.DbSession;
                db.Flush();

                foreach (var execution in outgoingExecutions)
                {
                    if (!execution.IsEnded) //Check if execution ended.
                        execution.Execute();
                }
            }
            else
            {
                this.Transition = transitions.First();
                this.Take(transition);
            }
        }

        public virtual void Join()
        {
            if (this.JoinedTokens == null)
                return;

            var joinedTokens = new List<Token>(this.JoinedTokens);
            if (joinedTokens == null || joinedTokens.Count == 0)
                return;

            var scopeToken = this.token.ResolveScope();

            //Exclude current token.
            joinedTokens.Remove(token);

            //Exclude scope token.
            if(scopeToken != null)
                joinedTokens.Remove(scopeToken);

            //Try to remove the others branch.
            Token current = null;
            foreach (var pToken in joinedTokens)
            {
                current = pToken;
                current.Remove();

                //Traverse token tree up.
                current = current.Parent;
                while ( current.Parent != scopeToken && current.Children.Count == 0)
                {
                    current.Remove();
                    current = current.Parent;
                }
            }

            //Try to remove current branch.
            var parent = token.Parent;
            if (parent != scopeToken && parent.Children.Count == 1)
            {
                current = token;
                while (current.Parent != scopeToken && current.Children.Count == 0)
                {
                    current.Remove();
                    current = current.Parent;
                }
            }

            if (!current.Equals(token))
                this.ReplaceToken(current);

            var db = this.Context.DbSession;
            db.Flush();
        }

        protected virtual void Take(SequenceFlow transition)
        {
            //Store transition source activity instance.
            this.TransitionSource = this.token.ActivityInstance;

            //Clear related information.
            this.token.Node = null;
            this.token.ActivityInstance = null;
            this.token.IsActive = true;
            this.token.IsMIRoot = false;
            this.token.Clear();

            this.JoinedTokens = null;

            //Set outgoing transition.
            this.Transition = transition;

            //fire transitionTakenEvent.
            var eventListener = this.Context.RuntimeManager.GetCompositeProcessEventListener();
            eventListener.TakeTransition(this);

            var targetNode = transition.TargetRef;
            this.EnterNode(targetNode);
        }

        public virtual void FireActivityStartEvent()
        {

        }

        public virtual bool TryGetVariable(string name, out object value)
            => this.token.TryGetVariable(name, out value);

        public virtual object GetVariable(string name)
            => this.token.GetVariable(name);

        public virtual object GetVariableLocal(string name)
            => this.token.GetVariableLocal(name);

        public virtual TValue GetVariable<TValue>(string name)
        {
            var value = this.GetVariable(name);
            if (value != null)
                return (TValue)value;

            return default(TValue);
        }

        public virtual TValue GetVariableLocal<TValue>(string name)
        {
            var value = this.GetVariableLocal(name);
            if (value != null)
                return (TValue)value;

            return default(TValue);
        }

        public virtual ExecutionContext SetVariable(string name, object value)
        {
            this.token.SetVariable(name, value);

            return this;
        }

        public virtual IProcessEngine Engine => this.Context.Engine;

        public virtual ExecutionContext SetVariableLocal(string name, object value)
        {
            this.token.SetVariableLocal(name, value);

            return this;
        }

        IExecutionContext IExecutionContext.SetVariable(string name, object value)
            => this.SetVariable(name, value);

        IExecutionContext IExecutionContext.SetVariableLocal(string name, object value)
            => this.SetVariableLocal(name, value);

        public virtual IEvaluator GetEvaluator(string scriptFormat = null)
            => new Internal.JavascriptEvalutor(this);

        /// <summary>
        /// Gets or sets sub-process-instance.
        /// </summary>
        public virtual ProcessInstance SubProcessInstance
        {
            get;
            set;
        }

        public virtual Context Context
        {
            get;
        }

        IContext IExecutionContext.Context => this.Context;

        public virtual IList<Token> JoinedTokens
        {
            get;
            set;
        }

        IReadOnlyList<Token> IExecutionContext.JoinedTokens => new List<Token>(this.JoinedTokens);

        class OutgoingExecution
        {
            private readonly ExecutionContext executionContext;
            private readonly SequenceFlow transition;

            public OutgoingExecution(ExecutionContext executionContext, SequenceFlow transition)
            {
                this.executionContext = executionContext;
                this.transition = transition;
            }

            public virtual bool IsEnded
            {
                get => this.executionContext.Token.IsEnded;
            }

            public virtual void Execute()
            {
                executionContext.Take(this.transition);
            }
        }
    }
}
