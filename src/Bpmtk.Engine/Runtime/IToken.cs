using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Runtime
{
    public interface IToken
    {
        long Id
        {
            get;
        }

        string ActivityId
        {
            get;
        }
    }
}
