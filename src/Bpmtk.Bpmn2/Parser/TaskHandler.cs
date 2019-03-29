using System;
using System.Xml.Linq;
using Bpmtk.Bpmn2.Extensions;

namespace Bpmtk.Bpmn2.Parser
{
    abstract class TaskHandler<TFlowElementContainer, TTask> : ActivityHandler<TFlowElementContainer, TTask>
        where TFlowElementContainer : IFlowElementsContainer
        where TTask : Task
    {
    }

    //task
    class TaskHandler<TFlowElementContainer> : TaskHandler<TFlowElementContainer, Task>
        where TFlowElementContainer : IFlowElementsContainer
    {
        protected override Task New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateTask();
    }

    //manualTask
    class ManualTaskHandler<TFlowElementContainer> : TaskHandler<TFlowElementContainer, ManualTask>
        where TFlowElementContainer : IFlowElementsContainer
    {
        protected override ManualTask New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateManualTask();
    }

    //userTask
    class UserTaskHandler<TFlowElementContainer> : TaskHandler<TFlowElementContainer, UserTask>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public override UserTask Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var userTask = base.Create(parent, context, element);

            userTask.TaskName = element.GetExtendedAttribute("taskName");
            userTask.AssignmentStrategy = element.GetExtendedAttribute("assignmentStrategy");
            userTask.Assignee = element.GetExtendedAttribute("assignee");
            
            var value = element.GetExtendedAttribute("priority");
            if (value != null)
                userTask.Priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), value);

            return userTask;
        }

        protected override UserTask New(IParseContext context, XElement element) => context.BpmnFactory.CreateUserTask();
    }

    //scriptTask
    class ScriptTaskHandler<TFlowElementContainer> : TaskHandler<TFlowElementContainer, ScriptTask>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public override ScriptTask Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var task = base.Create(parent, context, element);

            task.ScriptFormat = element.GetAttribute("scriptFormat");
            task.Script = element.Value;

            return task;
        }

        protected override ScriptTask New(IParseContext context, XElement element) => context.BpmnFactory.CreateScriptTask();
    }

    //serviceTask
    class ServiceTaskHandler<TFlowElementContainer> : TaskHandler<TFlowElementContainer, ServiceTask>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public override ServiceTask Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var task = base.Create(parent, context, element);

            task.Implementation = element.GetAttribute("implementation");
            task.OperationRef = element.GetAttribute("operationRef");

            return task;
        }

        protected override ServiceTask New(IParseContext context, XElement element) => context.BpmnFactory.CreateServiceTask();
    }

    //sendTask
    class SendTaskHandler<TFlowElementContainer> : TaskHandler<TFlowElementContainer, SendTask>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public override SendTask Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var task = base.Create(parent, context, element);

            task.Implementation = element.GetAttribute("implementation");
            task.OperationRef = element.GetAttribute("operationRef");
            task.MessageRef = element.GetAttribute("messageRef");

            return task;
        }

        protected override SendTask New(IParseContext context, XElement element) => context.BpmnFactory.CreateSendTask();
    }

    //receiveTask
    class ReceiveTaskHandler<TFlowElementContainer> : TaskHandler<TFlowElementContainer, ReceiveTask>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public override ReceiveTask Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var task = base.Create(parent, context, element);

            task.Implementation = element.GetAttribute("implementation");
            task.OperationRef = element.GetAttribute("operationRef");
            task.MessageRef = element.GetAttribute("messageRef");

            //Instantiate

            return task;
        }

        protected override ReceiveTask New(IParseContext context, XElement element) => context.BpmnFactory.CreateReceiveTask();
    }

    //businessRuleTask
    class BusinessRuleTaskHandler<TFlowElementContainer> : TaskHandler<TFlowElementContainer, BusinessRuleTask>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public override BusinessRuleTask Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var task = base.Create(parent, context, element);

            task.Implementation = element.GetAttribute("implementation");

            return task;
        }

        protected override BusinessRuleTask New(IParseContext context, XElement element) => context.BpmnFactory.CreateBusinessRuleTask();
    }
}
