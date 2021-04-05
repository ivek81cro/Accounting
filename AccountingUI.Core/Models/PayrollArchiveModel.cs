using System.Collections.Generic;

namespace AccountingUI.Core.Models
{
    public class PayrollArchiveModel
    {
        public PayrollAccountingModel Calculation { get; set; }
        public List<PayrollCalculationModel> Payrolls { get; set; } = new();
        public List<PayrollSupplementCalculationModel> Supplements { get; set; } = new();
    }
}
