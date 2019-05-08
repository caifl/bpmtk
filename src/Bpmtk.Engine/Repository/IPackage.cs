using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Repository
{
    public interface IPackage
    {
        int Id
        {
            get;
        }

        string Name
        {
            get;
        }

        DateTime Created
        {
            get;
        }
    }
}
