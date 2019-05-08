using System;

namespace Bpmtk.Engine.WebApi.Models
{
    public class DeployBpmnModel
    {
        /// <summary>
        /// BPMN 2.0 model xml content.
        /// </summary>
        public string BpmnXml
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
