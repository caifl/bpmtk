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

        public TaskController(IContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Filter Tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<TaskModel>> Get()
        {
            var query = this.context.TaskManager.Tasks;

            //var q = query.GroupBy(x => x.Key)
            //    .Select(x => new
            //    {
            //        key = x.Key,
            //        version = x.Max(y => y.Version)
            //    });

            //var q2 = from item in query
            //         join b in q on new
            //         {
            //             key = item.Key,
            //             version = item.Version
            //         } equals b
            //         select item;

            var data = query.Select(x => TaskModel.Create(x)).ToArray();

            return data;
        }

        /// <summary>
        /// Find the specified Task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> Get(int id)
        {
            var item = await this.context.TaskManager.FindTaskAsync(id);
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

        [HttpPut("{id}/rendered-form")]
        public ActionResult<string> Render(long id)
        {
            return "<div>test</div>";
        }

        [HttpPut("{id}/completed")]
        public ActionResult Complete(long id, [FromBody] CompleteTaskModel model)
        {
            return this.Ok();
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
