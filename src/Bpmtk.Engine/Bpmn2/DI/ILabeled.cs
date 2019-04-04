using System;

namespace Bpmtk.Engine.Bpmn2.DI
{
    public interface ILabeled
    {
        BPMNLabel BpmnLabel
        {
            get;
            set;
        }
    }
}
