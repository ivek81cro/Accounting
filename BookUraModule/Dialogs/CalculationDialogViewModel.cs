using AccountingUI.Core.Models;
using BookUraModule.ModelsLocal;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookUraModule.Dialogs
{
    public class CalculationDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "Izračun";

        public event Action<IDialogResult> RequestClose;

        private CalculationModel _calculation;
        public CalculationModel Calculation
        {
            get { return _calculation; }
            set { SetProperty(ref _calculation, value); }
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
            var list = parameters.GetValue<List<BookUraRestModel>>("collection");
            Calculation = new CalculationModel
            {
                ZaUplatu = list.Sum(x => x.ZaUplatu),
                PoreznaOsnovica0 = list.Sum(x => x.PoreznaOsnovica0),
                PoreznaOsnovica5 = list.Sum(x => x.PoreznaOsnovica5),
                PoreznaOsnovica10 = list.Sum(x => x.PoreznaOsnovica10),
                PoreznaOsnovica13 = list.Sum(x => x.PoreznaOsnovica13),
                PoreznaOsnovica23 = list.Sum(x => x.PoreznaOsnovica23),
                PoreznaOsnovica25 = list.Sum(x => x.PoreznaOsnovica25),
                PretporezT5 = list.Sum(x => x.PretporezT5),
                PretporezT10 = list.Sum(x => x.PretporezT10),
                PretporezT13 = list.Sum(x => x.PretporezT13),
                PretporezT23 = list.Sum(x => x.PretporezT23),
                PretporezT25 = list.Sum(x => x.PretporezT25),
                UkupniPretporez = list.Sum(x => x.UkupniPretporez),
                IznosSPorezom = list.Sum(x => x.IznosSPorezom)
            };
            Calculation.OsnovicaUkupno = Calculation.PoreznaOsnovica0 
                + Calculation.PoreznaOsnovica5 
                + Calculation.PoreznaOsnovica13 
                + Calculation.PoreznaOsnovica25;
        }
    }
}
