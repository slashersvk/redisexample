using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace RedisExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private DistributedCacheEntryOptions _entryOptions;

        public WeatherForecastController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;

            _entryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = null,
                AbsoluteExpirationRelativeToNow = null,
                SlidingExpiration = new TimeSpan(0,30,0)
            };
        }

        [HttpGet("/{age}")]
        public async Task<Animal> Get(int age)
            => await _distributedCache.GetAsync(GetKey(age));

        [HttpPost("/{age}")]
        public async Task Post(int age)
        {
            var animal = new Animal { Age = age };
            await _distributedCache.SetAsync(GetKey(age), animal, _entryOptions);
        }

        private string GetKey(int age)
            => $"animal:{age}"; 
    }

    public class Animal
    {
        public int Age { get; set; }
    }
}
