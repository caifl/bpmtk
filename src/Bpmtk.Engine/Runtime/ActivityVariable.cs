using System;

namespace Bpmtk.Engine.Runtime
{
    public class ActivityVariable : VariableInstance
    {
        protected ActivityInstance activityInstance;

        protected ActivityVariable()
        { }

        public ActivityVariable(ActivityInstance activityInstance,
            string name,
            object value,
            bool isLocal = false
            ) : base(name, value)
        {
            if (activityInstance == null)
                throw new ArgumentNullException(nameof(activityInstance));

            this.activityInstance = activityInstance;
            this.IsLocal = isLocal;
        }

        public virtual bool IsLocal
        {
            get;
            protected set;
        }

        public virtual ActivityInstance ActivityInstance
        {
            get => this.activityInstance;
        }
    }
}
