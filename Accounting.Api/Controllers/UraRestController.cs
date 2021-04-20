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
    public class UraRestController : ControllerBase
    {
        private readonly IBookUraRestData _data;

        public UraRestController(IBookUraRestData data)
        {
            _data = data;
        }

        // GET: api/<UraRestController>
        [HttpGet]
        public List<BookUraRestModel> Get()
        {
            return _data.GetAll();
        }

        // GET: api/<UraRestController>
        [HttpGet("Discounts/")]
        public List<BookUraRestModel> GetDiscounts()
        {
            return _data.GetDiscounts();
        }

        // POST api/<UraRestController>
        [HttpPost]
        public void Post([FromBody] List<BookUraRestModel> list)
        {
            _data.Insert(list);
        }

        // DELETE api/<UraRestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
