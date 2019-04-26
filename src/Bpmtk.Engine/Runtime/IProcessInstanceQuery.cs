using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Runtime
{
    public interface IProcessInstanceQuery
    {
        //IProcessInstanceQuery SetPageIndex(int pageIndex);

        //IProcessInstanceQuery SetPagedSize(int pageSize);

        ProcessInstance Single();

        Task<ProcessInstance> SingleAsync();

        int Count();

        int CountAsync();

        IEnumerable<ProcessInstance> List(int pageIndex = 0, int pageSize = 10);

        Task<IEnumerable<ProcessInstance>> ListAsync();
    }
}
