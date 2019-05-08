using System;

namespace Bpmtk.Engine.Models
{
    /// <summary>
    /// Historic BPMN model information.
    /// </summary>
    public class HistoricModel
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual Package Package
        {
            get;
            set;
        }

        public virtual string UserId
        {
            get;
            set;
        }

        public virtual ByteArray Content
        {
            get;
            set;
        }

        public virtual DateTime Created
        {
            get;
            set;
        }

        public virtual string Comment
        {
            get;
            set;
        }
    }
}
