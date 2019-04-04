using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class GlobalScriptTask : GlobalTask
    {
        public virtual string Script
        {
            get;
            set;
        }

        public virtual string ScriptLanguage
        {
            get;
            set;
        }
    }
}
