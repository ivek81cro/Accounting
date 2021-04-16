using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
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

        // GET api/<UraPrimkaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UraPrimkaController>
        [HttpPost]
        public void Post([FromBody] List<BookUraPrimkaModel> primke)
        {
            _primka.InsertPrimke(primke);
        }

        // PUT api/<UraPrimkaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UraPrimkaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
