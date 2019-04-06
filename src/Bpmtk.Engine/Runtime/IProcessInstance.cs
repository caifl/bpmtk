using System;

namespace Bpmtk.Engine.Runtime
{
    public interface IProcessInstance
    {
        long Id
        {
            get;
        }

        string Name
        {
            get;
        }

        ExecutionState State
        {
            get;
        }

        object GetVariable(string name);
    }
}
