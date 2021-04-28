using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VatController : ControllerBase
    {
        private readonly IVatArchiveData _data;

        public VatController(IVatArchiveData data)
        {
            _data = data;
        }

        // GET: api/<VatController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<VatController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<VatController>
        [HttpPost("Period/")]
        public VatModel GetForPeriod([FromBody] VatModel value)
        {
            return _data.GetByPeriod(value);
        }

        // PUT api/<VatController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VatController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
