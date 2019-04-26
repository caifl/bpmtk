using System;
using System.Collections.Generic;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Runtime
{
    public interface ITokenQuery
    {
        ITokenQuery SetProcessInstanceId(long id);

        IList<Token> List();
    }
}
