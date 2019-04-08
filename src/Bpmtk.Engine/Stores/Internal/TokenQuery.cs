using Bpmtk.Engine.Runtime;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bpmtk.Engine.Stores.Internal
{
    class TokenQuery : ITokenQuery
    {
        private readonly ISession session;
        protected long? processInstanceId;

        public TokenQuery(ISession session)
        {
            this.session = session;
        }

        public virtual IList<Token> List()
        {
            return this.session.Query<Token>().Where(x => x.Id == this.processInstanceId).ToList();
        }

        public virtual ITokenQuery SetProcessInstance(long id)
        {
            this.processInstanceId = id;

            return this;
        }
    }
}
