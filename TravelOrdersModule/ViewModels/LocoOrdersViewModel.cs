using AccountingUI.Core.Models;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;

namespace TravelOrdersModule.ViewModels
{
    public class LocoOrdersViewModel : ViewModelBase
    {
        public LocoOrdersViewModel()
        {
            GenerateList = new DelegateCommand(GenerateOrders);
        }

        public DelegateCommand GenerateList { get; private set; }

        private ObservableCollection<LocoOrderModel> _locoOrdersList;
        public ObservableCollection<LocoOrderModel> LocoOrdersList
        {
            get { return _locoOrdersList; }
            set { SetProperty(ref _locoOrdersList, value); }
        }

        private void GenerateOrders()
        {
            DateTime futureDate = new DateTime(2021, 12, 01);
            DateTime date = new DateTime(2021, 01, 01);
            int pocetno = 0;
            int zavrsno = 0;
            LocoOrdersList = new();
            for (DateTime startDate = date; startDate < futureDate; startDate = startDate.AddDays(1.0))
            {
                int random = new Random().Next(40, 90);
                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    pocetno = zavrsno;
                    zavrsno += random;
                    _locoOrdersList.Add(
                        new LocoOrderModel
                        {
                            Datum = startDate,
                            MarkaVozila = "Osobni 1.9",
                            Registracija = "ZG123AB",
                            Opis = "Dostava",
                            Relacija = "ZG-ZG-ZG",
                            PocetnoStanje = pocetno,
                            ZavrsnoStanje = zavrsno,
                            PrijedeniKilometri = random
                        });
                    zavrsno += new Random().Next(10, 20);
                }
            }
        }
    }
}
