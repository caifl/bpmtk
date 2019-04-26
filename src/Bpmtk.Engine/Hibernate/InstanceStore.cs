using System;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using Bpmtk.Engine.Runtime;
using System.Collections.Generic;
using Bpmtk.Engine.Models;
using NHibernate.Linq;

namespace Bpmtk.Engine.Hibernate
{
    public class InstanceStore
        : IInstanceStore
    {
        private readonly ISession session;

        public InstanceStore(ISession session)
        {
            this.session = session;
        }

        public IQueryable<ProcessInstance> ProcessInstances => this.session.Query<ProcessInstance>();

        public IQueryable<Token> Tokens => this.session.Query<Token>();

        public virtual void Add(ProcessInstance processInstance)
        {
            this.session.Save(processInstance);
        }

        public void Add(HistoricToken historicToken)
        {
            this.session.Save(historicToken);
        }

        public void Add(Token token)
        {
            this.session.Save(token);
        }

        public void Add(ActivityInstance activityInstance)
        {
            this.session.Save(activityInstance);
        }

        //public IActivityInstanceQuery CreateActivityQuery()
        //{
        //    return new ActivityInstanceQuery(this.session);
        //}

        public Task CreateAsync(ProcessInstance processInstance)
        {
            throw new NotImplementedException();
        }

        //public virtual ITokenQuery CreateTokenQuery()
        //{
        //    return new TokenQuery(this.session);
        //}

        //public virtual async Task<IEnumerable<ProcessInstance>> GetAsync()
        //{
        //    return await this.session.Query<ProcessInstance>().ToListAsync();
        //}

        public virtual ProcessInstance Find(long id)
        {
            return this.session.Get<ProcessInstance>(id);
        }

        public Task<ProcessInstance> FindAsync(long processInstanceId)
        {
            throw new NotImplementedException();
        }

        public virtual Token FindToken(long id)
        {
            return this.session.Get<Token>(id);
        }

        public virtual async Task<IList<string>> GetActiveActivityIdsAsync(long processInstanceId)
        {
            return await this.session.Query<Token>()
                .Where(x => x.ProcessInstance.Id == processInstanceId && x.IsActive && x.ActivityId != null)
                .Select(x => x.ActivityId)
                .Distinct()
                .ToListAsync();
        }

        public Task<int> GetActiveTaskCountAsync(long tokenId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ProcessVariable>> GetVariablesAsync(long processInstanceId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ProcessVariable>> GetVariablesAsync(long processInstanceId, params string[] names)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(ProcessInstance processInstance)
        {
            this.session.Delete(processInstance);
        }

        public virtual void Remove(Token token)
        {
            this.session.Delete(token);
        }

        public Task RemoveAsync(ProcessInstance processInstance)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(ProcessInstance processInstance)
        {
            return this.session.SaveAsync(processInstance);
        }

        public Task UpdateAsync(ProcessInstance processInstance)
            => this.session.UpdateAsync(processInstance);

        public Task UpdateAsync(Token token)
        {
            throw new NotImplementedException();
        }
    }
}
