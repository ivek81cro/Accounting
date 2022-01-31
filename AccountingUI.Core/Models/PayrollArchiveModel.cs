using System.Collections.Generic;

namespace AccountingUI.Core.Models
{
    public class PayrollArchiveModel
    {
        public PayrollArchiveHeaderModel Header { get; set; }
        public List<PayrollArchivePayrollModel> Payrolls { get; set; } = new();
        public List<PayrollArchiveSupplementModel> Supplements { get; set; } = new();
        public List<PayrollHours> WorkedHours { get; set; } = new();
    }
}
