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
    public class DbBackupController : ControllerBase
    {
        private readonly IDatabaseBackupData _data;

        public DbBackupController(IDatabaseBackupData data)
        {
            _data = data;
        }

        [HttpGet]
        public List<DatabaseBackupModel> Get()
        {
            var result = _data.GetAllBackups();
            return result;
        }

        [HttpGet("Create/")]
        public void CreateBackup()
        {
            _data.CreateBackup();
        }
    }
}
