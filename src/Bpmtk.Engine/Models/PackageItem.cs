using System;

namespace Bpmtk.Engine.Models
{
    public class PackageItem
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual ByteArray Content
        {
            get;
            set;
        }

        public virtual Package Package
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets name of item.
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets type of item.
        /// 
        /// such as models/forms/business objects/views/reports etc.
        /// </summary>
        public virtual string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user who created this item.
        /// </summary>
        public virtual string UserId
        {
            get;
            set;
        }

        public virtual DateTime Created
        {
            get;
            set;
        }

        public virtual DateTime Modified
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }
    }
}
