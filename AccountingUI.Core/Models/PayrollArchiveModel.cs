using System.Collections.Generic;

namespace AccountingUI.Core.Models
{
    public class PayrollArchiveModel
    {
        public PayrollArchiveHeaderModel Calculation { get; set; }
        public List<PayrollArchivePayrollModel> Payrolls { get; set; } = new();
        public List<PayrollArchiveSupplementModel> Supplements { get; set; } = new();
    }
}
