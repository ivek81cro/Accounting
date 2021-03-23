using Accounting.DataManager.Models;

namespace Accounting.DataManager.DataAccess
{
    public interface ICompanyData
    {
        CompanyModel GetCompany();
        void InsertCompany(CompanyModel company);
        void UpdateCompany(CompanyModel company);
    }
}