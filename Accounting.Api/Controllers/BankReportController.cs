﻿using Accounting.DataManager.DataAccess;
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
    public class BankReportController : ControllerBase
    {
        private readonly IBankReportData _data;

        public BankReportController(IBankReportData data)
        {
            _data = data;
        }

        [HttpGet]
        public List<BankReportModel> Get()
        {
            return _data.GetAllHeaders();
        }

        // GET: api/<BankReportController>
        [HttpGet("{reportNumber}")]
        public int Get(int reportNumber)
        {
            return _data.GetHeaderId(reportNumber);
        }

        [HttpGet("GetItems/{headerId}")]
        public List<BankReportItemModel> GetItems(int HeaderId)
        {
            return _data.GetItems(HeaderId);
        }

        [HttpGet("GetProcessed/")]
        public List<int> GetProcessed()
        {
            return _data.GetProcessedReports();
        }

        [HttpGet("GetHeader/{id}")]
        public BankReportItemModel GetHeader(int id)
        {
            return _data.GetHeader(id);
        }

        // POST api/<BankReportController>
        [HttpPost("Header/")]
        public void PostHeader([FromBody] BankReportModel reportHeader)
        {
            if (reportHeader.Id != 0)
            {
                _data.Delete(reportHeader.Id);
            }
            _data.InsertHeader(reportHeader);
        }

        // POST api/<BankReportController>
        [HttpPost("Update/")]
        public void UpdateHeader([FromBody] BankReportModel reportHeader)
        {
            _data.UpdateHeader(reportHeader);
        }

        [HttpPost("Items/")]
        public void PostItems([FromBody] List<BankReportItemModel> reportItems)
        {
            _data.InsertItems(reportItems);
        }

        // DELETE api/<BankReportController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _data.Delete(id);
        }
    }
}
