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
    public class PayrollArchiveController : ControllerBase
    {
        private readonly IPayrollArchiveData _payrollArchive;

        public PayrollArchiveController(IPayrollArchiveData payrollArchive)
        {
            _payrollArchive = payrollArchive;
        }

        // GET: api/<PayrollArchiveController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PayrollArchiveController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PayrollArchiveController>
        [HttpPost]
        public void Post([FromBody] PayrollArchiveModel archive)
        {
            _payrollArchive.Insert(archive);
        }

        // DELETE api/<PayrollArchiveController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
