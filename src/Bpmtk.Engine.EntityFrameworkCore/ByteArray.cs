using System;

namespace Bpmtk.Engine
{ 
    public class ByteArray
    {
        public virtual long Id
        {
            get;
            set;
        }

        //protected ByteArray()
        //{ }

        //public ByteArray(long id, byte[] value)
        //    : this(value)
        //{
        //    this.Id = id;
        //}

        //public ByteArray(byte[] value)
        //{
        //    this.Value = value;
        //}

        public virtual byte[] Value
        {
            get;
            set;
        }
    }
}
