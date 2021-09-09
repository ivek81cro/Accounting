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
    public class DbBackup : ControllerBase
    {
        private readonly IDatabaseBackupData _data;

        public DbBackup(IDatabaseBackupData data)
        {
            _data = data;
        }

        [HttpGet]
        public List<DatabaseBackupModel> Get()
        {
            var result = _data.GetAllBackups();
            return result;
        }
    }
}
