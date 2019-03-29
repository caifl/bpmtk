using System;

namespace Bpmtk.Bpmn2.Extensions
{
    public class EventListener
    {
        /// <summary>
        /// The name of Event.
        /// </summary>
        public virtual string Event
        {
            get;
            set;
        }

        /// <summary>
        /// The class name, which implements Bpmntk.Engine.Runtime.IEventListener.
        /// </summary>
        public virtual string Class
        {
            get;
            set;
        }

        public virtual string Script
        {
            get;
            set;
        }

        public virtual string ScriptFormat
        {
            get;
            set;
        }

        //public enum ImplementationType
        //{
        //    Class = 0,

        //    Script
        //}
    }
}
