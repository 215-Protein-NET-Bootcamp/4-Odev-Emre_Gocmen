using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace PaginationCacheAPI
{  

    [ApiController]
    [Route("protein/v1/api/[controller]")]
    public class MemoryCasheController : ControllerBase
    {
        const string cacheKey = "catalogKey";
        private readonly IMemoryCache memoryCache;


        public MemoryCasheController( IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public List<Catalog> Get()
        {

            if (!memoryCache.TryGetValue(cacheKey, out List<Catalog> casheList))
            {
                List<Catalog> catList = new List<Catalog> { new Catalog { Name = "Prod 1 ", Published = true }, new Catalog { Name = "Prod 2", Published = true }, new Catalog { Name = "Prod 3", Published = true } };

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                    Priority = CacheItemPriority.Normal
                };

                // set cashe
                memoryCache.Set(cacheKey, catList, cacheExpOptions);
                return catList;
            }
            return casheList;
        }

        [HttpDelete]
        public ActionResult DeleteCache()
        {
            // remove cashe
            memoryCache.Remove(cacheKey);
            return Ok();
        }


    }
}
