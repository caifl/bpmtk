using System;

namespace Bpmtk.Engine.Repository
{
    public interface IProcessDefinition
    {
        ProcessDefinitionState State
        {
            get;
        }

        int Id
        {
            get;
        }

        string Name
        {
            get;
        }

        string Key
        {
            get;
        }

        int Version
        {
            get;
        }

        DateTime Created
        {
            get;
        }

        DateTime Modified
        {
            get;
        }

        bool HasDiagram
        {
            get;
        }

        DateTime? ValidFrom
        {
            get;
        }

        DateTime? ValidTo
        {
            get;
        }

        string TenantId
        {
            get;
        }

        string Category
        {
            get;
        }

        int DeploymentId
        {
            get;
        }

        string VersionTag
        {
            get;
        }

        string Description
        {
            get;
        }
    }

    public enum ProcessDefinitionState : int
    {
        Inactive = 0,

        Active = 1
    }
}
