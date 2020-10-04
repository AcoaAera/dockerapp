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
        public static int count;

        [HttpGet]
        public ActionResult<int> Get()
        {
            count = count + 1;
            return count;
        }
    }
}