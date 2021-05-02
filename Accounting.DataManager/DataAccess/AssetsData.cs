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

        public List<AssetModel> Get(string assetType)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<AssetModel, dynamic>("dbo.spAssets_GetAll", new { VrstaPoTrajanju = assetType });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Insert(AssetModel asset)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spAssets_Insert", asset);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Update(AssetModel asset)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spAssets_Update", asset);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
