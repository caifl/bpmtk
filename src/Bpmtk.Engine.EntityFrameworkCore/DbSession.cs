using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bpmtk.Engine.Models;
using System.Linq.Expressions;

namespace Bpmtk.Engine.Storage
{
    public class DbSession : IDbSession
    {
        private readonly BpmDbContext context;

        public DbSession(BpmDbContext context)
        {
            this.context = context;
        }

        public virtual IQueryable<ProcessDefinition> ProcessDefinitions => this.context.ProcessDefinitions;

        public virtual IQueryable<EventSubscription> EventSubscriptions => this.context.EventSubscriptions;

        public virtual IQueryable<ProcessInstance> ProcessInstances => this.context.ProcessInstances;

        public virtual IQueryable<Token> Tokens => this.context.Tokens;

        public virtual IQueryable<TaskInstance> Tasks => this.context.Tasks;

        public virtual IQueryable<User> Users => this.context.Users;

        public virtual IQueryable<Deployment> Deployments => this.context.Deployments;

        public virtual IQueryable<ScheduledJob> ScheduledJobs => this.context.ScheduledJobs;

        public virtual IQueryable<IdentityLink> IdentityLinks => this.context.IdentityLinks;

        public virtual IQueryable<Group> Groups => this.context.Groups;

        public virtual IQueryable<ActivityInstance> ActivityInstances => this.context.ActivityInstances;

        public virtual ITransaction BeginTransaction()
        {
            var tx = this.context.Database.BeginTransaction();
            return new ContextTransaction(tx);
        }

        public virtual IQueryable<TEntity> Fetch<TEntity, TProperty>(IQueryable<TEntity> query,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            return query.Include(navigationPropertyPath);
        }

        public virtual async Task<ITransaction> BeginTransactionAsync()
        {
            var tx = await this.context.Database.BeginTransactionAsync();
            return new ContextTransaction(tx);
        }

        public virtual Task<int> CountAsync<TEntity>(IQueryable<TEntity> query)
            => query.CountAsync();

        public virtual Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
            => this.context.FindAsync<TEntity>(keyValues);

        public virtual Task FlushAsync() => this.context.SaveChangesAsync();

        public async virtual Task<IList<TEntity>> QueryMultipleAsync<TEntity>(IQueryable<TEntity> query)
            => await query.ToListAsync();

        public virtual Task<TEntity> QuerySingleAsync<TEntity>(IQueryable<TEntity> query)
            => query.SingleOrDefaultAsync();

        public virtual Task RemoveAsync(object entity)
        {
            this.context.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual Task RemoveRangeAsync(IEnumerable<object> items)
        {
            //this.context.AttachRange(items);
            this.context.RemoveRange(items);
            return Task.CompletedTask;
        }

        public virtual async Task SaveAsync(object entity)
        {
            await this.context.AddAsync(entity);
            //await this.context.SaveChangesAsync();
        }

        public virtual async Task SaveRangeAsync(IEnumerable<object> items)
        {
            await this.context.AddRangeAsync(items);
            //await this.context.SaveChangesAsync();
        }

        public virtual Task UpdateAsync(object entity)
        {
            this.context.Update(entity);

            return Task.CompletedTask;
        }

        public virtual IQueryable<TEntity> Query<TEntity>() where TEntity : class
            => this.context.Set<TEntity>();
    }
}
