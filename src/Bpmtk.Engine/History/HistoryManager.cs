using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Storage;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.History
{
    public class HistoryManager : IHistoryManager
    {
        private const string KeyIsActivityRecorderDisabled = "IsActivityRecorderDisabled";
        private const string KeyIsTokenRecorderEnabled = "IsTokenRecorderEnabled";
        protected IDbSession session;

        public HistoryManager(Context context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            this.session = context.DbSession;

            var engine = context.Engine;

            this.IsTokenRecorderEnabled = engine.GetValue<bool>(KeyIsTokenRecorderEnabled, false);
            this.IsActivityRecorderDisabled = engine.GetValue<bool>(KeyIsActivityRecorderDisabled, false);
        }

        protected virtual bool IsActivityRecorderDisabled
        {
            get;
        }

        public virtual bool IsTokenRecorderEnabled
        {
            get;
        }

        public virtual IQueryable<ActivityInstance> ActivityInstances => this.session.ActivityInstances;

        public virtual Context Context { get; }

        public virtual IActivityInstanceQuery CreateActivityQuery()
            => new ActivityInstanceQuery(this.session);

        public virtual void RecordProcessStart(ExecutionContext executionContext)
        {
            //this.RecordActivityReady(executionContext);
        }

        public virtual void RecordProcessEnd(ExecutionContext executionContext)
        {
            //this.RecordActivityReady(executionContext);
        }

        public virtual void RecordActivityReady(ExecutionContext executionContext)
        {
            if (this.IsActivityRecorderDisabled)
                return;

            var token = executionContext.Token;

            var act = new ActivityInstance();

            //init
            act.Variables = new List<ActivityVariable>();
            act.IdentityLinks = new List<IdentityLink>();
            //act.Children = new List<ActivityInstance>();

            //this.Parent = parent;
            //this.variableInstances = new List<ActivityVariable>();
            var node = executionContext.Node;
            var parentToken = token.Parent;
            if(parentToken != null && parentToken.IsMIRoot)
            {
                act.Parent = parentToken.ActivityInstance;
            }      
            else if(node.Container is Bpmtk.Bpmn2.SubProcess)//Check if nested.
            {
                //find parent activity-instance.
                var scope = token.ResolveScope();
                if (scope == null)
                    throw new RuntimeException("Can't resolve scope token.");

                act.Parent = scope.ActivityInstance;
            }

            act.ProcessInstance = executionContext.ProcessInstance;
            //this.activity = activity;

            act.ActivityId = node.Id;
            act.ActivityType = node.GetType().Name;
            act.TokenId = token.Id;

            var date = Clock.Now;

            //Set initial state.
            act.State = ExecutionState.Ready;
            act.Created = date;
            act.LastStateTime = date;

            act.Name = node.Name;
            if (string.IsNullOrEmpty(act.Name))
                act.Name = node.Id;

            if (node.Documentations.Count > 0)
            {
                var textArray = node.Documentations.Select(x => x.Text).ToArray();
                act.Description = StringHelper.Join(textArray, "\n", 255);
            }

            //Initialize context.
            var variables = token.Variables;
            foreach(var variable in variables)
            {
                act.SetVariable(variable.Name, variable.GetValue());
            }

            this.session.Save(act);
            this.session.Flush();

            //Set current-act-inst.
            executionContext.ActivityInstance = act;
        }

        public virtual void RecordActivityEnd(ExecutionContext executionContext)
        {
            if (this.IsActivityRecorderDisabled)
                return;

            var hasChanges = false;
            var act = executionContext.ActivityInstance;
            var date = Clock.Now;
            if (act != null)
            {
                act.State = ExecutionState.Completed;
                act.Modified = date;
                act.LastStateTime = date;
                hasChanges = true;
            }

            var joinedTokens = executionContext.JoinedTokens;
            if (joinedTokens != null && joinedTokens.Count > 0)
            {
                foreach (var token in joinedTokens)
                {
                    act = token.ActivityInstance;
                    if (act != null)
                    {
                        act.Modified = date;
                        act.State = ExecutionState.Completed;
                        act.LastStateTime = date;
                        hasChanges = true;
                    }
                }
            }

            if (hasChanges)
                this.session.Flush();
        }

        public virtual void RecordActivityStart(ExecutionContext executionContext)
        {
            if (this.IsActivityRecorderDisabled)
                return;

            var hasChanges = false;
            var date = Clock.Now;
            var act = executionContext.ActivityInstance;
            if (act != null)
            {
                act.State = ExecutionState.Active;
                act.StartTime = date;
                act.Modified = date;
                act.LastStateTime = date;

                var isMIRoot = executionContext.Token.IsMIRoot;
                if(isMIRoot)
                    act.IsMIRoot = isMIRoot;

                hasChanges = true;
            }

            var joinedTokens = executionContext.JoinedTokens;
            if(joinedTokens != null && joinedTokens.Count > 0)
            {
                foreach(var token in joinedTokens)
                {
                    act = token.ActivityInstance;
                    if(act != null)
                    {
                        act.State = ExecutionState.Active;
                        act.StartTime = date;
                        act.Modified = date;
                        act.LastStateTime = date;
                        hasChanges = true;
                    }
                }
            }

            if(hasChanges)
                this.session.Flush();
        }

        public virtual IList<ActivityInstance> GetActivityInstances(long processInstanceId)
        {
            return this.ActivityInstances.Where(x => x.ProcessInstance.Id == processInstanceId)
                .OrderByDescending(x => x.Created)
                .ToList();
        }

        public virtual System.Threading.Tasks.Task<IList<ActivityInstance>> GetActivityInstancesAsync(long processInstanceId)
        {
            var query = this.session.ActivityInstances.Where(x => x.ProcessInstance.Id == processInstanceId)
                .OrderByDescending(x => x.Created);

            return this.session.QueryMultipleAsync(query);
        }
    }
}
