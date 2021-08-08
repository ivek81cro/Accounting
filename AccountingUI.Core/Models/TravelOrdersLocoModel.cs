using System.Collections.Generic;

namespace AccountingUI.Core.Models
{
    public class TravelOrdersLocoModel
    {
        public List<LocoOrderModel> LocoOrders { get; set; }
        public LocoCalculationModel LocoCalculation { get; set; }
    }
}
