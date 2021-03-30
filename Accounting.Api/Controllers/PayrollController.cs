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
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollData _payrollData;

        public PayrollController(IPayrollData payrollData)
        {
            _payrollData = payrollData;
        }

        // GET: api/<PayrollController>
        [HttpGet]
        public List<PayrollModel> Get()
        {
            return _payrollData.GetAll();
        }

        // GET api/<PayrollController>/5
        [HttpGet("{id}")]
        public PayrollModel Get(string oib)
        {
            return _payrollData.GetByOib(oib);
        }

        // POST api/<PayrollController>
        [HttpPost]
        public void Post([FromBody] PayrollModel payroll)
        {
            if (Get(payroll.Oib) == null)
            {
                _payrollData.InsertPayroll(payroll);
            }
            else
            {
                _payrollData.UpdatePayroll(payroll);
            }
        }

        // PUT api/<PayrollController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PayrollController>/5
        [HttpDelete("{id}")]
        public void Delete(string oib)
        {
            _payrollData.DeletePayroll(oib);
        }
    }
}
