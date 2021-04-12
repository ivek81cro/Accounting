using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoppdEmployeeController : ControllerBase
    {
        private readonly IJoppdEmployeeData _joppdEmployee;

        public JoppdEmployeeController(IJoppdEmployeeData joppdEmployee)
        {
            _joppdEmployee = joppdEmployee;
        }

        // GET: api/<JoppdEmployeeController>
        [HttpGet]
        public List<JoppdEmployeeModel> Get()
        {
            return _joppdEmployee.GetAll();
        }

        // GET api/<JoppdEmployeeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<JoppdEmployeeController>
        [HttpPost]
        public void Post([FromBody] JoppdEmployeeModel employee)
        {
            _joppdEmployee.SaveData(employee);
        }

        // PUT api/<JoppdEmployeeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JoppdEmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
