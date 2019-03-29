using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Bpmn2
{
    public abstract class Artifact : BaseElement
    {
    }

    public class TextAnnotation : Artifact
    {
        public TextAnnotation()
        {
            this.TextFormat = "text/plain";
        }

        public virtual string Text
        {
            get;
            set;
        }

        public virtual string TextFormat
        {
            get;
            set;
        }
    }

    public class Association : Artifact
    {
        public virtual string SourceRef
        {
            get;
            set;
        }

        public virtual string TargetRef
        {
            get;
            set;
        }

        public virtual AssociationDirection AssociationDirection
        {
            get;
            set;
        }
    }

    public enum AssociationDirection
    {
        None,

        One,

        Both
    }
}
