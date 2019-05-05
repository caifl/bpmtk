using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bpmtk.Engine.WebApi.Models;

namespace Bpmtk.Engine.WebApi.Controllers
{
    [Route("/api/process-instances")]
    public class ProcessInstanceController : ControllerBase
    {
        private readonly IContext context;
        private readonly IRuntimeManager runtimeManager;

        public ProcessInstanceController(IContext context)
        {
            this.context = context;
            this.runtimeManager = context.RuntimeManager;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProcessInstanceModel>>> Get([FromBody] ProcessInstanceFilter filter)
        {
            var result = new PagedResult<ProcessInstanceModel>();

            var query = this.runtimeManager.CreateInstanceQuery()
                .FetchInitiator();

            if (filter == null)
                filter = new ProcessInstanceFilter();

            if (filter.AnyStates != null
                && filter.AnyStates.Length > 0)
                query.SetStateAny(filter.AnyStates);

            if (filter.State != null)
                query.SetState(filter.State.Value);

            var list = await query.ListAsync(filter.Page, filter.PageSize);
            result.Count = await query.CountAsync();

            result.Page = filter.Page;
            result.PageSize = filter.PageSize;
            result.Items = list.Select(x => ProcessInstanceModel.Create(x))
                .ToList();

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessInstanceModel>> Get(long id)
        {
            var item = await this.runtimeManager.CreateInstanceQuery()
                .SetId(id)
                .FetchInitiator()
                .SingleAsync();

            if(item != null)
                return ProcessInstanceModel.Create(item);

            return this.NotFound();
        }

        [HttpGet("{id}/activity-instances")]
        public async Task<ActionResult<IEnumerable<ActivityInstanceModel>>> GetActivityInstances(long id)
        {
            var q = await this.context.HistoryManager.GetActivityInstancesAsync(id);
            return q.Select(x => ActivityInstanceModel.Create(x)).ToArray();
        }

        [HttpGet("{id}/variables")]
        public async Task<ActionResult<IDictionary<string, object>>> GetVariables(long id)
        {
            var map = await this.runtimeManager.GetVariablesAsync(id);

            return this.Ok(map);
        }

        [HttpGet("{id}/active-activity-ids")]
        public async Task<ActionResult<IEnumerable<string>>> GetActiveActivityIds(long id)
        {
            var q = await this.context.RuntimeManager.GetActiveActivityIdsAsync(id);
            return q.ToArray();
        }

        [HttpPut("{id}/aborted")]
        public ActionResult Abort(long id)
        {
            return this.Ok();
        }
    }
}
