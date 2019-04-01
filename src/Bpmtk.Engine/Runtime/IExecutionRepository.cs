using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Runtime
{
    public interface IExecutionRepository
    {
        ProcessInstance Find(long id);

        void Add(ProcessInstance processInstance);

        void Remove(ProcessInstance processInstance);
    }
}
