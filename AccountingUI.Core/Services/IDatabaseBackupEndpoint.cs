using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IDatabaseBackupEndpoint
    {
        Task<List<DatabaseBackupModel>> GetAll();
    }
}