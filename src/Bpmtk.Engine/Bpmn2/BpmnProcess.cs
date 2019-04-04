using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Variables;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Expressions;

namespace Bpmtk.Engine.Bpmn2
{
    public class BpmnProcess
    {
        private Definitions definitions;
        private readonly Process process;
        private IDictionary<string, Property> properties;
        private IDictionary<string, DataObject> dataObjects;
        private IDictionary<string, IVariableType> contextSignatures;
        private IDictionary<string, DataInput> dataInputs;
        private IDictionary<string, DataOutput> dataOutputs;

        public BpmnProcess(Process process)
        {
            this.process = process;
            this.definitions = process.Definitions;
        }

        public virtual string Id => this.process.Id;

        protected virtual void Init()
        {
            //this.contextSignatures = new Dictionary<string, IItemAwareElement>();
            //this.argumentsInMetadata = new List<DataInput>();

            //var dataObjects = this.process.FlowElements.OfType<DataObject>();
            //var properties = this.process.Properties;

            //var io = this.process.IOSpecification;
            //if(io != null)
            //{
            //    foreach(var dataInput in io.DataInputs)
            //        this.argumentsInMetadata.Add(dataInput);

            //    foreach (var dataOutput in io.DataOutputs)
            //        this.resultsMetadata.Add(dataOutput);
            //}
        }

        public virtual IList<ScheduledJob> CreateTimers(ProcessDefinition processDefinition)
        {
            var startEvents = this.GetStartEvents();

            string eventType = null;
            string typeName = null;
            List<ScheduledJob> list = new List<ScheduledJob>();
            List<EventDefinition> eventDefinitions = null;
            foreach (var item in startEvents)
            {
                eventDefinitions = new List<EventDefinition>();

                if (item.EventDefinitionRefs.Count > 0)
                    eventDefinitions.AddRange(item.EventDefinitionRefs);

                if (item.EventDefinitions.Count > 0)
                    eventDefinitions.AddRange(item.EventDefinitions);

                if (eventDefinitions.Count == 0)
                    continue;

                foreach (var eventDefinition in eventDefinitions)
                {
                    if (eventDefinition is TimerEventDefinition)
                    {
                        var timerEvent = eventDefinition as TimerEventDefinition;

                        var timeExpr = timerEvent.TimeDate;
                        var cycleExpr = timerEvent.TimeCycle;
                        var durationExpr = timerEvent.TimeDuration;

                        var job = new ScheduledJob();

                        job.Type = "timer";
                        job.Handler = "timer-start-event";
                        job.ActivityId = item.Id;
                        job.ProcessDefinition = processDefinition;
                        job.TenantId = processDefinition.TenantId;

                        var evaluator = Context.Current.GetService<IExpressionEvaluator>();
                        if (timeExpr != null)
                        {
                            var time = evaluator.Evaluate<DateTime>(timeExpr.Text);
                            job.DueDate = time;
                        }
                        else if(cycleExpr != null)
                        {
                            var cycle = evaluator.Evaluate<TimeSpan>(cycleExpr.Text);
                        }
                        else if (durationExpr != null)
                        {
                            var duration = evaluator.Evaluate<DateTime>(durationExpr.Text);
                            job.EndDate = duration;
                        }

                        list.Add(job);

                        continue;
                    }
                }
            }

            return list;
        }

        public virtual IList<EventSubscription> CreateEventSubscriptions(ProcessDefinition processDefinition)
        {
            var startEvents = this.GetStartEvents();

            string eventType = null;
            string typeName = null;
            List<EventSubscription> list = new List<EventSubscription>();
            List<EventDefinition> eventDefinitions = null;
            foreach (var item in startEvents)
            {
                eventDefinitions = new List<EventDefinition>();

                if (item.EventDefinitionRefs.Count > 0)
                    eventDefinitions.AddRange(item.EventDefinitionRefs);

                if (item.EventDefinitions.Count > 0)
                    eventDefinitions.AddRange(item.EventDefinitions);

                if (eventDefinitions.Count == 0)
                    continue;

                foreach (var eventDefinition in eventDefinitions)
                {
                    if (eventDefinition is SignalEventDefinition)
                    {
                        var signalEvent = eventDefinition as SignalEventDefinition;
                        var eventName = signalEvent.SignalRef.Name ?? signalEvent.SignalRef.Id;
                        eventType = "signal";
                        typeName = signalEvent.SignalRef.StructureRef.StructureRef;

                        var eventSub = new EventSubscription();

                        eventSub.EventName = eventName;
                        eventSub.EventType = eventType;
                        eventSub.ActivityId = item.Id;
                        eventSub.ProcessDefinition = processDefinition;

                        list.Add(eventSub);

                        continue;
                    }

                    if (eventDefinition is MessageEventDefinition)
                    {
                        var messageEvent = eventDefinition as MessageEventDefinition;
                        var eventName = messageEvent.MessageRef.Name ?? messageEvent.MessageRef.Id;
                        eventType = "message";
                        typeName = messageEvent.MessageRef.ItemRef.StructureRef;

                        var eventSub = new EventSubscription();

                        eventSub.EventName = eventName;
                        eventSub.EventType = eventType;
                        eventSub.ActivityId = item.Id;
                        eventSub.ProcessDefinition = processDefinition;

                        list.Add(eventSub);

                        continue;
                    }
                }
            }

            return list;
        }

        public virtual IDictionary<string, IEnumerable<EventDefinition>> GetStartEventDefinitions()
        {
            return null;
        }

        public virtual IEnumerable<StartEvent> GetStartEvents()
        {
            return this.process.FlowElements.OfType<StartEvent>();
        }

        public virtual IDictionary<string, IVariableType> GetContextSignatures()
        {
            return this.contextSignatures;
        }

        public virtual void CreateEventSub()
        {
            

            //public virtual IEnumerable<IItemAwareElement> ContextMetdata
            //{
            //    get => this.contextMetadata;
            //}
        }
    }
}
