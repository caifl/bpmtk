using System;
using System.Collections.Generic;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Runtime
{
    public abstract class VariableInstance : IVariable, IValueFields
    {
        protected IVariableType type;

        protected VariableInstance()
        { }

        public VariableInstance(string name, 
            object value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var type = VariableType.Resolve(value);
            type.SetValue(this, value);

            this.type = type;
            this.Name = name;
            this.TypeName = type.Name;
        }

        public virtual long Id
        {
            get;
            protected set;
        }

        public virtual string Name
        {
            get;
            protected set;
        }

        public virtual string TypeName
        {
            get;
            protected set;
        }

        protected long? longVal;
        protected string text;
        protected string text2;
        protected double? doubleVal;
        protected ByteArray byteArray;

        public virtual ByteArray ByteArray
        {
            get;
            protected set;
        }

        public virtual object GetValue()
        {
            this.EnsureTypeInitialized();

            return this.type.GetValue(this);
        }

        public virtual void SetValue(object value)
        {
            this.EnsureTypeInitialized();

            this.type.SetValue(this, value);
        }

        protected virtual void EnsureTypeInitialized()
        {
            if(this.type == null)
            {
                this.type = VariableType.Get(this.TypeName);
            }
        }

        IVariableType IVariable.Type { get => this.type; }

        string IValueFields.Text
        {
            get => this.text;
            set => this.text = value;
        }

        string IValueFields.Text2
        {
            get => this.text2; set => this.text2 = value;
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
                if (this.byteArray == null)
                {
                    this.byteArray = new ByteArray(value);
                    return;
                }

                this.byteArray.Value = value;
            }
        }

        long? IValueFields.LongValue
        {
            get => this.longVal;
            set => this.longVal = value;
        }

        double? IValueFields.DoubleValue
        {
            get => this.doubleVal;
            set => this.doubleVal = value;
        }
    }
}
