using System;

namespace Bpmtk.Bpmn2.DC
{
    public class Font
    {
        public virtual string Name
        {
            get;
            set;
        }

        public virtual double Size
        {
            get;
            set;
        }

        public virtual bool IsBold
        {
            get;
            set;
        }

        public virtual bool IsItalic
        {
            get;
            set;
        }

        public virtual bool IsUnderline
        {
            get;
            set;
        }

        public virtual bool IsStrikeThrough
        {
            get;
            set;
        }
    }
}
