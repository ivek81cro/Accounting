using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AssetsModule.ViewModels
{
    public class AssetsFixedViewModel : ViewModelBase
    {
        private readonly IAssetsEndpoint _assetsEndpoint;

        public AssetsFixedViewModel(IAssetsEndpoint assetsEndpoint)
        {
            _assetsEndpoint = assetsEndpoint;
        }

        private ObservableCollection<AssetModel> _assetsList;
        public ObservableCollection<AssetModel> AssetsList
        {
            get { return _assetsList; }
            set { SetProperty(ref _assetsList, value); }
        }
        public async void LoadAssetsList()
        {
            var list = await _assetsEndpoint.GetAssets();
            AssetsList = new ObservableCollection<AssetModel>(list);
        }
    }
}
