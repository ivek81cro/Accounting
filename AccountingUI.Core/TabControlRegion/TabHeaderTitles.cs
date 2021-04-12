using System;

namespace AccountingUI.Core.TabControlRegion
{
    public class TabHeaderTitles
    {
        private enum ViewNames
        {
            PartnersView,
            CompanyView,
            EmployeesView,
            CitiesView,
            PayrollView,
            PayrollProcessing,
            ArchiveView,
            JoppdView
        }
        public static string GetHeaderTitle(string viewName)
        {
            switch (Enum.Parse(typeof(ViewNames), viewName))
            {
                case ViewNames.PartnersView:
                    return "Partneri";
                case ViewNames.CompanyView:
                    return "Komitent";
                case ViewNames.EmployeesView:
                    return "Zaposlenici";
                case ViewNames.CitiesView:
                    return "Pregled gradova i općina";
                case ViewNames.PayrollView:
                    return "Plaća";
                case ViewNames.PayrollProcessing:
                    return "Obračun plaće";
                case ViewNames.JoppdView:
                    return "Izrada JOPPD obrasca";
                case ViewNames.ArchiveView:
                    return "Arhiva obračuna";
                default:
                    return "";
            }
        }
    }
}
