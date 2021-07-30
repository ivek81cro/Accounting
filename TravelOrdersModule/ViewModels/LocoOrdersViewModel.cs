using AccountingUI.Core.Models;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace TravelOrdersModule.ViewModels
{
    public class LocoOrdersViewModel : ViewModelBase
    {
        private readonly IDialogService _showDialog;

        public LocoOrdersViewModel(IDialogService showDialog)
        {
            _showDialog = showDialog;

            GenerateList = new DelegateCommand(GenerateOrders);
        }

        public DelegateCommand GenerateList { get; private set; }

        private void GenerateOrders()
        {
            _showDialog.ShowDialog("GeneratorDialog", null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }
    }
}
