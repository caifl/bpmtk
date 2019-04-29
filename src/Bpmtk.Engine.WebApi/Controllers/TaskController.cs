using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bpmtk.Engine.WebApi.Controllers
{
    /// <summary>
    /// Task APIs
    /// </summary>
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IContext context;
        private readonly ITaskManager taskManager;

        public TaskController(IContext context)
        {
            this.context = context;
            this.taskManager = context.TaskManager;
        }

        /// <summary>
        /// Filter Tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<ActionResult<PagedResult<TaskModel>>> Get(TaskFilter filter)
        {
            var result = new PagedResult<TaskModel>();

            var query = this.taskManager.CreateQuery()
                .FetchAssignee();

            if (filter.AssigneeId != null)
                query.SetAssignee(filter.AssigneeId.Value);

            var list = await query.ListAsync(filter.Page, filter.PageSize);

            result.Count = await query.CountAsync();
            result.Items = list.Select(x => TaskModel.Create(x)).ToList();
            result.Page = filter.Page;
            result.PageSize = filter.PageSize;

            return result;
        }

        /// <summary>
        /// Find the specified Task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> Get(int id)
        {
            var item = await this.taskManager.CreateQuery()
                .FetchAssignee()
                .SetId(id)
                .SingleAsync();
            if (item != null)
                return TaskModel.Create(item);

            return this.NotFound();
        }

        /// <summary>
        /// Gets IdentityLinks of Task.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/identity-links")]
        public ActionResult<IEnumerable<IdentityLinkModel>> GetIdentityLinks(int id)
        {
            var q = this.context.DbSession.IdentityLinks
                .Where(x => x.Task.Id == id)
                .Select(x => new IdentityLinkModel
                {
                    Id = x.Id,
                    UserId = x.User.Id,
                    UserName = x.User.UserName,
                    GroupId = x.Group.Id,
                    GroupName = x.Group.Name,
                    Type = x.Type
                }).ToArray();
            
            return q;
        }

        [HttpGet("{id}/rendered-form")]
        public ActionResult<string> Render(long id)
        {
            return "<div>test</div>";
        }

        [HttpPut("{id}/completed")]
        public async Task<ActionResult> Complete(long id, CompleteTaskModel model)
        {
            try
            {
                await this.context.TaskManager.CompleteAsync(id, model.Variables);

                return this.Ok();
            }
            catch(Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }            
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
