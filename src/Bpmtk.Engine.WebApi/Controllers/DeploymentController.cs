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

        public DeploymentController(IContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DeploymentModel>> Get()
        {
            var q = this.context.DeploymentManager.Deployments;

            var data = q.Select(x => DeploymentModel.Create(x)).ToArray();

            return data;
        }

        [HttpGet("{id}/bpmn-model/xml")]
        public async Task<ActionResult<string>> GetModelXml(int id)
        {
            var model = await this.context.DeploymentManager.GetBpmnModelAsync(id);
            if(model != null)
                return Encoding.UTF8.GetString(model.Data);

            return null;
        }

        //[HttpGet("{id}/active-activity-ids")]
        //public ActionResult<IEnumerable<string>> GetActiveActivityIds(long id)
        //{
        //    var q = this.context.RuntimeManager.GetActiveActivityIdsAsync(id).Result;
        //    return q.ToArray();
        //}
    }
}
