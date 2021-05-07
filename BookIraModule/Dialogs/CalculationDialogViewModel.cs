using AccountingUI.Core.Models;
using BookIraModule.ModelsLocal;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookIraModule.Dialogs
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
            var list = parameters.GetValue<List<BookIraModel>>("collection");
            Calculation = new CalculationModel
            {
                IznosSPdv = list.Sum(x => x.IznosSPdv),
                OslobodjenoPdvEU = list.Sum(x => x.OslobodjenoPdvEU),
                OslobodjenoPdvOstalo = list.Sum(x => x.OslobodjenoPdvOstalo),
                ProlaznaStavka = list.Sum(x => x.ProlaznaStavka),
                PoreznaOsnovica0 = list.Sum(x => x.PoreznaOsnovica0),
                PoreznaOsnovica5 = list.Sum(x => x.PoreznaOsnovica5),
                PoreznaOsnovica10 = list.Sum(x => x.PoreznaOsnovica10),
                PoreznaOsnovica13 = list.Sum(x => x.PoreznaOsnovica13),
                PoreznaOsnovica23 = list.Sum(x => x.PoreznaOsnovica23),
                PoreznaOsnovica25 = list.Sum(x => x.PoreznaOsnovica25),
                Pdv5 = list.Sum(x => x.Pdv5),
                Pdv10 = list.Sum(x => x.Pdv10),
                Pdv13 = list.Sum(x => x.Pdv13),
                Pdv23 = list.Sum(x => x.Pdv23),
                Pdv25 = list.Sum(x => x.Pdv25),
                UkupniPdv = list.Sum(x => x.UkupniPdv)
            };
            Calculation.OsnovicaUkupno = Calculation.PoreznaOsnovica5
                + Calculation.PoreznaOsnovica13
                + Calculation.PoreznaOsnovica25;
        }
    }
}
