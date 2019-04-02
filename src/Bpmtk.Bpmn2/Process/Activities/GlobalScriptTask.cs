using System;

namespace Bpmtk.Bpmn2
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
