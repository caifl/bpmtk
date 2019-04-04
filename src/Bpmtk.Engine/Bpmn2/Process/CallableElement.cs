using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public abstract class CallableElement : RootElement
    {
        protected List<Interface> supportedInterfaceRefs = new List<Interface>();
        protected List<InputOutputBinding> ioBindings = new List<InputOutputBinding>();

        public virtual IList<Interface> SupportedInterfaceRefs => this.supportedInterfaceRefs;

        public virtual InputOutputSpecification IOSpecification
        {
            get;
            set;
        }

        public virtual IList<InputOutputBinding> IOBindings => this.ioBindings;

        public virtual string Name
        {
            get;
            set;
        }
    }

    public class InputOutputBinding : BaseElement
    {
        public virtual Operation OperationRef
        {
            get;
            set;
        }

        public virtual InputSet InputDataRef
        {
            get;
            set;
        }

        public virtual OutputSet OutputDataRef
        {
            get;
            set;
        }
    }
}
