using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bpmtk.Engine.WebApi.Controllers
{
    /// <summary>
    /// ProcessDefinition APIs
    /// </summary>
    [Route("api/process-definitions")]
    [ApiController]
    public class ProcessDefinitionController : ControllerBase
    {
        private readonly IContext context;
        private readonly IDeploymentManager deploymentManager;

        public ProcessDefinitionController(IContext context)
        {
            this.context = context;
            this.deploymentManager = context.DeploymentManager;
        }

        /// <summary>
        /// Filter ProcessDefinitions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedResult<ProcessDefinitionModel>>> Get(
            [FromBody] ProcessDefinitionFilter filter)
        {
            PagedResult<ProcessDefinitionModel> result = new PagedResult<ProcessDefinitionModel>();

            var query = this.deploymentManager.CreateDefinitionQuery()
                .FetchLatestVersionOnly();

            result.Count = await query.CountAsync();
            var items = await query.ListAsync(filter.Page, filter.PageSize);

            result.Items = items.Select(x => ProcessDefinitionModel.Create(x)).ToList();
            result.Page = filter.Page;
            result.PageSize = filter.PageSize;

            return result;
        }

        /// <summary>
        /// Find the specified ProcessDefinition.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessDefinitionModel>> Get(int id)
        {
            var item = await this.deploymentManager.CreateDefinitionQuery()
                .SetId(id)
                .SingleAsync();

            if (item != null)
                return ProcessDefinitionModel.Create(item);

            return this.NotFound();
        }

        /// <summary>
        /// Gets IdentityLinks of ProcessDefinition.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/identity-links")]
        public ActionResult<IEnumerable<IdentityLinkModel>> GetIdentityLinks(int id)
        {
            var q = this.context.DbSession.IdentityLinks
                .Where(x => x.ProcessDefinition.Id == id)
                .Select(x => new IdentityLinkModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    GroupId = x.GroupId,
                    Type = x.Type
                }).ToArray();
            
            return q;
        }

        [HttpPut("{id}/inactive")]
        public async Task<ActionResult> Inactivate(int id,
            ProcessDefinitionActionModel model)
        {
            await this.deploymentManager.InactivateProcessDefinitionAsync(id,
                model?.Comment);

            return this.Ok();
        }

        [HttpPut("{id}/active")]
        public async Task<ActionResult> Activate(int id,
            ProcessDefinitionActionModel model)
        {
            var procDef = await this.deploymentManager.ActivateProcessDefinitionAsync(id, 
                model?.Comment);

            return this.Ok();
        }
    }
}
