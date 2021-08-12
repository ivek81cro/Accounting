using AccountingUI.Core.Models;

namespace TravelOrdersModule.LocalModel
{
    public class LocoCalculationListModel : LocoCalculationModel
    {
        private string _employeeName;
        public string EmployeeName
        {
            get { return _employeeName; }
            set { SetProperty(ref _employeeName, value); }
        }

        private string _employeeOib;
        public string EmployeeOib
        {
            get { return _employeeOib; }
            set { SetProperty(ref _employeeOib, value); }
        }
    }
}
