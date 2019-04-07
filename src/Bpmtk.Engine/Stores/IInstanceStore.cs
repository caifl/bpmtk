using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Stores
{
    public interface IInstanceStore
    {
        IActivityInstanceQuery CreateActivityQuery();

        ProcessInstance Find(long id);

        Token FindToken(long id);

        void Add(ProcessInstance processInstance);

        void Add(ActivityInstance activityInstance);

        Task SaveAsync(ProcessInstance processInstance);

        void Remove(ProcessInstance processInstance);

        void Remove(Token token);

        void Add(Token token);

        void Add(HistoricToken historicToken);

        IEnumerable<string> GetActiveActivityIds(long id);

        Task UpdateAsync(ProcessInstance processInstance);

        void Update(Token token);
    }
}
