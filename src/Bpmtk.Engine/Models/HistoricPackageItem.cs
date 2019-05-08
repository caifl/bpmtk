using System;

namespace Bpmtk.Engine.Models
{
    /// <summary>
    /// Historic package item information.
    /// </summary>
    public class HistoricPackageItem
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual PackageItem Item
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
