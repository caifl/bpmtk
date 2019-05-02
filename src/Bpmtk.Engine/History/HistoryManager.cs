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
        protected IDbSession db;

        public HistoryManager(Context context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            this.db = context.DbSession;

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

        public virtual IQueryable<ActivityInstance> ActivityInstances => this.db.ActivityInstances;

        public virtual Context Context { get; }

        public virtual IActivityInstanceQuery CreateActivityQuery()
        {
            throw new NotImplementedException();
        }

        public virtual async Task RecordActivityReadyAsync(ExecutionContext executionContext)
        {
            if (this.IsActivityRecorderDisabled)
                return;

            var act = new ActivityInstance();

            //this.Parent = parent;
            //this.variableInstances = new List<ActivityVariable>();
            var node = executionContext.Node;

            act.ProcessInstance = executionContext.ProcessInstance;
            //this.activity = activity;

            act.ActivityId = node.Id;
            act.ActivityType = node.GetType().Name;
            act.TokenId = executionContext.Token.Id;

            //Set initial state.
            act.State = ExecutionState.Ready;
            act.Created = Bpmtk.Engine.Utils.Clock.Now;
            act.LastStateTime = act.Created;

            act.Name = node.Name;
            if (string.IsNullOrEmpty(act.Name))
                act.Name = node.Id;

            if (node.Documentations.Count > 0)
            {
                var textArray = node.Documentations.Select(x => x.Text).ToArray();
                act.Description = Bpmtk.Engine.Utils.StringHelper.Join(textArray, "\n", 255);
            }

            await this.db.SaveAsync(act);
            await this.db.FlushAsync();

            //Set current-act-inst.
            executionContext.ActivityInstance = act;
        }

        public virtual async Task RecordActivityEndAsync(ExecutionContext executionContext)
        {
            if (this.IsActivityRecorderDisabled)
                return;

            var hasChanges = false;
            var act = executionContext.ActivityInstance;
            var date = Clock.Now;
            if (act != null)
            {
                act.State = ExecutionState.Completed;
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
                        act.State = ExecutionState.Completed;
                        act.LastStateTime = date;
                        hasChanges = true;
                    }
                }
            }

            if (hasChanges)
                await this.db.FlushAsync();
        }

        public virtual async Task RecordActivityStartAsync(ExecutionContext executionContext)
        {
            if (this.IsActivityRecorderDisabled)
                return;

            var hasChanges = false;
            var date = Clock.Now;
            var act = executionContext.ActivityInstance;
            if (act != null)
            {
                act.State = ExecutionState.Active;
                act.LastStateTime = date;
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
                        act.LastStateTime = date;
                        hasChanges = true;
                    }
                }
            }

            if(hasChanges)
                await this.db.FlushAsync();
        }

        public Task<IList<ActivityInstance>> GetActivityInstancesAsync(long processInstanceId)
        {
            var q = this.ActivityInstances.Where(x => x.ProcessInstance.Id == processInstanceId);
            return this.db.QueryMultipleAsync(q);
        }
    }
}
