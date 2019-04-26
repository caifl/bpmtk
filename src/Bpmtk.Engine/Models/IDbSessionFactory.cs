using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Models
{
    public interface IDbSessionFactory
    {
        IDbSession Create();
    }
}
