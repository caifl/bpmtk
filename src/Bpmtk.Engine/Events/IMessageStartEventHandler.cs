using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Events
{
    public interface IMessageStartEventHandler
    {
        IProcessInstance Execute(
            IContext context,
            EventSubscription eventSubscription,
            IDictionary<string, object> messasgeData);
    }
}
