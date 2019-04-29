using System;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Events
{
    public interface ITaskEventListener
    {
        Task CreatedAsync();

        Task AssignedAsync();

        Task CompletedAsync();
    }
}
