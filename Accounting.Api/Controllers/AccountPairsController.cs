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
    public class AccountPairsController : ControllerBase
    {
        private readonly IAccountPairData _data;

        public AccountPairsController(IAccountPairData data)
        {
            _data = data;
        }

        // GET api/<AccountPairsController>/5
        [HttpGet("{bookName}")]
        public List<AccountPairModel> Get(string bookName)
        {
            return _data.GetByBookName(bookName);
        }

        // POST api/<AccountPairsController>
        [HttpPost]
        public void Post([FromBody] AccountPairModel pair)
        {
            _data.Insert(pair);
        }
    }
}
