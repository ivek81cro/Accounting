using AccountingUI.Core.Models;
using BookUraModule.ModelsLocal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookUraModule.ServiceLocal
{
    public interface IDataForXml
    {
        Task<sObrazacURA> GenerateXml(List<BookUraRestModel> uraList, string autor, DateTime[] period);
    }
}