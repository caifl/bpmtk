using System;

namespace Bpmtk.Engine.Models
{
    public class ScheduledJob
    {
        public virtual long Id
        {
            get;
            set;
        }

        public virtual string TenantId
        {
            get;
            set;
        }

        public virtual string Key
        {
            get;
            set;
        }

        public virtual int Retries
        {
            get;
            set;
        }

        //public virtual int MaxRetries
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// timer or message.
        /// </summary>
        public virtual string Type
        {
            get;
            set;
        }

        /// <summary>
        /// timer-start-event
        /// </summary>
        public virtual string Handler
        {
            get;
            set;
        }

        public virtual DateTime? DueDate
        {
            get;
            set;
        }

        public virtual DateTime? EndDate
        {
            get;
            set;
        }

        public virtual ProcessDefinition ProcessDefinition
        {
            get;
            set;
        }

        public virtual string ActivityId
        {
            get;
            set;
        }

        public virtual ProcessInstance ProcessInstance
        {
            get;
            set;
        }

        public virtual string Message
        {
            get;
            set;
        }

        public virtual string StackTrace
        {
            get;
            set;
        }

        public virtual Token Token
        {
            get;
            set;
        }

        public virtual TaskInstance Task
        {
            get;
            set;
        }

        public virtual DateTime Created
        {
            get;
            set;
        }

        public virtual string Options
        {
            get;
            set;
        }
    }
}
