using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Identity;
using Bpmtk.Engine.Models;
using NHibernate;
using NHibernate.Linq;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine
{
    public class IdentityStore //: IIdentityStore
    {
        private readonly ISession session;

        public IdentityStore(ISession session)
        {
            this.session = session;
            var q = session.QueryOver<ProcessInstance>()
                .Fetch(SelectMode.Fetch, x => x.Initiator)
                .List();
         
        }

        public void Add(User user)
        {
            this.session.Save(user);
        }

        public User FindUserByName(string name)
        {
            return this.session.Query<User>().Where(x => x.Name == name).SingleOrDefault();
        }
    }
}
