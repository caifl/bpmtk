﻿using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public abstract class CallableElement : RootElement
    {
        protected List<string> supportedInterfaceRefs = new List<string>();
        protected List<InputOutputBinding> ioBindings = new List<InputOutputBinding>();

        public virtual IList<string> SupportedInterfaceRefs => this.supportedInterfaceRefs;

        public virtual IOSpecification IOSpecification
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
        public string OperationRef
        {
            get;
            set;
        }

        public string InputDataRef
        {
            get;
            set;
        }

        public string OutputDataRef
        {
            get;
            set;
        }
    }
}