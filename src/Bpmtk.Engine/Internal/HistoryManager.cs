using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Internal
{
    public class HistoryManager : IHistoryManager
    {
        protected IDbSession db;

        public HistoryManager(Context context)
        {
            Context = context;
            this.db = context.DbSession;
        }

        public virtual IQueryable<ActivityInstance> ActivityInstances => this.db.ActivityInstances;

        public virtual Context Context { get; }

        public Task CreateActivityInstanceAsync(ActivityInstance activityInstance)
        {
            throw new NotImplementedException();
        }

        public IActivityInstanceQuery CreateActivityQuery()
        {
            throw new NotImplementedException();
        }

        public virtual async Task RecordActivityReadyAsync(ExecutionContext executionContext, IList<Token> joinedTokens)
        {
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
            var act = executionContext.ActivityInstance;
            if(act != null)
            {
                act.Finish();
                await this.db.FlushAsync();
            }
        }

        public virtual async Task RecordActivityStartAsync(ExecutionContext executionContext)
        {
            var act = executionContext.ActivityInstance;
            if (act != null)
            {
                act.Activate();
                await this.db.FlushAsync();
            }
        }
    }
}
