using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using NHibernate;

namespace Bpmtk.Engine.Stores.Internal
{
    public class IdentityStore : IIdentityStore
    {
        private readonly ISession session;

        public IdentityStore(ISession session)
        {
            this.session = session;
        }

        public void Add(User user)
        {
            this.session.Save(user);
        }
    }
}
