using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine
{
    public class Token
    {
        public Token()
        {
            this.Variables = new List<Variable>();
            this.Children = new List<Token>();
        }

        public virtual long Id
        {
            get;
            set;
        }

        public virtual ProcessInstance ProcessInstance
        {
            get;
            set;
        }

        public virtual bool IsActive
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

        public virtual Token Parent
        {
            get;
            set;
        }

        public virtual ICollection<Token> Children
        {
            get;
            set;
        }
    }
}
