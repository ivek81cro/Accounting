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
    public class BookIraHzzoController : ControllerBase
    {
        private readonly IBookIraHzzoData _data;

        public BookIraHzzoController(IBookIraHzzoData data)
        {
            _data = data;
        }

        // GET: api/<BookIraHzzoController>
        [HttpGet]
        public List<BookIraHzzoModel> Get()
        {
            return _data.GetAll();
        }

        // POST api/<BookIraHzzoController>
        [HttpPost]
        public void Post([FromBody] List<BookIraHzzoModel> payments)
        {
            _data.Insert(payments);
        }
    }
}
