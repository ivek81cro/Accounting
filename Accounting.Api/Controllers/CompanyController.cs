using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public void Post([FromBody] CompanyModel company)
        {
            if (company.Id == 0)
            {
                _company.InsertCompany(company);
            }
            else
            {
                _company.UpdateCompany(company);
            }
        }

        // PUT api/<CompanyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
