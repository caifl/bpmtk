namespace Bpmtk.Engine.Runtime
{
    /// <summary>
    /// Runtime Variable Object.
    /// </summary>
    public class Variable : VariableInstance
    {
        protected Variable()
        {
        }

        public Variable(Token token, string name, object value) : base(name, value)
        {
            this.Token = token;
        }

        public virtual Token Token
        {
            get;
            protected set;
        }
    }
}
