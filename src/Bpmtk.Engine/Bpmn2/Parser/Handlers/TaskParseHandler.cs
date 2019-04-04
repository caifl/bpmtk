using System;
using System.Xml.Linq;
using Bpmtk.Engine.Bpmn2.Extensions;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    class TaskParseHandler : ActivityParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var task = context.BpmnFactory.CreateTask();
            parent.FlowElements.Add(task);

            base.Init(task, context, element);

            return task;
        }
    }

    //task
    //class TaskHandler<TFlowElementContainer> : TaskHandler<TFlowElementContainer, Task>
    //    where TFlowElementContainer : IFlowElementsContainer
    //{
    //    protected override Task New(IParseContext context, XElement element)
    //        => context.BpmnFactory.CreateTask();
    //}

    //manualTask
    class ManualTaskParseHandler : TaskParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var task = context.BpmnFactory.CreateManualTask();
            parent.FlowElements.Add(task);

            return task;
        }
    }

    //userTask
    class UserTaskParseHandler : TaskParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var userTask = context.BpmnFactory.CreateUserTask();
            parent.FlowElements.Add(userTask);
            //userTask.TaskName = element.GetExtendedAttribute("taskName");
            //userTask.AssignmentStrategy = element.GetExtendedAttribute("assignmentStrategy");
            //userTask.Assignee = element.GetExtendedAttribute("assignee");
            
            var value = element.GetExtendedAttribute("priority");
            if (value != null)
                userTask.Priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), value);

            base.Init(userTask, context, element);

            return userTask;
        }
    }

    //scriptTask
    class ScriptTaskParseHandler : TaskParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var task = context.BpmnFactory.CreateScriptTask();
            parent.FlowElements.Add(task);

            task.ScriptFormat = element.GetAttribute("scriptFormat");
            task.Script = element.Value;

            base.Init(task, context, element);

            return task;
        }
    }

    //serviceTask
    class ServiceTaskParseHandler : TaskParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var task = context.BpmnFactory.CreateServiceTask();
            parent.FlowElements.Add(task);

            task.Implementation = element.GetAttribute("implementation");

            var operationRef = element.GetAttribute("operationRef");
            if (operationRef != null)
                context.AddReferenceRequest(operationRef, (Operation operation) => task.OperationRef = operation);

            base.Init(task, context, element);

            return task;
        }
    }

    //sendTask
    class SendTaskParseHandler : TaskParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var task = context.BpmnFactory.CreateSendTask();
            parent.FlowElements.Add(task);

            task.Implementation = element.GetAttribute("implementation");

            var operationRef = element.GetAttribute("operationRef");
            if (operationRef != null)
                context.AddReferenceRequest(operationRef, (Operation operation) => task.OperationRef = operation);

            var messageRef = element.GetAttribute("messageRef");
            if (messageRef != null)
                context.AddReferenceRequest(messageRef, (Message message) => task.MessageRef = message);

            base.Init(task, context, element);

            return task;
        }
    }

    //receiveTask
    class ReceiveTaskParseHandler : TaskParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var task = context.BpmnFactory.CreateReceiveTask();
            parent.FlowElements.Add(task);

            task.Implementation = element.GetAttribute("implementation");

            var operationRef = element.GetAttribute("operationRef");
            if (operationRef != null)
                context.AddReferenceRequest(operationRef, (Operation operation) => task.OperationRef = operation);

            var messageRef = element.GetAttribute("messageRef");
            if (messageRef != null)
                context.AddReferenceRequest(messageRef, (Message message) => task.MessageRef = message);
            //Instantiate

            base.Init(task, context, element);

            return task;
        }
    }

    //businessRuleTask
    class BusinessRuleTaskParseHandler : TaskParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var task = context.BpmnFactory.CreateBusinessRuleTask();
            parent.FlowElements.Add(task);

            task.Implementation = element.GetAttribute("implementation");

            base.Init(task, context, element);

            return task;
        }
    }
}
