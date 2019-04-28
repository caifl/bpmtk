using Bpmtk.Engine.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bpmtk.Engine.WebApi.Controllers
{
    [Route("/api/process-instances")]
    public class ProcessInstanceController : ControllerBase
    {
        private readonly IContext context;

        public ProcessInstanceController(IContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProcessInstanceModel>> Get()
        {
            var q = this.context.RuntimeManager.ProcessInstances;

            var data = q.Select(x => ProcessInstanceModel.Create(x)).ToArray();

            return data;
        }

        [HttpGet("{id}/activity-instances")]
        public async Task<ActionResult<IEnumerable<ActivityInstanceModel>>> GetActivityInstances(long id)
        {
            var q = await this.context.HistoryManager.GetActivityInstancesAsync(id);
            return q.Select(x => ActivityInstanceModel.Create(x)).ToArray();
        }

        [HttpGet("{id}/active-activity-ids")]
        public ActionResult<IEnumerable<string>> GetActiveActivityIds(long id)
        {
            var q = this.context.RuntimeManager.GetActiveActivityIdsAsync(id).Result;
            return q.ToArray();
        }

        [HttpPut("{id}/aborted")]
        public ActionResult Abort(long id)
        {
            return this.Ok();
        }
    }
}
