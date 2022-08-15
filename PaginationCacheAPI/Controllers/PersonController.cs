using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaginationCacheAPI
{
    [Route("protein/v1/api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonRepository personRepository;
        private readonly IMemoryCache memoryCache;
        IMapper mapper;

        public PersonController(PersonRepository personRepository, IMapper mapper, IMemoryCache memoryCache)
        {
            this.personRepository = personRepository;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
        }

        const string cacheKey = "catalogKey";

        [HttpGet]
        public async Task<IActionResult> GetPaginationAsync([FromQuery] int page, [FromQuery] int pageSize)
        {
            Log.Information($"{User.Identity?.Name}: get pagination person.");
            //PersonDto query = new PersonDto()
            //{
            //    StaffId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value,
            //    CreatedBy = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value
            //};

            var result = new PaginationResponse<IEnumerable<PersonDto>>(true);

            QueryResource pagination = new QueryResource(page, pageSize);

            if (!memoryCache.TryGetValue(cacheKey, out List<Person> casheList))
            {
                var allOfPerson = await personRepository.GetQueryAble();

                var storable = allOfPerson.ToList();

                var records = allOfPerson
                    .OrderBy(x => x.Id)
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToList();

                var total = allOfPerson.Count();

                // Mapping
                var tempResource = mapper.Map<IEnumerable<Person>, IEnumerable<PersonDto>>(records);

                var resource = new PaginationResponse<IEnumerable<PersonDto>>(tempResource);

                // Using extension-method for pagination
                resource.CreatePaginationResponse(pagination, total);

                result = resource;

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                    Priority = CacheItemPriority.Normal
                };

                memoryCache.Set(cacheKey, storable, cacheExpOptions);

                return Ok(result);
            }

            var records2 = casheList.OrderBy(x => x.Id)
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            var total2 = records2.Count();
            var tempResource2 = mapper.Map<IEnumerable<Person>, IEnumerable<PersonDto>>(records2);
            var resource2 = new PaginationResponse<IEnumerable<PersonDto>>(tempResource2);
            resource2.CreatePaginationResponse(pagination, total2);
            result = resource2;
            

            if (!result.Success)
                return BadRequest(result);

            if (result.Response is null)
                return NoContent();

            return Ok(result);
        }
    }
}
