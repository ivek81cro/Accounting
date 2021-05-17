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
            JoppdView,
            PrimkeView,
            AccountsView,
            PrimkeRepro,
            RestView,
            IraView,
            CashRegisterBookView,
            PrimkeDiscounts,
            BankStatementView, 
            VatCalculation,
            AssetsFixedView,
            AssetsCurrentView,
            JournalView,
            RetailView
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
                case ViewNames.AccountsView:
                    return "Kontni plan";
                case ViewNames.PrimkeView:
                    return "URA-Kalkulacije primki";
                case ViewNames.PrimkeRepro:
                    return "URA-Repromaterijal";
                case ViewNames.PrimkeDiscounts:
                    return "URA-Odobrenja";
                case ViewNames.RestView:
                    return "URA-sve";
                case ViewNames.IraView:
                    return "IRA-sve";
                case ViewNames.CashRegisterBookView:
                    return "IRA-knjiga blagajne";
                case ViewNames.BankStatementView:
                    return "Izvodi";
                case ViewNames.VatCalculation:
                    return "PDV-izračun";
                case ViewNames.AssetsFixedView:
                    return "Dugotrajna imovina";
                case ViewNames.AssetsCurrentView:
                    return "Sitni inventar";
                case ViewNames.JournalView:
                    return "Temeljnice";
                case ViewNames.RetailView:
                    return "Maloprodaja";
                default:
                    return "";
            }
        }
    }
}
