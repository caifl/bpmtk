
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage
{
    public class BpmDbContext : DbContext, IBpmDbContext
    {
        public BpmDbContext(DbContextOptions options) : base(options)
        {

        }

        #region Properties

        public virtual DbSet<Deployment> Deployments
        {
            get;
            set;
        }

        public virtual DbSet<ProcessDefinition> ProcessDefinitions
        {
            get;
            set;
        }

        public virtual DbSet<ProcessInstance> ProcessInstances
        {
            get;
            set;
        }

        public virtual DbSet<Token> Tokens
        {
            get;
            set;
        }

        public virtual DbSet<Variable> Variables
        {
            get;
            set;
        }

        public virtual DbSet<ActivityVariable> ActivityVariables
        {
            get;
            set;
        }

        public virtual DbSet<ActivityInstance> ActivityInstances
        {
            get;
            set;
        }

        public virtual DbSet<TaskInstance> Tasks
        {
            get;
            set;
        }

        public virtual DbSet<EventSubscription> EventSubscriptions
        {
            get;
            set;
        }

        public virtual DbSet<ScheduledJob> ScheduledJobs
        {
            get;
            set;
        }

        public virtual DbSet<IdentityLink> IdentityLinks
        {
            get;
            set;
        }

        public virtual DbSet<User> Users
        {
            get;
            set;
        }

        public virtual DbSet<Group> Groups
        {
            get;
            set;
        }

        public virtual DbSet<UserGroup> UserGroups
        {
            get;
            set;
        }

        public virtual DbSet<Comment> Comments
        {
            get;
            set;
        }

        public Task AddRangeAsync(IEnumerable<object> entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyBpmtkModelConfigurations();

            base.OnModelCreating(modelBuilder);
        }

        Task IBpmDbContext.AddAsync(object entity) => this.AddAsync(entity);

        void IBpmDbContext.Add(object entity) => this.Add(entity);

        void IBpmDbContext.Remove(object entity) => this.Remove(entity);

        void IBpmDbContext.Update(object entity) => this.Update(entity);
    }
}
