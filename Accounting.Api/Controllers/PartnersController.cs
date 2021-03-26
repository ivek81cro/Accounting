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
    public class PartnersController : ControllerBase
    {
        private readonly IPartnersData _partnersData;

        public PartnersController(IPartnersData partnersData)
        {
            _partnersData = partnersData;
        }

        // GET: api/<PartnersController>
        [HttpGet]
        public List<PartnersModel> Get()
        {
            return _partnersData.GetPartners();
        }

        // GET api/<PartnersController>/5
        [HttpGet("{id}")]
        public PartnersModel Get(int id)
        {
            return _partnersData.GetPartnersById(id);
        }

        // POST api/<PartnersController>
        [HttpPost]
        public void Post([FromBody] PartnersModel partner)
        {
            if (partner.Id == 0)
            {
                _partnersData.InsertPartner(partner);
            }
            else
            {
                _partnersData.UpdatePartner(partner);
            }
        }

        // PUT api/<PartnersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<PartnersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _partnersData.DeletePartner(id);
        }
    }
}
