using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Bpmtk.Bpmn2.Extensions;

namespace Bpmtk.Bpmn2
{
    /// <summary>
    /// 流程定义信息
    /// </summary>
    public class Process : CallableElement, 
        IFlowElementsContainer 
    {
        protected bool isExecutable;
        protected ProcessType processType;
        protected bool isClosed;
        protected string definitionalCollaborationRef;

        //extended attributes
        protected readonly Dictionary<string, string> attributes = new Dictionary<string, string>();

        private readonly List<LaneSet> laneSets = new List<LaneSet>();
        protected List<Property> properties = new List<Property>();
        protected List<ResourceRole> resources = new List<ResourceRole>();
        private readonly FlowElementCollection flowElements;
        protected List<Artifact> artifacts = new List<Artifact>();
        protected List<EventListener> eventListeners = new List<EventListener>();

        private IDictionary<string, FlowElement> flowElementById;

        public Process()
        {
            this.flowElements = new FlowElementCollection(this);
            this.processType = ProcessType.None;
            this.isClosed = false;
            this.isExecutable = false;
        }

        public virtual IList<LaneSet> LaneSets => this.laneSets;

        /// <summary>
        /// Get extended attributes of Process.
        /// </summary>
        public virtual IReadOnlyDictionary<string, string> Attributes
        {
            get
            {
                return new ReadOnlyDictionary<string, string>(this.attributes);
            }
        }

        public virtual IList<StartEvent> StartEvents
        {
            get => this.flowElements.OfType<StartEvent>().ToList();
        }

        public virtual FlowElement FindFlowElementById(string id, bool recurive = false)
        {
            if (this.flowElementById == null)
            {
                this.flowElementById = this.flowElements.ToDictionary(x => x.Id);
            }

            FlowElement flowElement = null;
            if (this.flowElementById.TryGetValue(id, out flowElement))
                return flowElement;

            if (recurive)
            {
                var subProcessList = this.flowElements.OfType<SubProcess>();
                foreach (var subProcess in subProcessList)
                {
                    flowElement = subProcess.FindFlowElementById(id, recurive);
                    if (flowElement != null)
                        return flowElement;
                }
            }

            return null;
        }

        //public virtual TFlowElement GetFlowElementById<TFlowElement>(string id, bool recursive = false) 
        //    where TFlowElement : FlowElement
        //{
        //    var flowElement = this.GetFlowElementById(id, recursive);
        //    if(flowElement != null)
        //        return (TFlowElement)flowElement;

        //    return null;
        //}

        //public void Initialize(Definitions model)
        //{
        //    this.transitions = this.FlowElements.OfType<SequenceFlow>().ToDictionary(x => x.Id);
        //    this.flowNodes = this.FlowElements.OfType<FlowNode>().ToDictionary(x => x.Id);

        //    this.InitializeSequenceFlowRefs();

        //    //Set Default outgong transitions
        //    foreach (var element in this.FlowElements)
        //    {
        //        if (element is IHasDefaultOutgoing)
        //        {
        //            IHasDefaultOutgoing hasDefaultElement = element as IHasDefaultOutgoing;
        //            if (!string.IsNullOrEmpty(hasDefaultElement.Default))
        //            {
        //                SequenceFlow sequenceFlow = null;
        //                if (this.transitions.TryGetValue(hasDefaultElement.Default, out sequenceFlow))
        //                {
        //                    hasDefaultElement.DefaultSequenceFlow = sequenceFlow;
        //                }
        //                else
        //                {
        //                    throw new BpmnException($"无效的BPMN模型, 缺省迁移不存在[{hasDefaultElement.Default}]");
        //                }
        //            }
        //        }

        //        //初始化子流程
        //        if (element is IFlowElementContainer)
        //            ((IFlowElementContainer)element).Initialize(model);
        //    }

        //    var startNodes = this.InitializeFlowNodeTransitions();
        //    if (startNodes.Count == 0)
        //        throw new BpmnException("无效的流程定义, 未找到任何起始节点");

        //    this.initialNodes = startNodes;
        //    this.hasDiagram = model.Diagrams.Any(x => x.BpmnPlane.BpmnElement == this.Id);
        //}

        /// <remarks/>
        //[XmlElement("auditing", Order = 0)]
        //public Auditing Auditing
        //{
        //    get
        //    {
        //        return this.auditing;
        //    }
        //    set
        //    {
        //        this.auditing = value;
        //        this.RaisePropertyChanged("auditing");
        //    }
        //}

        /// <remarks/>
        //[XmlElement("monitoring", Order = 1)]
        //public Monitoring Monitoring
        //{
        //    get
        //    {
        //        return this.monitoring;
        //    }
        //    set
        //    {
        //        this.monitoring = value;
        //        this.RaisePropertyChanged("monitoring");
        //    }
        //}

        /// <remarks/>
        //[XmlElement("property", Order = 2)]
        public virtual IList<Property> Properties => this.properties;

        /// <remarks/>
        //[XmlElement("laneSet", Order = 3)]
        //public LaneSet[] LaneSet
        //{
        //    get
        //    {
        //        return this.laneSet;
        //    }
        //    set
        //    {
        //        this.laneSet = value;
        //        this.RaisePropertyChanged("laneSet");
        //    }
        //}
        public virtual IList<FlowElement> FlowElements => this.flowElements;

        public virtual IList<EventListener> EventListeners => this.eventListeners;

        //[XmlElement("group", typeof(Group), Order = 5)]
        public virtual IList<Artifact> Artifacts => this.artifacts;

        public virtual IList<ResourceRole> Resources => this.resources;

        /// <remarks/>
        //[XmlElement("correlationSubscription", Order = 7)]
        //public CorrelationSubscription[] CorrelationSubscriptions
        //{
        //    get
        //    {
        //        return this.correlationSubscription;
        //    }
        //    set
        //    {
        //        this.correlationSubscription = value;
        //        this.RaisePropertyChanged("correlationSubscription");
        //    }
        //}

        /// <remarks/>
        //[XmlElement("supports", Order = 8)]
        //public string[] Supports
        //{
        //    get
        //    {
        //        return this.supports;
        //    }
        //    set
        //    {
        //        this.supports = value;
        //        this.RaisePropertyChanged("supports");
        //    }
        //}

        #region Properties

        public virtual ProcessType ProcessType
        {
            get;
            set;
        }

        public virtual bool IsClosed
        {
            get;
            set;
        }

        public virtual bool IsExecutable
        {
            get;
            set;
        }

        /// <summary>
        /// The form handler key or class name.(extended)
        /// </summary>
        public virtual string FormHandler
        {
            get;
            set;
        }

        public virtual string VersionTag
        {
            get;
            set;
        }

        #endregion
    }

    public enum ProcessType
    {
        /// <remarks/>
        None,

        /// <remarks/>
        Public,

        /// <remarks/>
        Private,
    }
}
