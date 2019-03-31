using System;
using System.Xml.Linq;
using Bpmtk.Bpmn2.DI;

namespace Bpmtk.Bpmn2.Parser.Handlers
{
    class BPMNDiagramParseHandler : ParseHandler<Definitions>
    {
        public BPMNDiagramParseHandler()
        {
            this.handlers.Add("BPMNPlane", new BPMNPlaneParseHandler());    
        }

        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            var diagram = context.BpmnFactory.CreateDiagram();
            parent.Diagrams.Add(diagram);

            diagram.Id = element.GetAttribute("id");
            diagram.Name = element.GetAttribute("name");
            diagram.Documentation = element.GetAttribute("documentation");

            var value = element.GetAttribute("resolution");
            if (value != null)
                diagram.Resolution = double.Parse(value);

            if(element.HasElements)
                base.CreateChildren(diagram, context, element);

            return diagram;
        }
    }

    class BPMNPlaneParseHandler : ParseHandler<BPMNDiagram>
    {
        public BPMNPlaneParseHandler()
        {
            this.handlers.Add("BPMNShape", new BPMNShapeParseHandler());
            this.handlers.Add("BPMNEdge", new BPMNEdgeParseHandler());
        }

        public override object Create(BPMNDiagram parent, IParseContext context, XElement element)
        {
            var plane = context.BpmnFactory.CreatePlane();
            parent.BPMNPlane = plane;

            plane.Id = element.GetAttribute("id");
            plane.BpmnElement = element.GetAttribute("bpmnElement");

            if(element.HasElements)
                base.CreateChildren(plane, context, element);

            return plane;
        }
    }

    class BPMNShapeParseHandler : ParseHandler<BPMNPlane>
    {
        public BPMNShapeParseHandler()
        {
            this.handlers.Add("BPMNLabel", new BPMNLabelParseHandler<BPMNShape>());
            this.handlers.Add("Bounds", new ParseHandlerAction<BPMNShape>((parent, context, element) =>
            {
                var bounds = context.BpmnFactory.CreateBounds();

                bounds.X = double.Parse(element.GetAttribute("x"));
                bounds.Y = double.Parse(element.GetAttribute("y"));
                bounds.Width = double.Parse(element.GetAttribute("width"));
                bounds.Height = double.Parse(element.GetAttribute("height"));

                parent.Bounds = bounds;
            }));
        }

        public override object Create(BPMNPlane parent, IParseContext context, XElement element)
        {
            var shape = context.BpmnFactory.CreateShape();

            shape.Id = element.GetAttribute("id");
            shape.BpmnElement = element.GetAttribute("bpmnElement");

            parent.DiagramElements.Add(shape);

            if (element.HasElements)
                base.CreateChildren(shape, context, element);

            return shape;
        }
    }

    class BPMNEdgeParseHandler : ParseHandler<BPMNPlane>
    {
        public BPMNEdgeParseHandler()
        {
            this.handlers.Add("BPMNLabel", new BPMNLabelParseHandler<BPMNEdge>());
            this.handlers.Add("waypoint", new ParseHandlerAction<BPMNEdge>((parent, context, element) =>
            {
                var waypoint = context.BpmnFactory.CreatePoint();

                waypoint.X = double.Parse(element.GetAttribute("x"));
                waypoint.Y = double.Parse(element.GetAttribute("y"));

                parent.Waypoints.Add(waypoint);
            }));
        }

        public override object Create(BPMNPlane parent, IParseContext context, XElement element)
        {
            var edge = context.BpmnFactory.CreateEdge();
            parent.DiagramElements.Add(edge);

            edge.Id = element.GetAttribute("id");
            edge.BpmnElement = element.GetAttribute("bpmnElement");

            if(element.HasElements)
                base.CreateChildren(edge, context, element);

            return edge;
        }
    }

    class BPMNLabelParseHandler<TLabeledShape> : ParseHandler<TLabeledShape>
        where TLabeledShape : ILabeled
    {
        public BPMNLabelParseHandler()
        {
            this.handlers.Add("Bounds", new ParseHandlerAction<BPMNLabel>((parent, context, element) =>
            {
                var bounds = context.BpmnFactory.CreateBounds();

                bounds.X = double.Parse(element.GetAttribute("x"));
                bounds.Y = double.Parse(element.GetAttribute("y"));
                bounds.Width = double.Parse(element.GetAttribute("width"));
                bounds.Height = double.Parse(element.GetAttribute("height"));

                parent.Bounds = bounds;
            }));
        }

        public override object Create(TLabeledShape parent, IParseContext context, XElement element)
        {
            var label = context.BpmnFactory.CreateLabel();

            label.Id = element.GetAttribute("id");

            parent.BpmnLabel = label;

            if(element.HasElements)
                base.CreateChildren(label, context, element);

            return label;
        }
    }
}
