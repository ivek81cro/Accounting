using AccountingUI.Core.Models;
using PayrollModule.ServiceLocal.EporeznaModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayrollModule.ServiceLocal
{
    public interface IJoppdGenerate
    {
        Task<sObrazacJOPPD> CreateJoppdEporezna(DateTime? date, string identifier, string formCreator, PayrollArchiveModel archive, List<JoppdEmployeeModel> joppdEmployee);
    }
}