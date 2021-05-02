using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IAssetsData
    {
        List<AssetModel> Get(string assetType);
        void Insert(AssetModel asset);
        void Update(AssetModel asset);
    }
}