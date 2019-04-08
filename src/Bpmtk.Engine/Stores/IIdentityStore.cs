using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Stores
{
    public interface IIdentityStore
    {
        void Add(User user);
    }
}
