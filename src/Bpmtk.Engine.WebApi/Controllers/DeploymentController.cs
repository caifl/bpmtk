using Bpmtk.Engine.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bpmtk.Engine.WebApi.Controllers
{
    [Route("/api/deployments")]
    public class DeploymentController : ControllerBase
    {
        private readonly IContext context;
        private readonly IDeploymentManager deploymentManager;

        public DeploymentController(IContext context)
        {
            this.context = context;
            this.deploymentManager = context.DeploymentManager;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<DeploymentModel>>> Get(DeploymentFilter filter)
        {
            var result = new PagedResult<DeploymentModel>();
            
            var query = this.deploymentManager.CreateDeploymentQuery();

            var list = await query.ListAsync(filter.Page, filter.PageSize);
            result.Count = await query.CountAsync();
            result.Page = filter.Page;
            result.PageSize = filter.PageSize;
            result.Items = list.Select(x => DeploymentModel.Create(x))
                .ToList();

            return this.Ok(result);
        }

        [HttpGet("{id}/bpmn-model/xml")]
        public async Task<ActionResult<string>> GetModelXml(int id)
        {
            var content = await deploymentManager.GetBpmnModelContentAsync(id);
            if(content != null)
                return Encoding.UTF8.GetString(content);

            return null;
        }

        /// <summary>
        /// Deploy BPMN 2.0 model.
        /// </summary>
        [HttpPost("deploy")]
        public ActionResult Deploy(DeployBpmnModel model)
        {
            return this.Ok();
        }

        //[HttpGet("{id}/active-activity-ids")]
        //public ActionResult<IEnumerable<string>> GetActiveActivityIds(long id)
        //{
        //    var q = this.context.RuntimeManager.GetActiveActivityIdsAsync(id).Result;
        //    return q.ToArray();
        //}
    }
}
