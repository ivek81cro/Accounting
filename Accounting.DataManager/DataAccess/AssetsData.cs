using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class AssetsData : IAssetsData
    {
        private readonly ISqlDataAccess _sql;

        public AssetsData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<AssetModel> Get()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<AssetModel, dynamic>("dbo.spAssets_GetAll", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
