using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyData _company;

        public CompanyController(ICompanyData company)
        {
            _company = company;
        }

        // GET: api/<CompanyController>
        [HttpGet]
        public CompanyModel Get()
        {
            return _company.GetCompany();
        }
        
        // POST api/<CompanyController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<CompanyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
