using System;
using System.Collections.Generic;
using Bpmtk.Bpmn2.Extensions;

namespace Bpmtk.Bpmn2
{
    public class ResourceRole : BaseElement
    {
        protected List<ResourceParameterBinding> parameterBindings = new List<ResourceParameterBinding>();

        public ResourceAssignmentExpression AssignmentExpression
        {
            get;
            set;
        }

        public string ResourceRef
        {
            get;
            set;
        }

        public virtual Resource Resource
        {
            get;
            set;
        }

        public virtual IList<ResourceParameterBinding> ParameterBindings => this.parameterBindings;

        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The type of resource.(Extended)
        /// </summary>
        public virtual ResourceType? Type
        {
            get;
            set;
        }
    }

    /// <summary>
    /// The Performer class defines the resource that will perform or will be responsible for an Activity. The performer can
    /// be specified in the form of a specific individual, a group, an organization role or position, or an organization.
    /// The Performer element inherits the attributes and model associations of BaseElement(see Table 8.5) through its
    /// relationship to ResourceRole, but does not have any additional attributes or model associations.
    /// </summary>
    public class Performer : ResourceRole
    {

    }

    public class HumanPerformer : Performer
    {

    }

    /// <summary>
    /// Potential owners of a User Task are persons who can claim and work on it.
    /// A potential owner becomes the actual owner
    /// of a Task, usually by explicitly claiming it.
    /// </summary>
    public class PotentialOwner : HumanPerformer
    {

    }

    public class ResourceAssignmentExpression : BaseElement
    {
        public virtual Expression Expression
        {
            get;
            set;
        }
    }

    public class ResourceParameterBinding : BaseElement
    {
        public virtual Expression Expression
        {
            get;
            set;
        }

        public virtual string ParameterRef
        {
            get;
            set;
        }
    }
}
