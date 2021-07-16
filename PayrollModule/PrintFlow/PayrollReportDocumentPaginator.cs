using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PayrollModule.PrintFlow
{
    class PayrollReportDocumentPaginator : DocumentPaginator
    {
        private int _pageCount;

        public PayrollReportDocumentPaginator(/*employee, company, payroll, supplements*/)
        {

        }

        public override bool IsPageCountValid { get { return true; } }

        public override int PageCount { get { return _pageCount; } }

        public override Size PageSize { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override IDocumentPaginatorSource Source { get { return null; } }

        public override DocumentPage GetPage(int pageNumber)
        {
            DocumentPage page = null;

            Grid table = new();

            CreateTable(table);

            return page;
        }

        private void CreateTable(Grid table)
        {

        }
    }
}
