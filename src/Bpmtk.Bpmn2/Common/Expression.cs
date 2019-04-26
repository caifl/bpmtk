using System;

namespace Bpmtk.Bpmn2
{
    public class Expression : BaseElementWithMixedContent
    {

    }

    public class FormalExpression : Expression
    {
        public virtual string Language
        {
            get;
            set;
        }

        public virtual string EvaluatesToTypeRef
        {
            get;
            set;
        }
    }
}
