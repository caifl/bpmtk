using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine
{
    public interface IIdentityService
    {
        void CreateUser(User user);
    }
}
