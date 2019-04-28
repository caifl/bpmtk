using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Storage
{
    public interface IDbSession
    {
        IQueryable<Deployment> Deployments
        {
            get;
        }

        IQueryable<ProcessDefinition> ProcessDefinitions
        {
            get;
        }

        IQueryable<EventSubscription> EventSubscriptions
        {
            get;
        }

        IQueryable<ScheduledJob> ScheduledJobs
        {
            get;
        }

        IQueryable<ProcessInstance> ProcessInstances
        {
            get;
        }

        IQueryable<Token> Tokens
        {
            get;
        }

        IQueryable<TaskInstance> Tasks
        {
            get;
        }

        IQueryable<ActivityInstance> ActivityInstances
        {
            get;
        }

        IQueryable<IdentityLink> IdentityLinks
        {
            get;
        }

        IQueryable<User> Users
        {
            get;
        }

        IQueryable<Group> Groups
        {
            get;
        }

        Task<TEntity> QuerySingleAsync<TEntity>(IQueryable<TEntity> query);

        Task<IList<TEntity>> QueryMultipleAsync<TEntity>(IQueryable<TEntity> query);

        Task<int> CountAsync<TEntity>(IQueryable<TEntity> query);

        Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
            where TEntity : class;


        Task UpdateAsync(object entity);

        Task SaveAsync(object entity);

        Task SaveRangeAsync(IEnumerable<object> items);

        Task RemoveAsync(object entity);

        Task RemoveRangeAsync(IEnumerable<object> items);

        ITransaction BeginTransaction();

        Task FlushAsync();
    }
}
