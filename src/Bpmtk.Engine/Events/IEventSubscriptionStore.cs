using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Events
{
    public interface IEventSubscriptionStore
    {
        IQueryable<EventSubscription> EventSubscriptions
        {
            get;
        }
        //IEventSubscriptionQuery CreateQuery();

        Task<EventSubscription> FindByIdAsync(int id);

        Task<EventSubscription> FindByNameAsync(string eventName, string eventType);

        IList<EventSubscription> GetByProcessDefinitionAsync(int processDefinitionId);

        Task CreateAsync(params EventSubscription[] eventSubscriptions);

        Task RemoveAsync(params EventSubscription[] eventSubscriptions);
    }
}
