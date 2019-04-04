using System;

namespace Bpmtk.Engine.Bpmn2
{
    /// <summary>
    /// Structure information of data item.
    /// </summary>
    public class ItemDefinition : RootElement
    {
        public virtual string StructureRef
        {
            get;
            set;
        }

        public virtual bool IsCollection
        {
            get;
            set;
        }

        public virtual ItemKind ItemKind
        {
            get;
            set;
        }
    }

    public enum ItemKind
    {
        /// <remarks/>
        Information,

        /// <remarks/>
        Physical,
    }
}
