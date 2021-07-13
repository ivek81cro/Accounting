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
    public class BookIraController : ControllerBase
    {
        private readonly IBookIraData _data;

        public BookIraController(IBookIraData data)
        {
            _data = data;
        }

        // GET: api/<BookIraController>
        [HttpGet]
        public List<BookIraModel> Get()
        {
            return _data.GetAll();
        }

        // POST api/<BookIraController>
        [HttpPost]
        public void Post([FromBody] List<BookIraModel> list)
        {
            _data.Insert(list);
        }

        [HttpPost("Processed/")]
        public void Post([FromBody] int iraNumber)
        {
            _data.SetProcessed(iraNumber);
        }

        [HttpPost("Update/")]
        public void Update([FromBody] BookIraModel item)
        {
            _data.Update(item);
        }

        [HttpPost("UpdateHzzo/")]
        public void UpdateHzzo([FromBody] BookIraHzzoModel item)
        {
            _data.UpdateHzzo(item);
        }
    }
}
