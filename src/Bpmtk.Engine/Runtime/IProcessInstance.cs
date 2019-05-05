using System;
using Bpmtk.Engine.Identity;

namespace Bpmtk.Engine.Runtime
{
    public interface IProcessInstance : IExecutionObject
    {
        string Key
        {
            get;
        }

        string EndReason
        {
            get;
        }

        IUser Initiator
        {
            get;
        }
    }
}
