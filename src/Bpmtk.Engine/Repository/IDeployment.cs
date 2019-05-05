using System;
using System.Collections.Generic;
using Bpmtk.Engine.Identity;

namespace Bpmtk.Engine.Repository
{
    public interface IDeployment
    {
        int Id
        {
            get;
        }

        string Name
        {
            get;
        }

        string TenantId
        {
            get;
        }

        /// <summary>
        /// Gets or sets category.
        /// </summary>
        string Category
        {
            get;
        }

        /// <summary>
        /// Gets or sets BPMN Meta-Model Data.
        /// </summary>
        //Byte[] Model
        //{
        //    get;
        //}

        /// <summary>
        /// Gets the deployed time.
        /// </summary>
        DateTime Created
        {
            get;
        }

        /// <summary>
        /// Gets the user
        /// </summary>
        IUser User
        {
            get;
        }

        /// <summary>
        /// Gets the source package identifier.
        /// </summary>
        int? PackageId
        {
            get;
        }

        IReadOnlyList<IProcessDefinition> ProcessDefinitions
        {
            get;
        }

        string Memo
        {
            get;
        }
    }
}
