using System.Collections.Generic;

namespace Accounting.DataManager.Models
{
    public class TravelOrdersLocoModel
    {
        public List<LocoOrderModel> LocoOrders { get; set; }
        public LocoCalculationModel LocoCalculation { get; set; }
    }
}
