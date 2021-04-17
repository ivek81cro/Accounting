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
    public class UraReproController : ControllerBase
    {
        private readonly IBookUraReproData _primka;

        public UraReproController(IBookUraReproData primka)
        {
            _primka = primka;
        }

        [HttpGet]
        public List<BookUraPrimkaReproModel> Get()
        {
            return _primka.GetAll();
        }

        // POST api/<UraPrimkaController>
        [HttpPost]
        public void Post([FromBody] List<BookUraPrimkaReproModel> primke)
        {
            _primka.InsertPrimke(primke);
        }
    }
}
