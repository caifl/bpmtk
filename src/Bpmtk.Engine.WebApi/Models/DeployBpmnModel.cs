using System;

namespace Bpmtk.Engine.WebApi.Models
{
    public class DeployBpmnModel
    {
        public string BpmnModelXml
        {
            get;
            set;
        }

        /// <summary>
        /// The name of deployment.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The identifier of source package.
        /// </summary>
        public virtual int? PackageId
        {
            get;
            set;
        }
    }
}
