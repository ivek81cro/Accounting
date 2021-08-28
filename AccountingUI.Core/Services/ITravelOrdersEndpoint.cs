using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface ITravelOrdersEndpoint
    {
        Task<bool> PostLocoTravel(TravelOrdersLocoModel locoTravelOrder);
        Task<List<LocoCalculationModel>> GetLocoCalculations();
        Task<List<LocoOrderModel>> GetLocoOrders(int id);
        Task<bool> DeleteOrder(int id);
    }
}