using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class JoppdEmployeeData : IJoppdEmployeeData
    {
        private readonly ISqlDataAccess _sql;

        public JoppdEmployeeData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<JoppdEmployeeModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<JoppdEmployeeModel, dynamic>("dbo.spJoppdEmployee_GetAll", new { });

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void SaveData(JoppdEmployeeModel employee)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                _sql.SaveDataInTransaction("dbo.spJoppdEmployee_Update", employee);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
