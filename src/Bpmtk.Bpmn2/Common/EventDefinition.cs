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
        public virtual Expression TimeDate
        {
            get;
            set;
        }

        public virtual Expression TimeDuration
        {
            get;
            set;
        }

        public virtual Expression TimeCycle
        {
            get;
            set;
        }

        //public virtual Expression Expression
        //{
        //    get;
        //    set;
        //}

        //public virtual TimeExpressionType ExpressionType
        //{
        //    get;
        //    set;
        //}
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
