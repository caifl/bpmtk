using System;

namespace Bpmtk.Bpmn2
{
    public interface IItemAwareElement
    {
        string Id
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }

        string ItemSubjectRef
        {
            get;
            set;
        }

        bool IsCollection
        {
            get;
            set;
        }

        DataState DataState
        {
            get;
            set;
        }
    }
}
