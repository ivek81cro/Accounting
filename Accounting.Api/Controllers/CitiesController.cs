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
    public class CitiesController : ControllerBase
    {
        private readonly ICityData _cityData;

        public CitiesController(ICityData cityData)
        {
            _cityData = cityData;
        }

        // GET: api/<CitiesController>
        [HttpGet]
        public List<CityModel> Get()
        {
            return _cityData.GetCities();
        }

        // GET api/<CitiesController>/5
        [HttpGet("int/{id}")]
        public CityModel Get(int id)
        {
            return _cityData.GetCityById(id);
        }

        // GET api/<CitiesController>/Zagreb
        [HttpGet("string/{mjesto}")]
        public CityModel Get(string mjesto)
        {
            return _cityData.GetCityByName(mjesto);
        }

        // POST api/<CitiesController>
        [HttpPost]
        public void Post([FromBody] CityModel city)
        {
            if (city.Id == 0)
            {
                _cityData.InsertCity(city);
            }
            else
            {
                _cityData.UpdateCity(city);
            }
        }

        // DELETE api/<CitiesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _cityData.DeleteCity(id);
        }
    }
}
