using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PayrollModule.ServiceLocal
{
    public class PayrollArchivePrepare : IPayrollArchivePrepare
    {
        private readonly IPayrollArchiveEndpoint _archiveEndpoint;

        public PayrollArchivePrepare(IPayrollArchiveEndpoint archiveEndpoint)
        {
            _archiveEndpoint = archiveEndpoint;
        }

        private PayrollArchiveModel _archive;
        private ObservableCollection<PayrollArchivePayrollModel> _payrolls;
        private ObservableCollection<PayrollArchiveSupplementModel> _supplements;

        public PayrollArchiveModel Process(ObservableCollection<PayrollArchivePayrollModel> payrollCalculations,
            ObservableCollection<PayrollArchiveSupplementModel> supplementCalculations,
            PayrollArchiveHeaderModel payrollAccounting)
        {
            _archive = new();
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

            payrollAccounting.CreateUniqueIdentifier();
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

        public async Task<bool> SaveToDatabase(PayrollArchiveModel archive)
        {
            _archive = archive;
            bool result = await _archiveEndpoint.IfIdentifierExists(_archive.Calculation.UniqueId);
            if (!result)
            {
                await _archiveEndpoint.PostToArchive(_archive);
                return true;
            }
            return false;
        }
    }
}
