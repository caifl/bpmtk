using System;
using System.Xml.Linq;
using Bpmtk.Bpmn2.DI;

namespace Bpmtk.Bpmn2.Parser
{
    class BpmnDiagramHandler : BpmnHandler<Definitions, BPMNDiagram>
    {
        public BpmnDiagramHandler()
        {
            this.handlers.Add("BPMNPlane", new BpmnPlaneHandler());    
        }

        public override BPMNDiagram Create(Definitions parent, IParseContext context, XElement element)
        {
            var diagram = base.Create(parent, context, element);

            diagram.Id = element.GetAttribute("id");
            diagram.Name = element.GetAttribute("name");
            diagram.Documentation = element.GetAttribute("documentation");

            var value = element.GetAttribute("resolution");
            if (value != null)
                diagram.Resolution = double.Parse(value);

            parent.Diagrams.Add(diagram);

            return diagram;
        }

        protected override BPMNDiagram New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateDiagram();
    }

    class BpmnPlaneHandler : BpmnHandler<BPMNDiagram, BPMNPlane>
    {
        public BpmnPlaneHandler()
        {
            this.handlers.Add("BPMNShape", new BpmnShapeHandler());
            this.handlers.Add("BPMNEdge", new BpmnEdgeHandler());
        }

        public override BPMNPlane Create(BPMNDiagram parent, IParseContext context, XElement element)
        {
            var plane = base.Create(parent, context, element);

            plane.Id = element.GetAttribute("id");
            plane.BpmnElement = element.GetAttribute("bpmnElement");

            parent.BPMNPlane = plane;

            return plane;
        }

        protected override BPMNPlane New(IParseContext context, XElement element) => context.BpmnFactory.CreatePlane();
    }

    class BpmnShapeHandler : BpmnHandler<BPMNPlane, BPMNShape>
    {
        public BpmnShapeHandler()
        {
            this.handlers.Add("BPMNLabel", new BpmnLabelHandler<BPMNShape>());
            this.handlers.Add("Bounds", new BpmnHandlerCallback<BPMNShape>((parent, context, element) =>
            {
                var bounds = context.BpmnFactory.CreateBounds();

                bounds.X = double.Parse(element.GetAttribute("x"));
                bounds.Y = double.Parse(element.GetAttribute("y"));
                bounds.Width = double.Parse(element.GetAttribute("width"));
                bounds.Height = double.Parse(element.GetAttribute("height"));

                parent.Bounds = bounds;

                return bounds;
            }));
        }

        public override BPMNShape Create(BPMNPlane parent, IParseContext context, XElement element)
        {
            var shape = base.Create(parent, context, element);

            shape.Id = element.GetAttribute("id");
            shape.BpmnElement = element.GetAttribute("bpmnElement");

            parent.DiagramElements.Add(shape);

            return shape;
        }

        protected override BPMNShape New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateShape();
    }

    class BpmnEdgeHandler : BpmnHandler<BPMNPlane, BPMNEdge>
    {
        public BpmnEdgeHandler()
        {
            this.handlers.Add("BPMNLabel", new BpmnLabelHandler<BPMNEdge>());
            this.handlers.Add("waypoint", new BpmnHandlerCallback<BPMNEdge>((parent, context, element) =>
            {
                var waypoint = context.BpmnFactory.CreatePoint();

                waypoint.X = double.Parse(element.GetAttribute("x"));
                waypoint.Y = double.Parse(element.GetAttribute("y"));

                parent.Waypoints.Add(waypoint);

                return waypoint;
            }));
        }

        public override BPMNEdge Create(BPMNPlane parent, IParseContext context, XElement element)
        {
            var edge = base.Create(parent, context, element);

            edge.Id = element.GetAttribute("id");
            edge.BpmnElement = element.GetAttribute("bpmnElement");

            parent.DiagramElements.Add(edge);

            return edge;
        }

        protected override BPMNEdge New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateEdge();
    }

    class BpmnLabelHandler<TLabeledShape> : BpmnHandler<TLabeledShape, BPMNLabel>
        where TLabeledShape : ILabeled
    {
        public BpmnLabelHandler()
        {
            this.handlers.Add("Bounds", new BpmnHandlerCallback<BPMNLabel>((parent, context, element) =>
            {
                var bounds = context.BpmnFactory.CreateBounds();

                bounds.X = double.Parse(element.GetAttribute("x"));
                bounds.Y = double.Parse(element.GetAttribute("y"));
                bounds.Width = double.Parse(element.GetAttribute("width"));
                bounds.Height = double.Parse(element.GetAttribute("height"));

                parent.Bounds = bounds;

                return bounds;
            }));
        }

        public override BPMNLabel Create(TLabeledShape parent, IParseContext context, XElement element)
        {
            var label = base.Create(parent, context, element);

            label.Id = element.GetAttribute("id");

            parent.BpmnLabel = label;

            return label;
        }

        protected override BPMNLabel New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateLabel();
    }
}
