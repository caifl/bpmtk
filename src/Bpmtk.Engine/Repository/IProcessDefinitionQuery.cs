﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Repository
{
    public interface IProcessDefinitionQuery
    {
        IProcessDefinitionQuery SetId(int processDefinitionId);

        IProcessDefinitionQuery SetDeploymentId(int deploymentId);

        IProcessDefinitionQuery SetKey(string key);

        IProcessDefinitionQuery SetKeyAny(IEnumerable<string> keys);

        IProcessDefinitionQuery SetCategory(string category);

        IProcessDefinitionQuery SetState(ProcessDefinitionState state);

        IProcessDefinitionQuery SetName(string name);

        IProcessDefinitionQuery SetVersion(int version);

        IProcessDefinitionQuery SetDescription(string description);

        IProcessDefinitionQuery FetchDeployment();

        IProcessDefinitionQuery FetchIdentityLinks();

        IProcessDefinitionQuery FetchLatestVersionOnly();

        IProcessDefinition Single();

        Task<IProcessDefinition> SingleAsync();

        IList<IProcessDefinition> List();

        Task<int> CountAsync();

        Task<IList<IProcessDefinition>> ListAsync();

        Task<IList<IProcessDefinition>> ListAsync(int count);

        Task<IList<IProcessDefinition>> ListAsync(int page, int pageSize);
    }
}
