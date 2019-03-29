using System;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine
{
    public interface ITaskService
    {
        ITaskInstance Find(long id);


    }
}
