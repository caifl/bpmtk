using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        IQueryable<TEntity> Query<TEntity>(string sql, params object[] parameters) where TEntity : class;

        IQueryable<TEntity> Query<TEntity>() where TEntity : class;

        IQueryable<TEntity> Fetch<TEntity, TProperty>(IQueryable<TEntity> query,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class;

        Task<TEntity> QuerySingleAsync<TEntity>(IQueryable<TEntity> query);

        Task<IList<TEntity>> QueryMultipleAsync<TEntity>(IQueryable<TEntity> query);

        Task<int> CountAsync<TEntity>(IQueryable<TEntity> query);

        Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
            where TEntity : class;

        TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;

        void Update(object entity);

        void Save(object entity);

        void SaveRange(IEnumerable<object> items);

        Task SaveAsync(object entity);

        Task SaveRangeAsync(IEnumerable<object> items);

        void Remove(object entity);

        void RemoveRange(IEnumerable<object> items);

        ITransaction BeginTransaction();

        ITransaction BeginTransaction(System.Data.IsolationLevel isolationLevel);

        void Flush();

        Task FlushAsync();
    }
}
