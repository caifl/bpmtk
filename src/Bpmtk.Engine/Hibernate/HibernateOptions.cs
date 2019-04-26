using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Stores
{
    public class HibernateOptions
    {
        /// <summary>
        /// Gets or sets entity mappings directory.
        /// </summary>
        public virtual string Mappings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets hibernate cfg xml file name.
        /// </summary>
        public virtual string FileName
        {
            get;
            set;
        }
    }
}
