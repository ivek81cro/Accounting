﻿using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeData _employeeData;

        public EmployeeController(IEmployeeData employeeData)
        {
            _employeeData = employeeData;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public List<EmployeeModel> Get()
        {
            return _employeeData.GetEmployees();
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public EmployeeModel Get(int id)
        {
            return _employeeData.GetEmployeeById(id);
        }

        // GET api/<EmployeeController>/12345678901
        [HttpGet("Oib/{oib}")]
        public EmployeeModel Get(string oib)
        {
            return _employeeData.GetEmployeeByOib(oib);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public void Post([FromBody] EmployeeModel employee)
        {
            if (employee.Id == 0)
            {
                _employeeData.InsertEmployee(employee);
            }
            else
            {
                _employeeData.UpdateEmployee(employee);
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _employeeData.DeleteEmployee(id);
        }
    }
}
