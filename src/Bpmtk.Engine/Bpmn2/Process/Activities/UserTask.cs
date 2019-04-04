using System;
using System.Collections.Generic;
using Bpmtk.Engine.Bpmn2.Extensions;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine.Bpmn2
{
    public class UserTask : Task
    {
        protected List<Rendering> renderings = new List<Rendering>();
        //protected List<FormField> formProperties = new List<FormField>();

        public UserTask()
        {
            this.Implementation = "##unspecified";
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public virtual IList<Rendering> Renderings => this.renderings;

        //public virtual IList<FormField> FormProperties => this.formProperties;

        public virtual string Implementation
        {
            get;
            set;
        }

        #region human-task extended attributes.

        /// <summary>
        /// Gets or sets task name.
        /// </summary>
        public virtual Expression TaskName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets task priority.
        /// </summary>
        public virtual TaskPriority Priority
        {
            get;
            set;
        }

        /// <summary>
        /// The default task assignee.(formalExpression)
        /// </summary>
        public virtual Expression Assignee
        {
            get;
            set;
        }

        /// <summary>
        /// The task assignment strategy.
        /// </summary>
        public virtual string AssignmentStrategy
        {
            get;
            set;
        }

        /// <summary>
        /// The task duration.
        /// </summary>
        public virtual Expression Duration
        {
            get;
            set;
        }

        #endregion

        public override void Execute(ExecutionContext executionContext)
        {
            //var context = executionContext.Context;
            //var builder = context.GetService<ITaskInstanceBuilder>();
            //builder.SetUserTask(this)
            //    .Build(executionContext);

            base.Leave(executionContext);
        }

        public override void Signal(ExecutionContext executionContext, 
            string signalName, string signalData)
        {
            //check if all task-instances completed.
            var token = executionContext.Token;
            var context = executionContext.Context;
            var store = context.GetService<ITaskStore>();
            var count = store.GetActiveTaskCount(token.Id);
            if (count > 0)
                throw new RuntimeException($"There are '{count}' tasks need to be completed.");

            base.Leave(executionContext);
        }
    }
}
