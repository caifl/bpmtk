using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Events;
using NHibernate;
using System.Threading.Tasks;
using NHibernate.Linq;

namespace Bpmtk.Engine
{
    public class EventSubscriptionStore : IEventSubscriptionStore
    {
        public EventSubscriptionStore(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; }

        public virtual IQueryable<EventSubscription> EventSubscriptions => this.Session.Query<EventSubscription>();

        public virtual void AddRange(IEnumerable<EventSubscription> eventSubscriptions)
        {
            foreach (var item in eventSubscriptions)
                this.Session.Save(item);
        }

        public virtual async Task CreateAsync(params EventSubscription[] eventSubscriptions)
        {
            foreach (var entity in eventSubscriptions)
                await this.Session.SaveAsync(entity);
        }

        public IEventSubscriptionQuery CreateQuery()
        {
            throw new NotImplementedException();
        }

        public virtual Task<EventSubscription> FindByIdAsync(int id)
            => this.Session.GetAsync<EventSubscription>(id);

        public virtual Task<EventSubscription> FindByNameAsync(string eventName, string eventType)
        {
            return this.Session.Query<EventSubscription>()
                .Where(x => x.EventName == eventName && x.EventType == eventType)
                .SingleOrDefaultAsync();
        }

        public virtual async Task<IList<EventSubscription>> GetByProcessDefinitionAsync(int processDefinitionId)
        {
            return await this.Session.Query<EventSubscription>()
                .Where(x => x.ProcessDefinition.Id == processDefinitionId)
                .ToListAsync();
        }

        public virtual async Task RemoveAsync(params EventSubscription[] eventSubscriptions)
        {
            foreach (var entity in eventSubscriptions)
                await this.Session.DeleteAsync(entity);
        }
    }
}
