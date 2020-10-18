using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace dockerapi.Controllers
{
    [ApiController]
    [Route("/")]
    public class CounterController : ControllerBase
    {
        public static int count = 0;
        IDatabase redis = RedisStore.RedisCache;
        string intKey = "mykey";

        [HttpGet]
        public ActionResult<int> Get()
        {
            //count++;
            //return count;
            var result = (int)redis.StringIncrement(intKey);
            return result;
        }

        [HttpGet("{count}")]
        public ActionResult<int> GetCount()
        {
            return count;
        }
    }
}