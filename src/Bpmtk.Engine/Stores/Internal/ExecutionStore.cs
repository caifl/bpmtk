using System;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using Bpmtk.Engine.Runtime;
using System.Collections.Generic;

namespace Bpmtk.Engine.Stores.Internal
{
    public class ProcessInstanceStore
        : IProcessInstanceStore
    {
        private readonly ISession session;

        public ProcessInstanceStore(ISession session)
        {
            this.session = session;
        }

        public virtual void Add(ProcessInstance processInstance)
        {
            this.session.Save(processInstance);
        }

        //public virtual async Task<IEnumerable<ProcessInstance>> GetAsync()
        //{
        //    return await this.session.Query<ProcessInstance>().ToListAsync();
        //}

        public virtual ProcessInstance Find(long id)
        {
            return this.session.Get<ProcessInstance>(id);
        }

        public virtual IEnumerable<string> GetActiveActivityIds(long id)
        {
            return this.session.Query<Token>()
                .Where(x => x.ProcessInstance.Id == id && x.IsActive && x.ActivityId != null)
                .Select(x => x.ActivityId)
                .Distinct()
                .ToList();
        }

        public virtual void Remove(ProcessInstance processInstance)
        {
            this.session.Delete(processInstance);
        }

        public virtual void Remove(Token token)
        {
            this.session.Delete(token);
        }

        public Task UpdateAsync(ProcessInstance processInstance)
            => this.session.UpdateAsync(processInstance);
    }
}
