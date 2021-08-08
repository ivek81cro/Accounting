using AccountingUI.Core.Models;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface ITravelOrdersEndpoint
    {
        Task<bool> PostLocoTravel(TravelOrdersLocoModel locoTravelOrder);
    }
}