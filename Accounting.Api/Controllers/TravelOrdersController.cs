using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TravelOrdersController : ControllerBase
    {
        private readonly ITravelOrdersData _data;

        public TravelOrdersController(ITravelOrdersData travelOrdersData)
        {
            _data = travelOrdersData;
        }

        // POST api/<TravelOrders>
        [HttpPost("LocoTravel/")]
        public void Post([FromBody] TravelOrdersLocoModel locoTravel)
        {
            int result = _data.InsertLocoCalculation(locoTravel.LocoCalculation);
            if (result != 0)
            {
                foreach(var item in locoTravel.LocoOrders)
                {
                    item.CalculationId = result;
                }
                _data.InsertLocoOrders(locoTravel.LocoOrders);
            }
        }
    }
}
