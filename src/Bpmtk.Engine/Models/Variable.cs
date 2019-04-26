using System;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Models
{
    public class Variable : IValueFields, IVariable
    {
        //protected IVariableType type;

        //protected Variable()
        //{ }

        //public Variable(string name, 
        //    object value)
        //{
        //    if (name == null)
        //        throw new ArgumentNullException(nameof(name));

        //    var type = VariableType.Resolve(value);
        //    type.SetValue(this, value);

        //    this.type = type;
        //    this.Name = name;
        //    this.TypeName = type.Name;
        //}


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

        //protected long? longVal;
        //protected string text;
        //protected string text2;
        //protected double? doubleVal;
        //protected ByteArray byteArray;

        //public virtual long? ByteArrayId
        //{
        //    get;
        //    set;
        //}

        public virtual ByteArray ByteArray
        {
            get;
            set;
        }

        //public virtual object GetValue()
        //{
        //    this.EnsureTypeInitialized();

        //    return this.type.GetValue(this);
        //}

        //public virtual void SetValue(object value)
        //{
        //    this.EnsureTypeInitialized();

        //    this.type.SetValue(this, value);
        //}

        //protected virtual void EnsureTypeInitialized()
        //{
        //    if(this.type == null)
        //    {
        //        this.type = VariableType.Get(this.TypeName);
        //    }
        //}

        //IVariableType IVariable.Type { get => this.type; }

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

        public virtual object GetValue()
        {
            return null;
        }

        public virtual void SetValue(object value)
        {
        }
    }
}
