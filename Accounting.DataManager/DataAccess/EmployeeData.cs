using Accounting.DataManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace Accounting.DataManager.DataAccess
{
    public class EmployeeData : IEmployeeData
    {
        private ISqlDataAccess _sql;

        public EmployeeData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<EmployeeModel> GetEmployees()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<EmployeeModel, dynamic>("dbo.spEmployee_GetAll", new { });

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public EmployeeModel GetEmployeeById(int id)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<EmployeeModel, dynamic>("dbo.spEmployee_GetById", new { Id = id })
                        .FirstOrDefault();

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public EmployeeModel GetEmployeeByOib(string oib)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<EmployeeModel, dynamic>("dbo.spEmployee_GetByOib", new { Oib = oib })
                        .FirstOrDefault();

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void InsertEmployee(EmployeeModel employee)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spEmployee_Insert", employee);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void UpdateEmployee(EmployeeModel employee)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spEmployee_Update", employee);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void DeleteEmployee(int id)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.LoadDataInTransaction<EmployeeModel, dynamic>("dbo.spEmployee_Delete", new { Id = id });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
