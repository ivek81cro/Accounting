using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceSheetData _data;

        public BalanceController(IBalanceSheetData data)
        {
            _data = data;
        }

        // GET: api/<BalanceController>
        [HttpGet]
        public List<BalanceSheetModel> Get()
        {
            return _data.GetBalance();
        }

        // GET api/<BalanceController>/{dateFrom, dateTo}
        [HttpPost]
        public List<BalanceSheetModel> Post([FromBody] List<DateTime> dates)
        {
            return _data.GetBalancePeriod(dates);
        }
    }
}
