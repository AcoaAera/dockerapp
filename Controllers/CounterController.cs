using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dockerapi.Controllers
{
    [ApiController]
    [Route("/")]
    public class CounterController : ControllerBase
    {
        public static int count = 0;

        [HttpGet]
        public ActionResult<int> Get()
        {
            count++;
            return count;
        }

        [HttpGet("{count}")]
        public ActionResult<int> GetCount()
        {
            return count;
        }
    }
}