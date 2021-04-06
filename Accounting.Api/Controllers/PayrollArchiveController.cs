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
    public class PayrollArchiveController : ControllerBase
    {
        private readonly IPayrollArchiveData _payrollArchive;

        public PayrollArchiveController(IPayrollArchiveData payrollArchive)
        {
            _payrollArchive = payrollArchive;
        }

        [HttpGet("Headers/")]
        public List<PayrollArchiveHeaderModel> Get()
        {
            return _payrollArchive.GetHeaders();
        }

        // GET: api/<PayrollArchiveController>
        [HttpGet("IfExists/{identifier}")]
        public bool Get(string identifier)
        {
            return _payrollArchive.IfExists(identifier);
        }

        [HttpGet("Payrolls/{accountingId}")]
        public List<PayrollArchivePayrollModel> GetPayrolls(int accountingId)
        {
            return _payrollArchive.GetArchivePayrolls(accountingId);
        }
        
        [HttpGet("Supplements/{accountingId}")]
        public List<PayrollArchiveSupplementModel> GetSupplements(int accountingId)
        {
            return _payrollArchive.GetArchiveSupplements(accountingId);
        }

        // POST api/<PayrollArchiveController>
        [HttpPost]
        public void Post([FromBody] PayrollArchiveModel archive)
        {
            _payrollArchive.Insert(archive);
        }

        // DELETE api/<PayrollArchiveController>/5
        [HttpDelete("{accountingId}")]
        public void Delete(int accountingId)
        {
            _payrollArchive.DeleteRecord(accountingId);
        }
    }
}
