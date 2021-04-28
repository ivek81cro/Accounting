using Accounting.DataManager.Models;

namespace Accounting.DataManager.DataAccess
{
    public interface IVatArchiveData
    {
        VatModel GetByPeriod(VatModel vatData);
    }
}