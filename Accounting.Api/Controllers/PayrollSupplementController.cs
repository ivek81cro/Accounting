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
    public class PayrollSupplementController : ControllerBase
    {
        private readonly ISupplementData _supplement;
        public PayrollSupplementController(ISupplementData supplement)
        {
            _supplement = supplement;
        }

        // GET: api/<PayrollSupplementController>
        [HttpGet]
        public List<PayrollSupplementModel> Get()
        {
            return _supplement.GetAll();
        }
    }
}
