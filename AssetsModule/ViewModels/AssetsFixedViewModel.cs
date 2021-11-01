using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace AssetsModule.ViewModels
{
    public class AssetsFixedViewModel : ViewModelBase
    {
        private readonly IAssetsEndpoint _assetsEndpoint;
        private readonly IDialogService _showDialog;

        private readonly string _vrstaImovine;

        public AssetsFixedViewModel(IAssetsEndpoint assetsEndpoint, IDialogService showDialog)
        {
            _assetsEndpoint = assetsEndpoint;
            _showDialog = showDialog;
            _vrstaImovine = "Dugotrajna";

            OpenDetailDialogCommand = new DelegateCommand<string>(ShowDetailDailog);
            ProcessItemCommand = new DelegateCommand(ProcessItem, CanProcess);
        }

        public DelegateCommand<string> OpenDetailDialogCommand { get; set; }
        public DelegateCommand ProcessItemCommand { get; set; }

        private ObservableCollection<AssetModel> _assetsList;
        public ObservableCollection<AssetModel> AssetsList
        {
            get { return _assetsList; }
            set { SetProperty(ref _assetsList, value); }
        }

        private AssetModel _selectedAsset;
        public AssetModel SelectedAsset
        {
            get { return _selectedAsset; }
            set
            {
                SetProperty(ref _selectedAsset, value);
                ProcessItemCommand.RaiseCanExecuteChanged();
            }
        }

        public async void LoadAssetsList()
        {
            var list = await _assetsEndpoint.GetAssets(_vrstaImovine);
            AssetsList = new ObservableCollection<AssetModel>(list);
        }

        private void ShowDetailDailog(string tag)
        {
            if(tag == "new")
            {
                SelectedAsset = new() { VrstaPoTrajanju = _vrstaImovine};
            }

            var param = new DialogParameters();
            param.Add("asset", SelectedAsset);
            _showDialog.ShowDialog("AssetDetailDialog", param, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    LoadAssetsList();
                }
            });
        }

        private bool CanProcess()
        {
            return SelectedAsset != null;
        }

        private void ProcessItem()
        {
            var param = new DialogParameters();
            param.Add("asset", SelectedAsset);
            _showDialog.ShowDialog("AssetCalculationDialog", param, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    LoadAssetsList();
                }
            });
        }
    }
}
