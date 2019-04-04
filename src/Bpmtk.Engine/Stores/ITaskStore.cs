using Bpmtk.Engine.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Stores
{
    public interface ITaskStore
    {
        TaskInstance Find(long id);

        int GetActiveTaskCount(long tokenId);
    }
}
