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
    public class BookSettingsController : ControllerBase
    {
        private readonly IBookAccountSettingsData _settings;

        public BookSettingsController(IBookAccountSettingsData settings)
        {
            _settings = settings;
        }

        // GET api/<BookSettingsController>/5
        [HttpGet("{name}")]
        public List<BookAccountsSettingsModel> Get(string name)
        {
            return _settings.Get(name);
        }

        // POST api/<BookSettingsController>
        [HttpPost]
        public void Post([FromBody] BookAccountsSettingsModel setting)
        {
            _settings.Insert(setting);
        }

        // PUT api/<BookSettingsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BookSettingsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _settings.Delete(id);
        }
    }
}
