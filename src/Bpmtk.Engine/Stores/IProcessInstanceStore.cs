using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Stores
{
    public interface IProcessInstanceStore
    {
        ProcessInstance Find(long id);

        void Add(ProcessInstance processInstance);

        Task SaveAsync(ProcessInstance processInstance);

        void Remove(ProcessInstance processInstance);

        void Remove(Token token);

        void Add(Token token);

        void Add(HistoricToken historicToken);

        IEnumerable<string> GetActiveActivityIds(long id);

        Task UpdateAsync(ProcessInstance processInstance);
    }
}
