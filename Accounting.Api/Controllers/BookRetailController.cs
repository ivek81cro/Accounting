using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookRetailController : ControllerBase
    {
        private readonly IBookIraRetailData _data;

        public BookRetailController(IBookIraRetailData data)
        {
            _data = data;
        }

        // GET: api/<BookRetailController>
        [HttpGet]
        public List<RetailIraModel> Get()
        {
            return _data.GetAll();
        }

        // POST api/<BookRetailController>
        [HttpPost]
        public void Post([FromBody] List<RetailIraModel> list)
        {
            _data.Insert(list);
        }


        [HttpPost("Processed/")]
        public void Post([FromBody] int number)
        {
            _data.SetProcessed(number);
        }
    }
}
