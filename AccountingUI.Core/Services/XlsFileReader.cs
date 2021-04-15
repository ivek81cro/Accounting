using ExcelDataReader;
using System.Data;
using System.IO;
using System.Text;

namespace AccountingUI.Core.Services
{
    public class XlsFileReader : IXlsFileReader
    {
        public XlsFileReader()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public DataSet Convert(string put, string identifier)
        {
            if (put == "")
                return null;

            FileStream stream = File.Open(put, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader;
            try
            {
                //old xls format 93-07
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            catch
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            DataSet result = excelReader.AsDataSet();
            excelReader.Close();

            if (!result.Tables[0].Rows[0][0].ToString().Contains(identifier))
                return null;

            return result;
        }
    }
}
