using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Bpmtk.Engine.Bpmn2
{
    public static class BpmnModelHelper
    {
        readonly static ConcurrentDictionary<int, BpmnModel> cache = new ConcurrentDictionary<int, BpmnModel>();

        public static Process GetProcess(int processDefinitionId)
        {
            return null;
        }
    }
}
