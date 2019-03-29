using System;
using System.Collections.Generic;
using Bpmtk.Bpmn2.Extensions;

namespace Bpmtk.Bpmn2
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

        /// <summary>
        /// Gets or sets task name.
        /// </summary>
        public virtual string TaskName
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
        public virtual string Assignee
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
        public virtual string Duration
        {
            get;
            set;
        }
    }
}
