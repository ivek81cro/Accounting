using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceSheetData _data;

        public BalanceController(IBalanceSheetData data)
        {
            _data = data;
        }

        // GET: api/<BalanceController>
        [HttpGet]
        public List<BalanceSheetModel> Get()
        {
            return _data.GetBalance();
        }

        // GET api/<BalanceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BalanceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BalanceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BalanceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
