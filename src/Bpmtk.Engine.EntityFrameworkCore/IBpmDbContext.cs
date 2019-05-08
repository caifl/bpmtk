using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine
{
    public interface IBpmDbContext : IDisposable
    {
        DatabaseFacade Database
        {
            get;
        }

        DbSet<Deployment> Deployments
        {
            get;
        }

        DbSet<ProcessDefinition> ProcessDefinitions
        {
            get;
        }

        DbSet<ProcessInstance> ProcessInstances
        {
            get;
        }

        DbSet<Token> Tokens
        {
            get;
        }

        DbSet<Variable> Variables
        {
            get;
        }

        DbSet<ActivityVariable> ActivityVariables
        {
            get;
        }

        DbSet<ActivityInstance> ActivityInstances
        {
            get;
        }

        DbSet<TaskInstance> Tasks
        {
            get;
        }

        DbSet<EventSubscription> EventSubscriptions
        {
            get;
        }

        DbSet<ScheduledJob> ScheduledJobs
        {
            get;
        }

        DbSet<IdentityLink> IdentityLinks
        {
            get;
        }

        DbSet<User> Users
        {
            get;
        }

        DbSet<Group> Groups
        {
            get;
        }

        DbSet<UserGroup> UserGroups
        {
            get;
        }

        DbSet<Comment> Comments
        {
            get;
        }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        object Find(Type entityType, params object[] keyValues);

        TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;

        Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;

        void Remove(object entity);

        void RemoveRange(params object[] items);

        void RemoveRange(IEnumerable<object> items);

        void Add(object entity);

        void AddRange(params object[] items);

        void Update(object entity);

        Task AddAsync(object entity);

        Task AddRangeAsync(IEnumerable<object> entity);

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
