using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CashRegisterController : ControllerBase
    {
        private readonly ICashRegisterData _data;

        public CashRegisterController(ICashRegisterData data)
        {
            _data = data;
        }

        [HttpGet]
        public List<CashRegisterModel> Get()
        {
            return _data.GetAll();
        }

        [HttpPost]
        public void Post([FromBody] List<CashRegisterModel> list)
        {
            _data.InsertItems(list);
        }

        [HttpPost("Processed/")]
        public void Post([FromBody] int number)
        {
            _data.SetProcessed(number);
        }
    }
}
