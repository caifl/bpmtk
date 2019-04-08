using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Stores;

namespace Bpmtk.Engine.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityStore identityStore;

        public IdentityService(IIdentityStore identityStore)
        {
            this.identityStore = identityStore;
        }

        public void CreateUser(User user)
        {
            this.identityStore.Add(user);
        }

        public virtual User FindUserByName(string name)
        {
            return this.identityStore.FindUserByName(name);
        }
    }
}
