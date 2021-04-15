using System.Data;

namespace AccountingUI.Core.Services
{
    public interface IXlsFileReader
    {
        DataSet Convert(string put, string identifier);
    }
}