using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine
{
    public class ProcessInstance
    {
        public ProcessInstance()
        {
            this.Variables = new List<Variable>();
            this.Tokens = new List<Token>();
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

        public virtual ICollection<Variable> Variables
        {
            get;
            set;
        }

        public virtual Variable GetVariable(string name)
        {
            return null;
        }

        //public virtual Token Token
        //{
        //    get;
        //    set;
        //}

        public virtual ICollection<Token> Tokens
        {
            get;
            set;
        }
    }
}
