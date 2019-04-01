using System;
using System.Collections.Generic;
using Bpmtk.Engine.Bpmn2;

namespace Bpmtk.Engine.Runtime
{
    public class Token
    {
        private ProcessInstance processInstance;
        private ActivityInstance activityInstance;
        private BpmnActivity activity;

        //private bool isActive;
        //private string activityId;
        //private bool isSuspended;
        //private bool isLoopActivity;
        private Token parent;
        private ICollection<Token> children;

        protected Token()
        {}

        protected Token(Token parent)
        {
            this.parent = parent;
        }

        public virtual long Id
        {
            get;
            protected set;
        }

        public virtual string ActivityId
        {
            get;
            protected set;
        }

        public virtual Token Parent
        {
            get => this.parent;
        }

        public virtual void Activate()
        {
            this.IsActive = true;
        }

        public virtual void Inactivate()
        {
            this.IsActive = false;
        }

        public virtual void Remove()
        {
            if (this.parent != null)
                this.parent.children.Remove(this);
        }

        public virtual IEnumerable<Token> Children
        {
            get;
        }

        public Token(ProcessInstance processInstance, BpmnActivity initialActivity)
        {
            this.processInstance = processInstance ?? throw new ArgumentNullException(nameof(processInstance));
            this.activity = initialActivity ?? throw new ArgumentNullException(nameof(initialActivity));
            this.ActivateActivity();
        }

        protected virtual void ActivateActivity()
        {
            this.activityInstance = new ActivityInstance(this);
        }

        public virtual BpmnActivity Activity
        {
            get => this.activity;
            set
            {
                this.activity = value;
            }
        }

        public virtual bool IsActive
        {
            get;
            protected set;
        }

        public virtual ActivityInstance ActivityInstance
        {
            get => this.activityInstance;
        }

        public virtual ProcessInstance ProcessInstance { get => processInstance; }

        public virtual Token CreateToken()
        {
            return new Token(this);
        }

        //protected virtual void ExecuteLoopActivity(StandardLoopCharacteristics loopCharacteristics)
        //{

        //}

        //protected virtual void ExecuteMultiInstanceActivity(MultiInstanceLoopCharacteristics loopCharacteristics)
        //{

        //}

        //protected virtual void ExecuteSubProcess()
        //{

        //}

        protected IList<Token> GetJoinedTokens()
        {
            return null;
        }

        //protected virtual bool ExecuteParallelGateway()
        //{
        //    this.isActive = false;

        //    var joinedTokens = this.GetJoinedTokens();
        //    if(joinedTokens.Count >= this.activiy.Incomings.Count)
        //    {
        //        //Continue ..
        //        var act = joinedTokens.Where(x => x.activityInstance != null)
        //            .Select(x => x.activityInstance)
        //            .FirstOrDefault();

        //        if (act == null)
        //            act = new ActivityInstance();

        //        this.ActivateNode();

        //        //Fork
        //        var list = new List<Token>();

        //        foreach (var outgoing in this.activiy.Outgoings)
        //            list.Add(this.CreateToken());

        //        foreach (var t in list)
        //            t.Take(null);

        //        return true;
        //    }
        //    else
        //    {
        //        //Waiting tokens.
        //        return false;
        //    }
        //}

        //protected virtual void Take(SequenceFlow transition)
        //{

        //}

        //public virtual Token CreateToken()
        //{
        //    return null;
        //}

        //protected virtual void ActivateNode()
        //{
        //    this.activityInstance.Activate();
        //}

        //public virtual void EnterNode(FlowNode node)
        //{
        //    this.activiy = node;
        //    //this.isActive = true;

        //    if (node is ParallelGateway)
        //    {
        //        this.ExecuteParallelGateway();
        //        return;
        //    }

        //    if (node is Activity && ((Activity)node).LoopCharacteristics != null)
        //    {
        //        var activity = node as Activity;
        //        var loopCharacteristics = activity.LoopCharacteristics;
        //        this.isLoopActivity = true;

        //        if (loopCharacteristics is MultiInstanceLoopCharacteristics)
        //        {
        //            var multiInstance = loopCharacteristics as MultiInstanceLoopCharacteristics;
        //            this.ExecuteMultiInstanceActivity(multiInstance);
        //        }
        //        else
        //        {
        //            var loop = loopCharacteristics as StandardLoopCharacteristics;
        //            this.ExecuteLoopActivity(loop);
        //        }

        //        return;
        //    }

        //    if(node is SubProcess)
        //    {
        //        this.ExecuteSubProcess();
        //        return;
        //    }

        //    if(node is ScriptTask)
        //    {
        //        this.ExecuteScriptTask();
        //        return;
        //    }

        //    var context = new ExecutionContext(this);
            
        //}

        //public virtual void Take(SequenceFlow transition)
        //{

        //}

        //protected virtual void ExecuteScriptTask()
        //{

        //}

        public virtual void End()
        {

        }

        /// <summary>
        /// Sends a signal to this token. leaves the current {@link #getNode() node} over the default transition
        /// </summary>
        public virtual void Signal()
        {
            if (this.activity == null)
                throw new RuntimeException(this + " is not positioned in a node");

            //var defaultTransition = node.getDefaultLeavingTransition();
            //if (defaultTransition == null)
            //{
            //    throw new JbpmException(node + " has no default transition");
            //}

            //this.Signal(defaultTransition, new ExecutionContext(this));
        }

        public virtual void Signal(string signalName, object signalData)
        {
            this.activity.Signal(new ExecutionContext(this), signalName,
                signalData);
        }

        public virtual bool IsSuspended
        {
            get;
            protected set;
        }

        public virtual bool IsLoopActivity
        {
            get;
            protected set;
        }

        public virtual void Resume()
        {
            this.IsSuspended = false;
        }

        public virtual void Suspend()
        {
            this.IsSuspended = false;
        }
    }
}
