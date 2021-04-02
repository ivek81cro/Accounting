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
    public class EmployeeSupplementController : ControllerBase
    {
        private readonly IEmployeeSupplementData _employeeSupp;

        public EmployeeSupplementController(IEmployeeSupplementData employeeSupp)
        {
            _employeeSupp = employeeSupp;
        }

        // GET: api/<EmployeeSupplementController>
        [HttpGet]
        public List<PayrollSupplementEmployeeModel> Get()
        {
            return _employeeSupp.GetAll();
        }

        // GET api/<EmployeeSupplementController>/5
        [HttpGet("{oib}")]
        public List<PayrollSupplementEmployeeModel> Get(string oib)
        {
            return _employeeSupp.GetByOib(oib);
        }

        // POST api/<EmployeeSupplementController>
        [HttpPost]
        public void Post([FromBody] PayrollSupplementEmployeeModel supp)
        {
            _employeeSupp.InsertSupplement(supp);
        }

        // DELETE api/<EmployeeSupplementController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _employeeSupp.DeleteSupplement(id);
        }
    }
}
