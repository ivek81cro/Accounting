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
    public class PayrollAccountingController : ControllerBase
    {
        private readonly IPayrollAccountingData _accounting;

        public PayrollAccountingController(IPayrollAccountingData accounting)
        {
            _accounting = accounting;
        }

        // GET: api/<PayrollAccountingController>
        [HttpGet]
        public List<PayrollAccountingModel> Get()
        {
            return _accounting.GetAll();
        }

        // GET api/<PayrollAccountingController>/5
        [HttpGet("{id}")]
        public PayrollAccountingModel Get(int id)
        {
            return _accounting.GetById(id);
        }

        // POST api/<PayrollAccountingController>
        [HttpPost]
        public void Post([FromBody] PayrollAccountingModel payroll)
        {
            if (payroll.Id != 0)
            {
                _accounting.Update(payroll);
            }
            else
            {
                _accounting.Insert(payroll);
            }
        }

        // DELETE api/<PayrollAccountingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _accounting.Delete(id);
        }
    }
}
