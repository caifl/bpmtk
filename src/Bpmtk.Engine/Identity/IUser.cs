using System;

namespace Bpmtk.Engine.Identity
{
    public interface IUser
    {
        int Id
        {
            get;
        }

        string Name
        {
            get;
            set;
        }

        string UserName
        {
            get;
            set;
        }
    }
}
