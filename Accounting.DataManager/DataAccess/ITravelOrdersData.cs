using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface ITravelOrdersData
    {
        int InsertLocoCalculation(LocoCalculationModel locoCalculation);
        void InsertLocoOrders(List<LocoOrderModel> locoOrders);
        List<LocoCalculationModel> GetLocoCalculations();
    }
}