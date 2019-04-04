using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Events
{
    public interface IEventSubscriptionQuery
    {
        IEventSubscriptionQuery SetProcessDefinitionKey(string key);

        IEventSubscriptionQuery SetProcessDefinitionId(int id);

        IEventSubscriptionQuery SetProcessInstance(int id);

        IEventSubscriptionQuery SetActivityId(string activityId);

        IEventSubscriptionQuery SetEventType(string eventType);

        IEventSubscriptionQuery SetEventName(string eventName);
    }
}
