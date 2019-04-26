using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public abstract class EventDefinition : RootElement
    {
        
    }

    public class CancelEventDefinition : EventDefinition
    {
    }
    
    public class CompensateEventDefinition : EventDefinition
    {
        public virtual bool? WaitForCompletion
        {
            get;
            set;
        }

        public virtual Activity ActivityRef
        {
            get;
            set;
        }
    }

    public class ConditionalEventDefinition : EventDefinition
    {
        public virtual Expression Condition
        {
            get;
            set;
        }
    }

    public class ErrorEventDefinition : EventDefinition
    {
        public virtual Error ErrorRef
        {
            get;
            set;
        }
    }

    public class EscalationEventDefinition : EventDefinition
    {
        public virtual string EscalationRef
        {
            get;
            set;
        }
    }

    public class LinkEventDefinition : EventDefinition
    {
        private List<string> source = new List<string>();

        public virtual IList<string> Source
        {
            get
            {
                return this.source;
            }
        }

        public virtual string Target
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }
    }

    public class MessageEventDefinition : EventDefinition
    {
        public virtual Operation OperationRef
        {
            get;
            set;
        }

        public virtual Message MessageRef
        {
            get;
            set;
        }
    }

    public class SignalEventDefinition : EventDefinition
    {
        public virtual Signal SignalRef
        {
            get;
            set;
        }
    }

    public class TerminateEventDefinition : EventDefinition
    {
    }

    public class TimerEventDefinition : EventDefinition
    {
        /// <summary>
        /// 在给定的确切时间点触发(一次). (2019-04-03T16:14:10)
        /// </summary>
        public virtual Expression TimeDate
        {
            get;
            set;
        }

        /// <summary>
        /// 在上次事件被触发执行完成后的时间间隔后触发(循环).
        /// </summary>
        public virtual Expression TimeDuration
        {
            get;
            set;
        }

        /// <summary>
        /// 每间隔给定时间后触发(循环).
        /// </summary>
        public virtual Expression TimeCycle
        {
            get;
            set;
        }
    }

    //public enum TimeExpressionType
    //{
    //    /// <remarks/>
    //    TimeCycle,

    //    /// <remarks/>
    //    TimeDate,

    //    /// <remarks/>
    //    TimeDuration,
    //}
}
