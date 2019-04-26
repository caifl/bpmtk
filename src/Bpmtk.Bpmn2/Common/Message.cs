using System;

namespace Bpmtk.Bpmn2
{ 
    /// <summary>
    /// 消息定义
    /// </summary>
    public class Message : RootElement
    {
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 消息类型引用(itemDefinition#id)
        /// </summary>
        public virtual ItemDefinition ItemRef
        {
            get;
            set;
        }
    }
}
