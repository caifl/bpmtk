using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Scheduler;
using Bpmtk.Engine.Storage;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Repository
{
    public class DeploymentBuilder : IDeploymentBuilder
    {
        private readonly IDbSession session;
        private readonly Context context;
        private readonly DeploymentManager deploymentManager;
        private byte[] modelData;
        protected string tanentId;
        protected string name;
        protected string category;
        protected string memo;
        protected string tenantId;
        protected bool disableModelValidations;
        protected int? packageId;
        protected DateTime? validFrom;
        protected DateTime? validTo;

        public DeploymentBuilder(Context context, DeploymentManager deploymentManager)
        {
            this.context = context;
            this.session = context.DbSession;
            this.deploymentManager = deploymentManager;
        }

        //protected virtual async Task<IDictionary<string, ProcessDefinition>> GetLatestVersionsAsync(params string[] processDefinitionKeys)
        //{
        //    var keys = processDefinitionKeys;
        //    var query = this.session.ProcessDefinitions
        //        .Where(x => keys.Contains(x.Key))
        //        .GroupBy(x => x.Key)
        //        .Select(x => new
        //        {
        //            Key = x.Key,
        //            Version = x.OrderByDescending(y => y.Version).FirstOrDefault()
        //        });

        //    var results = this.session.QueryMultipleAsync(query);

        //    Dictionary<string, ProcessDefinition> map = new Dictionary<string, ProcessDefinition>();
        //    foreach (var item in results)
        //    {
        //        map.Add(item.Key, item.Version);
        //    }

        //    return map;
        //}

        //protected virtual async Task<IDictionary<string, ProcessDefinition>> GetLatestVersionsAsync(params string[] processDefinitionKeys)
        //{
        //    var keys = processDefinitionKeys;
        //    var query = this.session.ProcessDefinitions
        //        .Where(x => keys.Contains(x.Key))
        //        .GroupBy(x => x.Key)
        //        .Select(x => new
        //        {
        //            Key = x.Key,
        //            Version = x.OrderByDescending(y => y.Version).FirstOrDefault()
        //        });

        //    var results = this.session.QueryMultipleAsync(query);

        //    Dictionary<string, ProcessDefinition> map = new Dictionary<string, ProcessDefinition>();
        //    foreach (var item in results)
        //    {
        //        map.Add(item.Key, item.Version);
        //    }

        //    return map;
        //}

        IDeployment IDeploymentBuilder.Build() => this.Build();

        async Task<IDeployment> IDeploymentBuilder.BuildAsync() => await this.BuildAsync();

        public virtual Deployment Build()
        {
            return this.BuildAsync().Result;
        }

        public virtual async Task<Deployment> BuildAsync()
        {
            var model = BpmnModel.FromBytes(this.modelData, this.disableModelValidations);
            var processes = model.Processes;
            if (processes.Count == 0)
                throw new DeploymentException("The BPMN model does not contains any processes.");

            var keys = processes.Select(x => x.Id);

            var prevProcessDefinitions = await this.deploymentManager.CreateDefinitionQuery()
                .FetchLatestVersionOnly()
                .SetKeyAny(keys)
                .ListAsync();

            var prevProcessDefinitionMap = prevProcessDefinitions.ToDictionary(x => x.Key);

            //New deployment.
            var deployment = new Deployment();
            deployment.Name = name;
            deployment.Model = new ByteArray(modelData);
            deployment.Created = Clock.Now;
            deployment.Category = this.category;
            deployment.Memo = this.memo;
            deployment.TenantId = this.tenantId;

            ProcessDefinition prevProcessDefinition = null;

            foreach (var bpmnProcess in processes)
            {
                if (!bpmnProcess.IsExecutable)
                    throw new DeploymentException($"The process '{bpmnProcess.Id}' is not executabe.");

                prevProcessDefinition = null;
                if(prevProcessDefinitionMap.Count > 0)
                    prevProcessDefinitionMap.TryGetValue(bpmnProcess.Id, out prevProcessDefinition);

                var procDef = this.CreateProcessDefinition(deployment, bpmnProcess, prevProcessDefinition);
                procDef.HasDiagram = model.HasDiagram(bpmnProcess.Id);

                deployment.ProcessDefinitions.Add(procDef);

                this.InitializeEventsAndScheduledJobs(procDef, bpmnProcess, prevProcessDefinition);
            }

            await this.session.SaveAsync(deployment);
            await this.session.FlushAsync();

            return deployment;
        }

        protected virtual void InitializeEventsAndScheduledJobs(
            ProcessDefinition processDefinition,
            Process bpmnProcess,
            ProcessDefinition prevProcessDefinition)
        {
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
                var items = this.deploymentManager.GetEventSubscriptions(procDefId);
                if (items.Count > 0)
                    this.session.RemoveRange(items);

                //remove timer jobs.
                var jobs = this.deploymentManager.GetScheduledJobs(procDefId);
                if (jobs.Count > 0)
                    this.session.RemoveRange(jobs);
            }

            if (eventSubs.Count > 0)
                this.session.SaveRange(eventSubs);

            if (timerJobs.Count > 0)
                this.session.SaveRange(timerJobs);
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

        protected virtual ProcessDefinition CreateProcessDefinition(Deployment deployment, Process bpmnProcess,
            ProcessDefinition prevProcessDefinition
            )
        {
            int version = 1;
            if (prevProcessDefinition != null)
                version = prevProcessDefinition.Version + 1;

            var procDef = new ProcessDefinition();
           
            procDef.Deployment = deployment;
            procDef.TenantId = deployment.TenantId;
            procDef.Version = version;

            procDef.Key = bpmnProcess.Id;
            procDef.Name = StringHelper.Get(bpmnProcess.Name, 100, bpmnProcess.Id);
            procDef.State = ProcessDefinitionState.Active;
            procDef.Created = deployment.Created;
            procDef.Modified = procDef.Created;
            procDef.Category = deployment.Category;

            if (bpmnProcess.Documentations.Count > 0)
            {
                var textArray = bpmnProcess.Documentations.Select(x => x.Text).ToArray();
                procDef.Description = StringHelper.Join(textArray, "\n", 100);
            }

            procDef.ValidFrom = this.validFrom;
            procDef.ValidTo = this.validTo;
            procDef.VerifyState();

            return procDef;
        }

        public virtual IDeploymentBuilder SetBpmnModel(byte[] modelData)
        {
            if (modelData == null)
                throw new ArgumentNullException(nameof(modelData));

            this.modelData = modelData;

            return this;
        }

        public virtual IDeploymentBuilder SetTanentId(string tanentId)
        {
            this.tanentId = tanentId;

            return this;
        }

        public virtual IDeploymentBuilder SetPackageId(int packageId)
        {
            this.packageId = packageId;

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
