using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Events
{
    public interface IMessageStartEventHandler
    {
        Task<ProcessInstance> ExecuteAsync(
            IContext context,
            EventSubscription eventSubscription,
            IDictionary<string, object> messageData);
    }
}
