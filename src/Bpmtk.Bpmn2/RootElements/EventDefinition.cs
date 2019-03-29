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

        public virtual string ActivityRef
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
        public virtual string ErrorRef
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
        public virtual string OperationRef
        {
            get;
            set;
        }

        public virtual string MessageRef
        {
            get;
            set;
        }
    }

    public class SignalEventDefinition : EventDefinition
    {
        public virtual string SignalRef
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
        public virtual Expression Expression
        {
            get;
            set;
        }

        public virtual TimeExpressionType ExpressionType
        {
            get;
            set;
        }
    }

    public enum TimeExpressionType
    {
        /// <remarks/>
        TimeCycle,

        /// <remarks/>
        TimeDate,

        /// <remarks/>
        TimeDuration,
    }
}
