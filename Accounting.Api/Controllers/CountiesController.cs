using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountiesController : ControllerBase
    {
        private readonly ICountyData _countyData;

        public CountiesController(ICountyData countyData)
        {
            _countyData = countyData;
        }

        // GET: api/<CountiesController>
        [HttpGet]
        public List<CountyModel> Get()
        {
            return _countyData.GetCounties();
        }

        // GET api/<CountiesController>/5
        [HttpGet("{id}")]
        public CountyModel Get(int id)
        {
            return _countyData.GetCountyById(id);
        }

        // POST api/<CountiesController>
        [HttpPost]
        public void Post([FromBody] CountyModel county)
        {
            _countyData.InsertCounty(county);
        }

        // DELETE api/<CountiesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _countyData.DeleteCounty(id);
        }
    }
}
