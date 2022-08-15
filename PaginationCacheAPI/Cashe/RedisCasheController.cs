using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaginationCacheAPI
{
    [ApiController]
    [Route("proteinapi/v1/api/[controller]")]
    public class RedisCasheController : ControllerBase
    {
        private readonly IDistributedCache distributedCache;

        public RedisCasheController(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<Catalog>> GetRedis(int id)
        {
            string cacheKey = id.ToString();
            IEnumerable<Catalog> cities;
            string json;


            var citiesFromCache = await distributedCache.GetAsync(cacheKey);
            if (citiesFromCache != null)
            {
                json = Encoding.UTF8.GetString(citiesFromCache);
                cities = JsonConvert.DeserializeObject<List<Catalog>>(json);
                return cities;
            }
            else
            {
                List<Catalog> tempList = new List<Catalog> { new Catalog { Name = "Prod 1 ", Published = true }, new Catalog { Name = "Prod 2", Published = true }, new Catalog { Name = "Prod 3", Published = true } };

                json = JsonConvert.SerializeObject(tempList);
                citiesFromCache = Encoding.UTF8.GetBytes(json);
                var options = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(1)) // belirli bir süre erişilmemiş ise expire eder
                        .SetAbsoluteExpiration(DateTime.Now.AddMonths(1)); // belirli bir süre sonra expire eder.
                await distributedCache.SetAsync(cacheKey, citiesFromCache, options);
                return tempList;
            }
        }



        [HttpDelete]
        public ActionResult DeleteCache(int id)
        {
            // remove cashe
            distributedCache.Remove(id.ToString());
            return Ok();
        }

        [HttpGet("GetKey")]
        public string GetKey(string key)
        {
            return GetRedisValue(key);
        }


        private string GetRedisValue(string key)
        {
            var configurationOptions = new ConfigurationOptions();
            configurationOptions.EndPoints.Add("192.168.18.167", Convert.ToInt32("6379"));
            var redisConnection = ConnectionMultiplexer.Connect(configurationOptions);
            var Server = redisConnection.GetServer("192.168.18.167", Convert.ToInt32("6379"));
            var db = redisConnection.GetDatabase(0);

            var redisValue = db.StringGet(key);
            var response = redisValue.ToString();
            return response;
        }
    }
}
