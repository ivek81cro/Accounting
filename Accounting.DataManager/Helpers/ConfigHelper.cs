using Accounting.DataManager.Models;
using System;
using System.Configuration;

namespace Accounting.DataManager.Helpers
{
    public class ConfigHelper
    {
        public static void GetTaxRates()
        {
            //TODO: load taxrates from appsettings
            var rates =  ConfigurationManager.GetSection("TaxRates");

            TaxRates taxRates = new TaxRates();
            //foreach (var rate in rates)
            //{
            //    if (Decimal.TryParse(rate, out decimal output))
            //    {

            //    }
            //    else
            //    {
            //        throw new ConfigurationErrorsException("Tax rate not setup properly");
            //    }
            //}
        }
    }
}
