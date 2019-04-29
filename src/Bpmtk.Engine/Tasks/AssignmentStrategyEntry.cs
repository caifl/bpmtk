using System;

namespace Bpmtk.Engine.Tasks
{
    public class AssignmentStrategyEntry
    {
        public AssignmentStrategyEntry(string key, string name,
            IAssignmentStrategy assignmentStrategy)
        {
            this.Key = key;
            this.Name = name;
            this.AssignmentStrategy = assignmentStrategy;
        }

        public virtual string Key
        {
            get;
        }

        public virtual string Name
        {
            get;
        }

        public virtual IAssignmentStrategy AssignmentStrategy { get; }
    }
}
