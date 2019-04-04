using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Query
{
    public interface IProcessInstanceQuery
    {
        //IProcessInstanceQuery SetPageIndex(int pageIndex);

        //IProcessInstanceQuery SetPagedSize(int pageSize);

        IProcessInstance Single();

        Task<IProcessInstance> SingleAsync();

        int Count();

        int CountAsync();

        IEnumerable<IProcessInstance> List(int pageIndex = 0, int pageSize = 10);

        Task<IEnumerable<IProcessInstance>> ListAsync();
    }
}
