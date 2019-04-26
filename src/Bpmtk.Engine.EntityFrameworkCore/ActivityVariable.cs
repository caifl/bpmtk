using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine
{
    public class ActivityVariable
    {
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

        //public abstract Object Value
        //{
        //    get;
        //    set;
        //}

        public virtual long? ByteArrayId
        {
            get;
            set;
        }

        public virtual ByteArray ByteArray
        {
            get;
            set;
        }
    }
}
