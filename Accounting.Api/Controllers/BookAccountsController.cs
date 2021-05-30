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

        [HttpPost("ByNumber/")]
        public BookAccountModel GetByNumber([FromBody] string number)
        {
            return _bookAccount.GetByNumber(number);
        }

        // POST api/<BookIraController>
        [HttpPost]
        public void Post([FromBody] BookAccountModel account)
        {
            if (_bookAccount.Exists(account))
            {
                _bookAccount.Update(account);
            }
            else
            {
                _bookAccount.Insert(account);
            }
        }
    }
}
