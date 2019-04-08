using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Runtime
{
    public interface ITokenQuery
    {
        ITokenQuery SetProcessInstance(long id);

        IList<Token> List();
    }
}
