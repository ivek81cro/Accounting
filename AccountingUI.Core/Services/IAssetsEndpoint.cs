using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IAssetsEndpoint
    {
        Task<List<AssetModel>> GetAssets();
        Task<bool> Insert(AssetModel asset);
    }
}