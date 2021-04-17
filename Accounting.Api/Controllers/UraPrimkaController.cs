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
        private readonly IBookUraPrimkaData _primka;

        public UraPrimkaController(IBookUraPrimkaData primka)
        {
            _primka = primka;
        }

        // GET: api/<UraPrimkaController>
        [HttpGet]
        public List<BookUraPrimkaModel> Get()
        {
            return _primka.GetAll();
        }

        // POST api/<UraPrimkaController>
        [HttpPost]
        public void Post([FromBody] List<BookUraPrimkaModel> primke)
        {
            _primka.InsertPrimke(primke);
        }
    }
}
