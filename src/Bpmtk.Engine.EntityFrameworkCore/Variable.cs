using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine
{
    public class Variable
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

        //public abstract object Value
        //{
        //    get;
        //    set;
        //}

        //public virtual ProcessInstance ProcessInstance
        //{
        //    get;
        //    set;
        //}
    }
}
