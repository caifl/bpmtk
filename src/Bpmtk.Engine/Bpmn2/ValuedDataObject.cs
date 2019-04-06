using System;
using System.Linq;
using Bpmtk.Engine.Bpmn2.Types;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public class ValuedDataObject : DataObject
    {
        private ITypeHandler typeHandler;

        public ValuedDataObject()
        {
            this.Values = new List<string>();
        }

        public virtual void Initialize(BpmnTypes types)
        {
            typeHandler = types.Get(this.TypeName);
        }

        public virtual string TypeName
        {
            get;
            set;
        }

        public virtual Type ClrType
        {
            get => this.typeHandler.ClrType;
        }

        public virtual IList<string> Values
        {
            get;
        }

        public virtual object Value
        {
            get
            {
                var values = this.Values.ToArray();
                return this.typeHandler.Parse(values, this.IsCollection);
            }
        }
    }
}
