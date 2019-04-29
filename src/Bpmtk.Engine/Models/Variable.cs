using System;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Models
{
    public class Variable : IValueFields, IVariable
    {
        public virtual ProcessInstance ProcessInstance
        {
            get;
            set;
        }

        public virtual long Id
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Type
        {
            get;
            set;
        }

        public virtual ByteArray ByteArray
        {
            get;
            set;
        }

        public virtual string Text
        {
            get;
            set;
        }

        public virtual string Text2
        {
            get;
            set;
        }

        object IValueFields.CachedValue
        {
            get;
            set;
        }

        byte[] IValueFields.Bytes
        {
            get
            {
                return this.ByteArray?.Value;
            }
            set
            {
                this.ByteArray = new ByteArray(value);
            }
        }

        public virtual long? LongValue
        {
            get;
            set;
        }

        public virtual double? DoubleValue
        {
            get;
            set;
        }

        private IVariableType typeHandler = null;

        protected virtual void EnsureInitialized()
        {
            if (this.typeHandler != null)
                return;

            this.typeHandler = VariableType.Get(this.Type);
        }

        public virtual object GetValue()
        {
            this.EnsureInitialized();

            return this.typeHandler.GetValue(this);
        }

        public virtual void SetValue(object value)
        {
            this.EnsureInitialized();

            this.typeHandler.SetValue(this, value);
        }
    }
}
