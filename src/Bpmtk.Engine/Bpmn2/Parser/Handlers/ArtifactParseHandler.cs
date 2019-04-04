using System;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    class ArtifactParseHandler : BaseElementParseHandler
    {
        public override object Create(object parent, IParseContext context, XElement element)
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
                    association.AssociationDirection = element.GetEnum("AssociationDirection", AssociationDirection.None);

                    artifact = association;
                    break;

                default:
                    throw new NotImplementedException();
            }

            //if (artifact != null)
            //    parent.Artifacts.Add(artifact);

            return artifact;
        }
    }
}
