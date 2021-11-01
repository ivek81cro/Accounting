using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;

namespace AssetsModule.Dialogs
{
    public class AssetCalculationDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IDialogService _showDialog;
        private readonly IAssetsEndpoint _assetsEndpoint;

        private string _bookName = "Amortizacija";

        public AssetCalculationDialogViewModel(IAssetsEndpoint assetsEndpoint,
                                               IDialogService showDialog)
        {
            _assetsEndpoint = assetsEndpoint;
            _showDialog = showDialog;

            CalculateAssetCommand = new DelegateCommand(CalculateAssetValue, CanCalculate);
            OpenAccountsSelectionCommand = new DelegateCommand(OpenAccountsSelection);
            ProcessAssetCommand = new DelegateCommand(ProcessAsset, CanProcessAsset);
        }

        public string Title => "Obračun amortizacije";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand CalculateAssetCommand { get; private set; }
        public DelegateCommand OpenAccountsSelectionCommand { get; private set; }
        public DelegateCommand ProcessAssetCommand { get; private set; }

        private AssetModel _asset;
        public AssetModel Asset
        {
            get { return _asset; }
            set { SetProperty(ref _asset, value); }
        }

        private DateTime? _datumOd;
        public DateTime? DatumOd
        {
            get { return _datumOd; }
            set
            {
                SetProperty(ref _datumOd, value);
                CalculateAssetCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime? _datumDo;
        public DateTime? DatumDo
        {
            get { return _datumDo; }
            set
            {
                SetProperty(ref _datumDo, value);
                CalculateAssetCommand.RaiseCanExecuteChanged();
            }
        }

        private decimal _iznosAmortizacije;
        public decimal IznosAmortizacije
        {
            get { return _iznosAmortizacije; }
            set { SetProperty(ref _iznosAmortizacije, value); }
        }

        private string _kontoTroska;
        public string KontoTroska
        {
            get { return _kontoTroska; }
            set
            {
                SetProperty(ref _kontoTroska, value);
                ProcessAssetCommand.RaiseCanExecuteChanged();
            }
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

        private bool CanCalculate()
        {
            return DatumOd != null && DatumOd != null;
        }

        private void CalculateAssetValue()
        {
            IznosAmortizacije = Asset.NabavnaVrijednost * (Asset.StopaOtpisa / 100.0m);

            int? difference = ((DatumDo?.Year - DatumOd?.Year) * 12) + DatumDo?.Month - DatumOd?.Month + 1;

            if (difference < 12)
            {
                IznosAmortizacije = IznosAmortizacije / 12.0m * (decimal)difference;
            }

            if (IznosAmortizacije > Asset.SadasnjaVrijednost)
            {
                IznosAmortizacije = Asset.SadasnjaVrijednost;
            }
        }

        private void OpenAccountsSelection()
        {
            _showDialog.ShowDialog("AccountsSelectionDialog", null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    KontoTroska = result.Parameters.GetValue<BookAccountModel>("account").Konto;
                }
            });
        }

        private bool CanProcessAsset()
        {
            return KontoTroska != null;
        }

        private void ProcessAsset()
        {
            var entries = new List<AccountingJournalModel>
            {
                CreateJournalEntries(KontoTroska, "Dugovna"),
                CreateJournalEntries(Asset.KontoOtpisa, "Potražna")
            };
            SendToProcessingDialog(entries);
        }

        private void SendToProcessingDialog(List<AccountingJournalModel> entries)
        {
            var parameters = new DialogParameters();
            parameters.Add("entries", entries);
            _showDialog.ShowDialog("ProcessToJournal", parameters, async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    Asset.SadasnjaVrijednost -= IznosAmortizacije;
                    Asset.IznosOtpisa += IznosAmortizacije;
                    await _assetsEndpoint.PostAsset(Asset);//Update, branches in controller
                }
            });
        }

        private AccountingJournalModel CreateJournalEntries(string konto, string side)
        {
            return new AccountingJournalModel
            {
                Broj = Asset.InvBroj,
                Dokument = Asset.Naziv + ": " + Asset.Dokument,
                Datum = DatumDo,
                Opis = "Amortizacija " + DatumOd?.ToString("dd.MM.yyyy") + "-" + DatumDo?.ToString("dd.MM.yyyy"),
                Konto = konto,
                Dugovna = side == "Dugovna" ? IznosAmortizacije : 0,
                Potrazna = side == "Potražna" ? IznosAmortizacije : 0,
                Valuta = "HRK",
                VrstaTemeljnice = _bookName
            };
        }
    }
}
