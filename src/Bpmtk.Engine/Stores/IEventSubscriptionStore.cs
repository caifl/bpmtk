using Bpmtk.Engine.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Stores
{
    public interface IEventSubscriptionStore
    {
        IEventSubscriptionQuery CreateQuery();

        EventSubscription Find(int id);

        EventSubscription FindByName(string eventName, string eventType);

        IEnumerable<EventSubscription> GetEventSubscriptionsByProcess(int processDefinitionId);

        void Add(EventSubscription eventSubscription);

        void AddRange(IEnumerable<EventSubscription> eventSubscriptions);

        void Remove(EventSubscription eventSubscription);

        void RemoveRange(IEnumerable<EventSubscription> eventSubscriptions);
    }
}
