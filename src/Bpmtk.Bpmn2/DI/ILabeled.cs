using System;

namespace Bpmtk.Bpmn2.DI
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
