using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace PaginationCacheAPI
{
    public class Catalog
    {
        public string Name { get; set; }
        public bool Published { get; set; }
    }

    [ApiController]
    [Route("proteinapi/v1/api/[controller]")]
    public class ResponseCasheController : ControllerBase
    {
        private readonly ILogger<ResponseCasheController> logger;

        public ResponseCasheController(ILogger<ResponseCasheController> logger)
        {
            this.logger = logger;
        }



        // duration second
        // [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
        [ResponseCache(CacheProfileName = "Duration60")]
        [HttpPost]
        public List<Catalog> Get()
        {
            List<Catalog> catList = new List<Catalog> { new Catalog { Name = "Prod 1 ", Published = true }, new Catalog { Name = "Prod 2", Published = true }, new Catalog { Name = "Prod 3", Published = true } };
            return catList;
        }




    }
}
