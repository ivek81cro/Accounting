using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using System.Collections.ObjectModel;

namespace PayrollModule.ServiceLocal
{
    public class PayrollArchivePrepare : IPayrollArchivePrepare
    {
        private readonly IPayrollArchiveEndpoint _archiveEndpoint;

        public PayrollArchivePrepare(IPayrollArchiveEndpoint archiveEndpoint)
        {
            _archiveEndpoint = archiveEndpoint;
        }

        private PayrollArchiveModel _archive = new();
        private ObservableCollection<PayrollCalculationModel> _payrolls;
        private ObservableCollection<PayrollSupplementCalculationModel> _supplements;

        public PayrollArchiveModel Process(ObservableCollection<PayrollCalculationModel> payrollCalculations,
            ObservableCollection<PayrollSupplementCalculationModel> supplementCalculations,
            PayrollAccountingModel payrollAccounting)
        {
            _payrolls = payrollCalculations;
            _supplements = supplementCalculations;

            if (_payrolls != null)
            {
                AddPayrolls();
            }
            else
            {
                AddSupplementsOnly();
            }

            _archive.Calculation = payrollAccounting;

            return _archive;
        }

        private void AddPayrolls()
        {
            foreach (var pay in _payrolls)
            {
                if (pay.IsChecked)
                {
                    _archive.Payrolls.Add(pay);
                    if (_supplements != null)
                    {
                        AddSupplement(pay.Oib);
                    }
                    else
                    {
                        _archive.Supplements = null;
                    }
                }
            }
        }

        private void AddSupplement(string oib)
        {
            foreach (var sup in _supplements)
            {
                if (sup.Oib == oib && sup.IsChecked)
                {
                    _archive.Supplements.Add(sup);
                }
            }
        }

        private void AddSupplementsOnly()
        {
            foreach (var sup in _supplements)
            {
                if (sup.IsChecked)
                {
                    _archive.Supplements.Add(sup);
                }
            }
        }

        public void SaveToDatabase(PayrollArchiveModel archive)
        {
            _archive = archive;

            _archiveEndpoint.PostToArchive(_archive);
        }
    }
}
