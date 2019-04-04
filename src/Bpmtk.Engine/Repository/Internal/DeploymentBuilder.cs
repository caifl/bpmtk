using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Scheduler;
using Bpmtk.Engine.Stores;

namespace Bpmtk.Engine.Repository.Internal
{
    public class DeploymentBuilder : IDeploymentBuilder
    {
        private readonly IDeploymentStore deployments;
        private readonly IEventSubscriptionStore eventSubscriptions;
        private readonly IScheduledJobStore scheduledJobStore;
        private byte[] modelData;
        protected string name;
        protected string category;
        protected string memo;
        protected string tenantId;
        protected bool disableModelValidations;
        protected Package package;
        protected DateTime? validFrom;
        protected DateTime? validTo;

        public DeploymentBuilder(IDeploymentStore deployments,
            IEventSubscriptionStore eventSubscriptions,
            IScheduledJobStore scheduledJobStore)
        {
            this.deployments = deployments;
            this.eventSubscriptions = eventSubscriptions;
            this.scheduledJobStore = scheduledJobStore;
        }

        public virtual IDeployment Build()
        {
            var model = BpmnModel.FromBytes(this.modelData, this.disableModelValidations);
            var processes = model.Processes;
            if (processes.Count() == 0)
                throw new BpmnError("The BPMN model does not contains any processes.");

            var keys = processes.Select(x => x.Id).ToArray();
            var prevProcessDefinitions = this.deployments.GetProcessDefinitionLatestVersionsAsync(keys).Result;

            //New deployment.
            var deployment = new Deployment(this.name, this.modelData);
            deployment.Category = this.category;
            deployment.Memo = this.memo;
            deployment.TenantId = this.tenantId;
            deployment.Package = this.package;

            this.deployments.Add(deployment);

            ProcessDefinition prevProcessDefinition = null;

            foreach (var bpmnProcess in processes)
            {
                prevProcessDefinition = null;
                if(prevProcessDefinitions.Count > 0)
                    prevProcessDefinitions.TryGetValue(bpmnProcess.Id, out prevProcessDefinition);

                var hasDiagram = model.HasDiagram(bpmnProcess.Id);

                this.CreateProcessDefinition(deployment, bpmnProcess, hasDiagram, prevProcessDefinition);
            }

            return deployment;
        }

        #region Create signal/message/timer event handler.

        protected virtual EventSubscription CreateSignalEventSubscription(ProcessDefinition processDefinition,
            StartEvent startEvent,
            Signal signal)
        {
            var eventSub = new EventSubscription();

            eventSub.TenantId = this.tenantId;
            eventSub.EventType = "signal";
            eventSub.EventName = signal.Name ?? signal.Id;
            eventSub.ActivityId = startEvent.Id;
            eventSub.ProcessDefinition = processDefinition;

            return eventSub;
        }

        protected virtual EventSubscription CreateMessageEventSubscription(ProcessDefinition processDefinition, StartEvent startEvent,
            Message message)
        {
            var eventSub = new EventSubscription();

            eventSub.TenantId = this.tenantId;
            eventSub.EventType = "message";
            eventSub.EventName = message.Name ?? message.Id;
            eventSub.ActivityId = startEvent.Id;
            eventSub.ProcessDefinition = processDefinition;

            return eventSub;
        }

        protected virtual ScheduledJob CreateTimerJob(ProcessDefinition processDefinition,
            StartEvent startEvent,
            TimerEventDefinition timerEvent)
        {
            ScheduledJob job = new ScheduledJob();

            job.ActivityId = startEvent.Id;
            job.Key = Guid.NewGuid().ToString("n");
            job.ProcessDefinition = processDefinition;
            job.TenantId = this.tenantId;
            job.Handler = "timerStartEvent";

            return job;
        }

        #endregion

        protected virtual ProcessDefinition CreateProcessDefinition(Deployment deployment, Process bpmnProcess, bool hasDiagram,
            ProcessDefinition prevProcessDefinition
            )
        {
            int version = 1;
            if (prevProcessDefinition != null)
                version = prevProcessDefinition.Version + 1;

            var processDefinition = new ProcessDefinition(deployment, bpmnProcess, hasDiagram, version);
            processDefinition.ValidFrom = this.validFrom;
            processDefinition.ValidTo = this.validTo;
            processDefinition.VerifyState();

            deployment.Add(processDefinition);

            var startEvents = bpmnProcess.FlowElements.OfType<StartEvent>().ToList();
            List<EventDefinition> eventDefinitions = null;
            EventSubscription eventSub = null;

            var eventSubs = new List<EventSubscription>();
            var timerJobs = new List<ScheduledJob>();

            foreach (var startEvent in startEvents)
            {
                if (startEvent.EventDefinitionRefs.Count == 0
                    && startEvent.EventDefinitions.Count == 0)
                    continue;

                eventDefinitions = new List<EventDefinition>(startEvent.EventDefinitionRefs);
                if (startEvent.EventDefinitions.Count > 0)
                    eventDefinitions.AddRange(startEvent.EventDefinitions);

                foreach (var eventDefinition in eventDefinitions)
                {
                    if (eventDefinition is SignalEventDefinition)
                    {
                        var signalEvent = eventDefinition as SignalEventDefinition;
                        eventSub = this.CreateSignalEventSubscription(processDefinition, 
                            startEvent, signalEvent.SignalRef);
                        eventSubs.Add(eventSub);
                        continue;
                    }

                    if (eventDefinition is MessageEventDefinition)
                    {
                        var messageEvent = eventDefinition as MessageEventDefinition;
                        var message = messageEvent.MessageRef;

                        eventSub = this.CreateMessageEventSubscription(processDefinition, startEvent, message);
                        eventSubs.Add(eventSub);
                        continue;
                    }

                    if (eventDefinition is TimerEventDefinition)
                    {
                        var timerEvent = eventDefinition as TimerEventDefinition;

                        var timerJob = this.CreateTimerJob(processDefinition, startEvent, timerEvent);
                        timerJobs.Add(timerJob);
                        continue;
                    }
                }
            }

            if (prevProcessDefinition != null)
            {
                var procDefId = prevProcessDefinition.Id;

                //remove event subs.
                var items = this.eventSubscriptions.GetEventSubscriptionsByProcess(procDefId);
                if (items.Count() > 0)
                    this.eventSubscriptions.RemoveRange(items);

                //remove timer jobs.
                var jobs = this.scheduledJobStore.GetScheduledJobsByProcess(procDefId);
                if (jobs.Count() > 0)
                    this.scheduledJobStore.RemoveRange(jobs);
            }

            if (eventSubs.Count > 0)
                this.eventSubscriptions.AddRange(eventSubs);

            if (timerJobs.Count > 0)
                this.scheduledJobStore.AddRange(timerJobs);
            
            return processDefinition;
        }

        public Task<IDeployment> BuildAsync()
        {
            throw new NotImplementedException();
        }

        public virtual IDeploymentBuilder SetBpmnModel(byte[] modelData)
        {
            if (modelData == null)
                throw new ArgumentNullException(nameof(modelData));

            this.modelData = modelData;

            return this;
        }

        public virtual IDeploymentBuilder SetPackage(Package package)
        {
            this.package = package;

            return this;
        }

        public virtual IDeploymentBuilder SetCategory(string category)
        {
            this.category = category;

            return this;
        }

        public virtual IDeploymentBuilder SetMemo(string memo)
        {
            this.memo = memo;

            return this;
        }

        public virtual IDeploymentBuilder SetName(string name)
        {
            this.name = name;

            return this;
        }
    }
}
