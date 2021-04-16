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
    public class BookAccountsController : ControllerBase
    {
        private readonly IBookAccountData _bookAccount;

        public BookAccountsController(IBookAccountData bookAccount)
        {
            _bookAccount = bookAccount;
        }

        // GET: api/<BookAccountsController>
        [HttpGet]
        public List<BookAccountModel> Get()
        {
            return _bookAccount.GetAll();
        }

        // GET api/<BookAccountsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BookAccountsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BookAccountsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BookAccountsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
