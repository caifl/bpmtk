using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class Import : BaseElement
    {
        public string Namespace
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public string ImportType
        {
            get;
            set;
        }
    }
}
