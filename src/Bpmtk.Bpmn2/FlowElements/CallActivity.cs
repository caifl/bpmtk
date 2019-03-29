using System;

namespace Bpmtk.Bpmn2
{
    public class CallActivity : Activity
    {
        public virtual string CalledElement
        {
            get;
            set;
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        public virtual bool IsAsync
        {
            get;
            set;
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
