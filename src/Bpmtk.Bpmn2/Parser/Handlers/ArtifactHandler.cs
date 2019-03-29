using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class ArtifactHandler<TParent> : BaseElementHandler<TParent, Artifact>
        where TParent : IFlowElementsContainer
    {
        public override Artifact Create(TParent parent, IParseContext context, XElement element)
        {
            var artifact = base.Create(parent, context, element);

            if(artifact != null)
                parent.Artifacts.Add(artifact);

            return artifact;
        }

        protected override Artifact New(IParseContext context, XElement element)
        {
            var localName = element.Name.LocalName;
            Artifact artifact = null;

            switch (localName)
            {
                case "textAnnotation":
                    var item = new TextAnnotation();
                    item.Text = element.Value;
                    item.TextFormat = element.GetAttribute("textFormat");
                    artifact = item;
                    break;

                case "association":
                    var association = context.BpmnFactory.CreateAssociation();
                    association.SourceRef = element.GetAttribute("sourceRef");
                    association.TargetRef = element.GetAttribute("targetRef");
                    association.AssociationDirection = element.GetEnum<AssociationDirection>("AssociationDirection", AssociationDirection.None);

                    artifact = association;
                    break;

                default:
                    throw new NotImplementedException();
            }

            return artifact;
        }
    }
}
