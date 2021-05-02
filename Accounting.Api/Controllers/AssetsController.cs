using Accounting.DataManager.DataAccess;
using Accounting.DataManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetsData _data;

        public AssetsController(IAssetsData data)
        {
            _data = data;
        }

        [HttpGet("{assetType}")]
        public List<AssetModel> Get(string assetType)
        {
            return _data.Get(assetType);
        }

        [HttpPost]
        public void Post(AssetModel asset)
        {
            if (asset.Id == 0)
            {
                _data.Insert(asset);
            }
            else
            {
                _data.Update(asset);
            }
        }
    }
}
