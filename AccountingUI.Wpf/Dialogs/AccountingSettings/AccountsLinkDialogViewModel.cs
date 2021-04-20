using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AccountingUI.Wpf.Dialogs.AccountingSettings
{
    public class AccountsLinkDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IDialogService _showDialog;
        private readonly IBookAccountSettingsEndpoint _settingsEndpoint;

        public AccountsLinkDialogViewModel(IDialogService showDialog, IBookAccountSettingsEndpoint settingsEndpoint)
        {
            _showDialog = showDialog;
            _settingsEndpoint = settingsEndpoint;

            OpenAccountsSelectionCommand = new DelegateCommand(OpenAccountsSelection);
            AddNewSettingCommand = new DelegateCommand(InsertSetting);
            DeleteSettingCommand = new DelegateCommand(DeleteSetting, CanDelete);
        }

        public DelegateCommand OpenAccountsSelectionCommand { get; private set; }
        public DelegateCommand AddNewSettingCommand { get; private set; }
        public DelegateCommand DeleteSettingCommand { get; private set; }

        public string Title => "Postavke kniženja";
        public string[] OptionSide { get; set; } = { "Dugovna", "Potražna" };

        private List<string> _settingsOptions;
        public List<string> SettingsOptions
        {
            get { return _settingsOptions; }
            set { SetProperty(ref _settingsOptions, value); }
        }

        private ObservableCollection<BookAccountsSettingsModel> fieldName;
        public ObservableCollection<BookAccountsSettingsModel> PropertyName
        {
            get { return fieldName; }
            set { SetProperty(ref fieldName, value); }
        }

        private BookAccountsSettingsModel _selectedSetting;
        public BookAccountsSettingsModel SelectedSetting
        {
            get { return _selectedSetting; }
            set 
            { 
                SetProperty(ref _selectedSetting, value);
                DeleteSettingCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<BookAccountsSettingsModel> _settings;
        public ObservableCollection<BookAccountsSettingsModel> Settings
        {
            get { return _settings; }
            set { SetProperty(ref _settings, value); }
        }

        private BookAccountsSettingsModel _newSetting = new();
        public BookAccountsSettingsModel NewSetting
        {
            get { return _newSetting; }
            set { SetProperty(ref _newSetting, value); }
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            SettingsOptions = parameters.GetValue<List<string>>("columnsList");
            NewSetting.BookName = parameters.GetValue<string>("bookName");
            LoadSettings();
        }

        private void OpenAccountsSelection()
        {
            _showDialog.ShowDialog("AccountsSelectionDialog", null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    NewSetting.Account = result.Parameters.GetValue<BookAccountModel>("account").Konto;
                }
            });
        }

        private async void LoadSettings()
        {
            var list = await _settingsEndpoint.GetByBookName(NewSetting.BookName);
            Settings = new ObservableCollection<BookAccountsSettingsModel>(list);
        }

        private bool CanInsert()
        {
            return NewSetting.Account != null && NewSetting.Name != null && NewSetting.Side != null;
        }

        private async void InsertSetting()
        {
            if (CanInsert())
            {
                await _settingsEndpoint.PostSetting(NewSetting);
                LoadSettings();
            }
        }

        private bool CanDelete()
        {
            return SelectedSetting != null;
        }

        private async void DeleteSetting()
        {
            await _settingsEndpoint.Delete(SelectedSetting.Id);
            LoadSettings();
        }
    }
}
