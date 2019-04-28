using System;
using System.Collections.Generic;
using System.Linq;
using SysTasks = System.Threading.Tasks;
using Bpmn2 = Bpmtk.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Expressions;
using Bpmtk.Engine.Scripting;
using Bpmtk.Engine.Tasks;
using Bpmtk.Engine.Utils;
using Bpmtk.Engine.Variables;
using Bpmtk.Engine.Bpmn2.Behaviors;

namespace Bpmtk.Engine.Runtime
{
    public class ExecutionContext
    {
        private Dictionary<string, IVariable> variables = new Dictionary<string, IVariable>();
        private Token token;
        private Bpmtk.Bpmn2.FlowNode transitionSource;
        private Bpmtk.Bpmn2.SequenceFlow transition;


        protected ExecutionContext(Context context, Token token)
        {
            this.Context = context;
            this.token = token;
        }

        public static ExecutionContext Create(Context context, Token token)
        {
            return new ExecutionContext(context, token);
        }

        public virtual async SysTasks.Task StartAsync()
        {
            await this.EnterNodeAsync(this.Node);
        }

        public virtual async SysTasks.Task StartAsync(Bpmtk.Bpmn2.FlowNode initialNode)
        {
            if (initialNode == null)
                throw new ArgumentNullException(nameof(initialNode));

            await this.EnterNodeAsync(initialNode);
        }

        public virtual async SysTasks.Task SignalAsync(string signalEvent, 
            IDictionary<string, object> signalData)
        {
            var behavior = this.Node.Tag as ISignallableActivityBehavior;
            if (behavior != null)
            {
                await behavior.SignalAsync(this, signalEvent, signalData);
                return;
            }

            throw new NotSupportedException();
        }

        public virtual ExecutionContext CreateSubProcessContext()
        {
            var token = this.token.CreateToken();

            return Create(this.Context, token);
        }

        public virtual async SysTasks.Task EndAsync()
        {
            this.token.IsActive = false;

            await this.Context.HistoryManager.RecordActivityEndAsync(this);

            this.FireEvent("completed");
            //var store = context.GetService<IInstanceStore>();
            //store.Add(new HistoricToken(ExecutionContext.Create(context, this), "end"));

            var parentToken = this.token.Parent;
            if (parentToken != null)
            {
                //判断是否在子流程中
                var container = this.token.Node.Container;
                if (container is Bpmtk.Bpmn2.SubProcess)
                {
                    this.token.Remove();

                    if (parentToken.Children.Count > 0)
                        return;

                    var subProcess = container as Bpmtk.Bpmn2.SubProcess;

                    //删除并发Token
                    var p = parentToken;
                    while (!p.Node.Equals(subProcess))
                    {
                        if (p.Children.Count > 0) //还有未完成的并发执行
                            return;

                        p.Remove();
                        p = p.Parent;
                    }

                    var subProcessContext = ExecutionContext.Create(this.Context, p);
                    var behavior = subProcess.Tag as IFlowNodeActivityBehavior;
                    await behavior.LeaveAsync(subProcessContext);
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

            await this.Context.DbSession.FlushAsync();

            var superToken = procInst.Super;
            if(superToken != null)
            {

            }
        }

        public virtual SysTasks.Task<int> GetActiveTaskCountAsync()
        {
            return this.Context.RuntimeManager.GetActiveTaskCountAsync(this.token.Id);
        }

        public virtual IList<Token> GetJoinedTokens()
        {
            var node = this.Node;
            var scope = this.token.ResolveScope();
            if (scope != null)
                return scope.GetInactiveTokensAt(node.Id);

            return this.ProcessInstance.GetInactiveTokensAt(node.Id);
        }

        public virtual ProcessInstance ProcessInstance => this.token.ProcessInstance;

        public virtual Token Token => this.token;

        public virtual Bpmtk.Bpmn2.FlowNode Node
        {
            get
            {
                var node = this.token.Node;
                if(node == null)
                {
                    var processDefinition = this.ProcessInstance.ProcessDefinition;
                    var deploymentId = processDefinition.DeploymentId;
                    var model = this.Context.DeploymentManager.GetBpmnModelAsync(deploymentId).Result;

                    node = model.GetFlowElement(this.token.ActivityId) as Bpmtk.Bpmn2.FlowNode;
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
            //this.token.Scope = oldToken.Scope;
            this.token.ActivityInstance = oldToken.ActivityInstance;

            //re-activate.
            this.token.Activate();
        }

        public virtual Bpmtk.Bpmn2.SequenceFlow Transition
        {
            get => transition;
            set
            {
                this.transition = value;
                this.token.TransitionId = this.transition?.Id;
            }
        }

        public virtual Bpmtk.Bpmn2.FlowNode TransitionSource { get => transitionSource; set => transitionSource = value; }

        public virtual ActivityInstance ActivityInstance
        {
            get => this.token.ActivityInstance;
            set => this.token.ActivityInstance = value;
        }

        protected virtual async SysTasks.Task EnterNodeAsync(Bpmtk.Bpmn2.FlowNode node)
        {
            this.token.Node = node;
            var behavior = node.Tag as IFlowNodeActivityBehavior;
            if (behavior != null)
            {
                var historyManager = this.Context.HistoryManager;

                var joinedTokens = new List<Token>();
                if(!await behavior.CanActivateAsync(this, joinedTokens))
                {
                    if(joinedTokens.Count == 1)
                    {
                        await historyManager.RecordActivityReadyAsync(this, joinedTokens);

                        this.FireEvent("ready");

                        return;
                    }

                    //find created activity-instance.
                    var act = joinedTokens.Where(x => x.ActivityInstance != null)
                        .Select(x => x.ActivityInstance)
                        .FirstOrDefault();
                    if (act != null)
                    {
                        this.token.ActivityInstance = act;
                        await this.Context.DbSession.FlushAsync();
                    }

                    return;
                }
                else
                {
                    if (joinedTokens.Count > 0)
                    {
                        //find created activity-instance.
                        var act = joinedTokens.Where(x => x.ActivityInstance != null)
                            .Select(x => x.ActivityInstance)
                            .FirstOrDefault();
                        if (act != null)
                            this.token.ActivityInstance = act;
                    }
                    else
                    {
                        //fire activityStartEvent.
                        await historyManager.RecordActivityReadyAsync(this, joinedTokens);

                        this.FireEvent("ready");
                    }
                }

                //Clear
                this.Transition = null;
                this.TransitionSource = null;

                //fire activityStartEvent.
                await historyManager.RecordActivityStartAsync(this);

                //executeScript
                this.FireEvent("activated");

                await behavior.ExecuteAsync(this);
                return;
            }

            throw new NotSupportedException();
        }

        protected virtual void FireEvent(string eventName)
        {
            IList<Bpmtk.Bpmn2.Extensions.Script> scripts = null;

            var node = this.Node;
            if (node != null)
                scripts = node.Scripts;
            else
                scripts = this.transition?.Scripts;

            if (scripts != null && scripts.Count > 0)
            {
                var list = scripts.Where(x => x.On.Equals(eventName)).ToList();
                foreach (var item in list)
                    this.ExecutScript(item.Text, item.ScriptFormat);
            }
        }

        public virtual async SysTasks.Task LeaveNodeAsync(Bpmtk.Bpmn2.SequenceFlow transition)
        {
            if (transition == null)
                throw new ArgumentNullException(nameof(transition));

            //fire activityEndEvent.
            var historyManager = this.Context.HistoryManager;
            await historyManager.RecordActivityEndAsync(this);

            this.Transition = transition;

            await this.TakeAsync();
        }

        public virtual async SysTasks.Task LeaveNodeAsync(IEnumerable<Bpmtk.Bpmn2.SequenceFlow> transitions,
            IEnumerable<Token> joinedTokens)
        {
            if (transitions == null)
                throw new ArgumentNullException(nameof(transitions));

            if (joinedTokens == null)
                throw new ArgumentNullException(nameof(joinedTokens));

            var list = joinedTokens.ToList();
            if(list.Count > 0)
                this.Join(list);

            var db = this.Context.DbSession;
            await db.FlushAsync();

            //fire activityEndEvent.
            var historyManager = this.Context.HistoryManager;
            await historyManager.RecordActivityEndAsync(this);

            var childExecutions = new List<ExecutionContext>();

            foreach (var transition in transitions)
            {
                var childToken = this.token.CreateToken();

                var childExecutionContext = new ExecutionContext(this.Context, childToken);
                childExecutionContext.Transition = transition;

                //childExecutionContext.EnterNode(transition.TargetRef);
                childExecutions.Add(childExecutionContext);
            }

            await db.FlushAsync();

            foreach (var execution in childExecutions)
            {
                var childToken = execution.Token;
                if (childToken.IsEnded)
                    continue;

                await execution.TakeAsync();
            }
        }

        public virtual void Join(IList<Token> joinedTokens)
        {
            var scopeToken = this.token.ResolveScope();

            //保留当前token.
            joinedTokens.Remove(token);

            //保留rootToken.
            if(scopeToken != null)
                joinedTokens.Remove(scopeToken);

            //删除其他完成的分支
            Token current = null;
            foreach (var pToken in joinedTokens)
            {
                current = pToken;
                current.Remove();

                //往上遍历
                current = current.Parent;
                while (current.Parent != null
                    && current.Parent.Children.Count == 1)
                {
                    current.Remove();
                    current = current.Parent;
                }
            }

            var parentToken = token.Parent;

            //尝试删除当前分支
            current = token;
            while (current.Parent != null
                && current.Parent.Children.Count == 1)
            {
                current.Remove();
                current = current.Parent;
            }

            if (!current.Equals(token))
                this.ReplaceToken(current);
        }

        protected virtual async SysTasks.Task TakeAsync()
        {
            var targetNode = this.transition.TargetRef;

            //fire transitionTakenEvent.
            this.token.Node = null;

            this.FireEvent("taken");

            await this.EnterNodeAsync(targetNode);
        }

        public virtual object GetVariable(string name)
        {
            return this.token.GetVariable(name)?.GetValue();

            //IVariable variable = null;
            //if (variables.TryGetValue(name, out variable))
            //{
            //    return null;
            //}

            //var current = this.token;

            //do
            //{
            //    variable = current.GetVariable(name);
            //    if (variable != null)
            //    {
            //        this.variables.Add(name, variable);
            //        return null;
            //    }

            //    current = current.Parent;
            //}
            //while (current.Parent != null);

            //variable = this.token.ProcessInstance.GetVariable(name);

            //return variable?.GetValue();
            //ExecutionObject execution = this.ActivityInstance;
            //if (execution != null)
            //    return execution.GetVariable(name);

            //execution = this.Scope;
            //if (execution != null)
            //    return execution.GetVariable(name);

            //return this.ProcessInstance.GetVariable(name);

            //VariableInstance varInst = null;

            //var t = this.token;
            //while (true)
            //{
            //    varInst = t.ActivityInstance.GetVariableInstance(name);
            //    if (varInst != null)
            //        break;

            //    t = t.Parent;
            //    if (t == null)
            //        break;
            //}

            //var p = token.ProcessInstance;
            //varInst = p.GetVariableInstance(name);

            //return varInst?.GetValue();
        }

        public virtual object GetVariableLocal(string name)
        {
            return this.token.GetVariableLocal(name);
        }

        public virtual TValue GetVariableLocal<TValue>(string name)
        {
            var value = this.token.GetVariableLocal(name);
            if (value != null)
                return (TValue)value;

            return default(TValue);
        }

        public virtual TValue GetVariable<TValue>(string name)
        {
            var value = this.token.GetVariable(name);
            if (value != null)
                return (TValue)value;

            return default(TValue);
        }

        public virtual void SetVariable(string name, object value)
        {
            this.token.SetVariable(name, value);
            //ExecutionObject execution = this.ActivityInstance;
            //if (execution != null)
            //{
            //    execution.SetVariable(name, value);
            //    return;
            //}

            //execution = this.Scope;
            //if (execution != null)
            //{
            //    execution.SetVariable(name, value);
            //    return;
            //}

            //this.ProcessInstance.SetVariable(name, value);
        }

        public virtual void SetVariableLocal(string name, object value)
        {
            this.token.SetVariableLocal(name, value);
        }

        //protected IScriptEngine scriptEngine;
        //protected IScriptingScope scriptingScope;

        public virtual object EvaluteExpression(string expression)
        {
            //extract expression.
            expression = StringHelper.ExtractExpression(expression);
            var engine = new JavascriptEngine();
            var scope = engine.CreateScope(new ScriptingContext(this));

            return engine.Execute(expression, scope);
        }

        public virtual IEvaluator GetEvalutor(string scriptFormat = null)
            => new JavascriptEvalutor(this);

        class JavascriptEvalutor : IEvaluator
        {
            private JavascriptEngine engine;
            private IScriptingScope scope;

            public JavascriptEvalutor(ExecutionContext executionContext)
            {
                engine = new JavascriptEngine();
                scope = engine.CreateScope(new ScriptingContext(executionContext));
            }

            public virtual object Evalute(string script)
            {
                script = StringHelper.ExtractExpression(script);
                return engine.Execute(script, scope);
            }

            public virtual string EvaluteToString(string text)
            {
                return System.Text.RegularExpressions.Regex.Replace(text,
                    StringHelper.JuelSearchPattern,
                new System.Text.RegularExpressions.MatchEvaluator((m) =>
                {
                    var expr = m.Value;
                    object value = engine.Execute(expr, scope);
                    if (value != null)
                        return value.ToString();

                    return string.Empty;
                }));
            }
        }

        public virtual object ExecutScript(string script, string scriptFormat)
        {
            var engine = new JavascriptEngine();
            var scope = engine.CreateScope(new ScriptingContext(this));

            return engine.Execute(script, scope);
        }

        public virtual TValue EvaluteExpression<TValue>(string expression)
        {
            var result = this.EvaluteExpression(expression);
            if (result != null)
                return (TValue)result;

            return default(TValue);
        }

        //public virtual ActivityInstance Scope
        //{
        //    get => this.token.Scope;
        //    set => this.token.Scope = value;
        //}

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
    }
}
