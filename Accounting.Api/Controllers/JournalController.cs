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
    public class JournalController : ControllerBase
    {
        private readonly IAccountingJournalData _data;

        public JournalController(IAccountingJournalData data)
        {
            _data = data;
        }

        [HttpGet]
        public List<AccountingJournalModel> Get()
        {
            return _data.GetHeaders();
        }

        [HttpGet("Unprocessed/")]
        public List<AccountingJournalModel> GetUnprocessed()
        {
            return _data.GetUnprocessedHeaders();
        }

        [HttpGet("Ledger/")]
        public List<AccountingJournalModel> LoadLedger()
        {
            return _data.LoadLedger();
        }

        [HttpGet("Processed/")]
        public List<AccountingJournalModel> GetProcessed()
        {
            return _data.GetProcessedHeaders();
        }

        [HttpGet("Latest/")]
        public int GetLatestNumber()
        {
            return _data.LatestNumber();
        }

        [HttpGet("Card/{accountNumber}")]
        public List<AccountBalanceModel> GetAccountCard(string accountNumber)
        {
            return _data.GetAccountBalance(accountNumber);
        }

        [HttpPost("Details/")]
        public List<AccountingJournalModel> GetDetails([FromBody] AccountingJournalModel model)
        {
            return _data.GetJournalDetails(model);
        }

        [HttpPost("Delete/")]
        public void DeleteEntries([FromBody] AccountingJournalModel model)
        {
            _data.DeleteJournal(model);
        }

        // GET api/<JournalController>/5
        [HttpGet("{bookNumber}")]
        public List<AccountingJournalModel> Get(int bookNumber)
        {
            return _data.GetByBookNumber(bookNumber);
        }

        // POST api/<JournalController>
        [HttpPost]
        public void Post([FromBody] List<AccountingJournalModel> list)
        {
            _data.Insert(list);
        }

        [HttpPost("Update/")]
        public void Update([FromBody] List<AccountingJournalModel> list)
        {
            DeleteEntries(list.ElementAt(0));
            _data.Insert(list);
        }
    }
}
