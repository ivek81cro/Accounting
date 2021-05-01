using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace AssetsModule.Dialogs
{
    public class AssetDetailDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IDialogService _showDialog;
        private readonly IAssetsEndpoint _assetsEndpoint;

        public AssetDetailDialogViewModel(IDialogService showDialog,
                                          IAssetsEndpoint assetsEndpoint)
        {
            _showDialog = showDialog;
            _assetsEndpoint = assetsEndpoint;

            OpenAccountsSelectionCommand = new DelegateCommand<string>(OpenAccountsSelection);
            SaveAssetCommand = new DelegateCommand(SaveAssetToDatabase);
        }

        public string Title => "Osnovna sredstva";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand<string> OpenAccountsSelectionCommand { get; private set; }
        public DelegateCommand SaveAssetCommand { get; private set; }

        private AssetModel _asset;
        public AssetModel Asset
        {
            get { return _asset; }
            set { SetProperty(ref _asset, value); }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Asset = parameters.GetValue<AssetModel>("asset");
        }

        private void OpenAccountsSelection(string tag)
        {
            _showDialog.ShowDialog("AccountsSelectionDialog", null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    var retResult = result.Parameters.GetValue<BookAccountModel>("account").Konto;
                    if (tag == "otpis")
                    {
                        Asset.KontoOtpisa = retResult;
                    }
                    else
                    {
                        Asset.SintetickiKonto = retResult;
                    }
                }
            });
        }

        private bool CanSave()
        {
            return Asset != null && !Asset.HasErrors;
        }

        private async void SaveAssetToDatabase()
        {
            if (CanSave())
            {
                if (Asset.Id == 0)
                {
                    Asset.SadasnjaVrijednost = Asset.NabavnaVrijednost;
                }
                await _assetsEndpoint.Insert(Asset);
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
        }
    }
}
