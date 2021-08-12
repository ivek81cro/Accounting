using Accounting.DataManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace Accounting.DataManager.DataAccess
{
    public class TravelOrdersData : ITravelOrdersData
    {
        private readonly ISqlDataAccess _sql;

        public TravelOrdersData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public int InsertLocoCalculation(LocoCalculationModel locoCalculation)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                int output = _sql.LoadDataInTransaction<int, dynamic>("dbo.spLocoTravelCalc_Insert", locoCalculation).FirstOrDefault();

                _sql.CommitTransaction();
                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                return 0;
            }
        }

        public void InsertLocoOrders(List<LocoOrderModel> locoOrders)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spLocoTravelOrders_Insert", locoOrders);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<LocoCalculationModel> GetLocoCalculations()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<LocoCalculationModel, dynamic>("dbo.spLocoTravelOrders_GetAll", new { });

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
