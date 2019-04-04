using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Events;
using NHibernate;

namespace Bpmtk.Engine.Stores.Internal
{
    class EventSubscriptionStore : IEventSubscriptionStore
    {
        public EventSubscriptionStore(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; }

        public void Add(EventSubscription eventSubscription)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<EventSubscription> eventSubscriptions)
        {
            throw new NotImplementedException();
        }

        public IEventSubscriptionQuery CreateQuery()
        {
            throw new NotImplementedException();
        }

        public EventSubscription Find(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventSubscription> GetEventSubscriptionsByProcess(int processDefinitionId)
        {
            throw new NotImplementedException();
        }

        public void Remove(EventSubscription eventSubscription)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<EventSubscription> eventSubscriptions)
        {
            throw new NotImplementedException();
        }
    }
}
