
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore;
using Bpmtk.Engine.Cfg;

namespace Bpmtk.Engine
{
    public class BpmDbContext : DbContext
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

        public virtual DbSet<User> Users
        {
            get;
            set;
        }

        public virtual DbSet<Bpmtk.Engine.Models.Group> Groups
        {
            get;
            set;
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            



            modelBuilder.ApplyConfiguration(new ByteArrayConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityLinkConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupConfiguration());

            modelBuilder.ApplyConfiguration(new PackageConfiguration());
            modelBuilder.ApplyConfiguration(new DeploymentConfiguration());
            modelBuilder.ApplyConfiguration(new ProcessDefinitionConfiguration());
            modelBuilder.ApplyConfiguration(new ProcessInstanceConfiguration());
            modelBuilder.ApplyConfiguration(new TokenConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityInstanceConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityVariableConfiguration());
            modelBuilder.ApplyConfiguration(new VariableConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());

            modelBuilder.ApplyConfiguration(new ScheduledJobConfiguration());
            modelBuilder.ApplyConfiguration(new EventSubscriptionConfiguration());

            base.OnModelCreating(modelBuilder);

            //var modelTypes = modelBuilder.Model.GetEntityTypes();
            //foreach (var type in modelTypes)
            //{
            //    var props = type.GetProperties();
            //    var typeBuilder = modelBuilder.Entity(type.Name);
            //    foreach (var prop in props)
            //    {
            //        typeBuilder.Property(prop.Name).ApplyNamingStrategy();
            //    }
            //}

            //foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    entityType.Relational().TableName = entityType.;
            //}
        }
    }
}
