using System.Collections.Generic;

namespace Accounting.DataManager.Models
{
    public class PayrollArchiveModel
    {
        public PayrollArchiveHeaderModel Header { get; set; } = new();
        public List<PayrollArchivePayrollModel> Payrolls { get; set; } = new();
        public List<PayrollArchiveSupplementModel> Supplements { get; set; } = new();
        public List<PayrollHours> WorkedHours { get; set; } = new();
    }
}
