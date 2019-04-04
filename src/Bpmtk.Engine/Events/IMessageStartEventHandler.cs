using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Events
{
    public interface IMessageStartEventHandler
    {
        Task<ProcessInstance> Execute(EventSubscription eventSubscription,
            object messageData);
    }
}
