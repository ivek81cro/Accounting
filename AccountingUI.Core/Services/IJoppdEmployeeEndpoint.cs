using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IJoppdEmployeeEndpoint
    {
        Task<List<JoppdEmployeeModel>> GetAll();
        Task<bool> PostJoppdData(JoppdEmployeeModel employee);
    }
}