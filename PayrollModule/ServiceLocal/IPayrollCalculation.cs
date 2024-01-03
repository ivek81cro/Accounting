using AccountingUI.Core.Models;

namespace PayrollModule.ServiceLocal
{
    public interface IPayrollCalculation
    {
        void Calculate(PayrollModel p, CityModel grad, decimal olaksica);
    }
}