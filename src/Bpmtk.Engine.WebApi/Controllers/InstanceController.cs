using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bpmtk.Engine.WebApi.Controllers
{
    [Route("/api/proc-inst")]
    public class InstanceController : ControllerBase
    {
        private readonly IRuntimeService executionService;

        public InstanceController(IRuntimeService executionService)
        {
            this.executionService = executionService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return null;
        }

        [HttpGet("active-activity-ids/{id}")]
        public IActionResult GetActiveActivityIds(long id)
        {
            return null;
        }
    }
}
