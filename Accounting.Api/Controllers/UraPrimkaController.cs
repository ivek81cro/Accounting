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
    public class UraPrimkaController : ControllerBase
    {
        private readonly IBookUraPrimkaData _data;

        public UraPrimkaController(IBookUraPrimkaData primka)
        {
            _data = primka;
        }

        // GET: api/<UraPrimkaController>
        [HttpGet]
        public List<BookUraPrimkaModel> Get()
        {
            return _data.GetAll();
        }

        // POST api/<UraPrimkaController>
        [HttpPost]
        public void Post([FromBody] List<BookUraPrimkaModel> primke)
        {
            _data.InsertPrimke(primke);
        }

        [HttpPost("Processed/")]
        public void Post([FromBody] int number)
        {
            _data.SetProcessed(number);
        }
    }
}
