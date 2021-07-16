using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace PayrollModule.PrintFlow
{
    public class PayrollReportViewModel : BindableBase, IDialogAware
    {
        private readonly ICompanyEndpoint _companyEndpoint;
        private readonly IPayrollArchiveEndpoint _archiveEndpoint;
        private readonly IEmployeeEndpoint _employeeEndpoint;

        private CompanyModel _company;
        private List<PayrollArchivePayrollModel> _payroll;
        private List<PayrollArchiveSupplementModel> _supplement;
        private EmployeeModel _employee;
        private DateTime[] _period;

        public PayrollReportViewModel(ICompanyEndpoint companyEndpoint,
                                      IPayrollArchiveEndpoint archiveEndpoint,
                                      IEmployeeEndpoint employeeEndpoint)
        {
            _companyEndpoint = companyEndpoint;

            PrintCommand = new DelegateCommand(PrintGridView);
            //PrintCommand2 = new DelegateCommand<Visual>(PrintGridView2);

            ReadStyles();
            _archiveEndpoint = archiveEndpoint;
            _employeeEndpoint = employeeEndpoint;
        }

        public DelegateCommand PrintCommand { get; private set; }
        public DelegateCommand<Visual> PrintCommand2 { get; private set; }

        public string Title => "Ispis obračuna plaće";

        public event Action<IDialogResult> RequestClose;

        public Style BorderTitleStyle { get; private set; }
        public Style BorderHeaderStyle { get; private set; }
        public Style TextBoxCustom { get; private set; }

        private void ReadStyles()
        {
            var resources = new ResourceDictionary { Source = new Uri("Styles/PayrollReport.xaml", UriKind.Relative) };
            if (resources != null)
            {
                this.BorderTitleStyle = (Style)resources["BorderTitleStyle"];
                this.BorderHeaderStyle = (Style)resources["BorderHeaderStyle"];
                this.TextBoxCustom = (Style)resources["TextBoxCustom"];
            }
        }

        private FixedDocumentSequence _printDocument;
        public FixedDocumentSequence PrintDocument
        {
            get { return _printDocument; }
            set { SetProperty(ref _printDocument, value); }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            int idArchive = parameters.GetValue<int>("archiveId");
            _period = parameters.GetValue<DateTime[]>("period");
            LoadDataFromDatabase(idArchive);
        }

        private async void LoadDataFromDatabase(int idArchive)
        {
            _payroll = await _archiveEndpoint.GetArchivePayrolls(idArchive);
            _supplement = await _archiveEndpoint.GetArchiveSupplements(idArchive);
        }

        private async void PrintGridView()
        {
            #region Main grid
            Grid grid = new();

            AddDocumentTitle(grid);

            await AddDocumentHeader(grid);

            AddDocumentTimePeriod(grid);
            #endregion

            PrintDialog pd = new PrintDialog();
            pd.PrintTicket.PageOrientation = PageOrientation.Portrait;
            pd.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);

            FixedDocument doc = new FixedDocument();
            doc.DocumentPaginator.PageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
            FixedPage fxpg = new FixedPage();
            fxpg.Width = doc.DocumentPaginator.PageSize.Width;
            fxpg.Height = doc.DocumentPaginator.PageSize.Height;
            fxpg.Children.Add(grid);

            PageContent pc = new PageContent();
            ((IAddChild)pc).AddChild(fxpg);

            doc.Pages.Add(pc);
            CreateDocument(doc.DocumentPaginator);

        }

        private void CreateDocument(DocumentPaginator paginator)
        {
            string tempFileName = Path.GetTempFileName();
            File.Delete(tempFileName);
            using (XpsDocument xpsDocument = new XpsDocument(tempFileName, FileAccess.ReadWrite))
            {
                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
                writer.Write(paginator);
                PrintDocument = xpsDocument.GetFixedDocumentSequence();
            }
        }

        private void AddDocumentTitle(Grid grid)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Border titleBorder = new();
            titleBorder.Style = BorderTitleStyle;
            titleBorder.SetCurrentValue(Grid.RowProperty, 0);

            Grid tGrid = new();
            tGrid.ColumnDefinitions.Add(new ColumnDefinition());
            tGrid.ColumnDefinitions.Add(new ColumnDefinition());

            TextBlock tLeft = new TextBlock
            {
                Text = "OBRAČUN ISPLAĆENE PLAĆENE NAKNADE PLAĆE",
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Width = 350
            };
            tLeft.SetCurrentValue(Grid.ColumnProperty, 0);

            TextBlock tRight = new TextBlock
            {
                Text = "OBRAZAC IP1",
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                TextAlignment = TextAlignment.Right,
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 350
            };
            tRight.SetCurrentValue(Grid.ColumnProperty, 1);

            tGrid.Children.Add(tLeft);
            tGrid.Children.Add(tRight);

            titleBorder.Child = tGrid;

            grid.Children.Add(titleBorder);
        }

        private async Task AddDocumentHeader(Grid grid)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Border hBorder = new Border { Padding = new Thickness(5, 0, 0, 0 )};
            hBorder.Style = BorderHeaderStyle;
            hBorder.SetCurrentValue(Grid.RowProperty, 1);

            Grid hGrid = new();
            hGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            hGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            await AddEmployeeHeaderData(hGrid);

            await AddCompanyHeaderData(hGrid);

            hBorder.Child = hGrid;

            grid.Children.Add(hBorder);
        }

        private async Task AddEmployeeHeaderData(Grid hGrid)
        {
            hGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            _employee = await _employeeEndpoint.GetByOib(_payroll[0].Oib);

            Grid eGrid = new();
            eGrid.SetCurrentValue(Grid.RowProperty, 0);
            eGrid.SetCurrentValue(Grid.ColumnProperty, 0);
            eGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            eGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            eGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock title = new TextBlock { Text = "I. PODACI O RADNIKU" };
            title.SetCurrentValue(Grid.RowProperty, 0);
            title.SetCurrentValue(Grid.ColumnProperty, 0);
            title.SetCurrentValue(Grid.ColumnSpanProperty, 2);
            eGrid.Children.Add(title);

            eGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock name = new TextBlock { Text = "1. Ime i prezime" };
            name.SetCurrentValue(Grid.RowProperty, 1);
            name.SetCurrentValue(Grid.ColumnProperty, 0);
            eGrid.Children.Add(name);

            TextBlock nameD = new TextBlock { Text = _employee.Ime + " " + _employee.Prezime };
            nameD.SetCurrentValue(Grid.RowProperty, 1);
            nameD.SetCurrentValue(Grid.ColumnProperty, 1);
            eGrid.Children.Add(nameD);

            eGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock address = new TextBlock { Text = "2. Adresa" };
            address.SetCurrentValue(Grid.RowProperty, 2);
            address.SetCurrentValue(Grid.ColumnProperty, 0);
            eGrid.Children.Add(address);

            TextBlock addressD = new TextBlock
            {
                Text = _employee.Ulica + " " + _employee.Broj + ", " + _employee.Mjesto,
                TextWrapping = TextWrapping.Wrap
            };
            addressD.SetCurrentValue(Grid.RowProperty, 2);
            addressD.SetCurrentValue(Grid.ColumnProperty, 1);
            eGrid.Children.Add(addressD);

            eGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock oib = new TextBlock { Text = "3. OIB" };
            oib.SetCurrentValue(Grid.RowProperty, 3);
            oib.SetCurrentValue(Grid.ColumnProperty, 0);
            eGrid.Children.Add(oib);

            TextBlock oibD = new TextBlock { Text = _employee.Oib };
            oibD.SetCurrentValue(Grid.RowProperty, 3);
            oibD.SetCurrentValue(Grid.ColumnProperty, 1);
            eGrid.Children.Add(oibD);

            eGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock iban = new TextBlock { Text = "4. IBAN" };
            iban.SetCurrentValue(Grid.RowProperty, 4);
            iban.SetCurrentValue(Grid.ColumnProperty, 0);
            eGrid.Children.Add(iban);

            TextBlock ibanD = new TextBlock { Text = _employee.Iban };
            ibanD.SetCurrentValue(Grid.RowProperty, 4);
            ibanD.SetCurrentValue(Grid.ColumnProperty, 1);
            eGrid.Children.Add(ibanD);

            eGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock iban2 = new TextBlock { Text = "5. IBAN čl.212 Ovršnog zakona" };
            iban2.SetCurrentValue(Grid.RowProperty, 5);
            iban2.SetCurrentValue(Grid.ColumnProperty, 0);
            eGrid.Children.Add(iban2);

            TextBlock iban2D = new TextBlock { Text = "" };
            iban2D.SetCurrentValue(Grid.RowProperty, 5);
            iban2D.SetCurrentValue(Grid.ColumnProperty, 1);
            eGrid.Children.Add(iban2D);

            hGrid.Children.Add(eGrid);
        }

        private async Task AddCompanyHeaderData(Grid hGrid)
        {
            hGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            _company = await _companyEndpoint.Get();

            Border cBorder = new Border
            {
                BorderThickness = new Thickness(1, 0, 0, 0),
                Padding = new Thickness(5, 0, 0, 0),
                BorderBrush = Brushes.Black
            };
            cBorder.SetCurrentValue(Grid.RowProperty, 0);
            cBorder.SetCurrentValue(Grid.ColumnProperty, 1);

            Grid cGrid = new();
            cGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            cGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            cGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock title = new TextBlock { Text = "II. PODACI O POSLODAVCU" };
            title.SetCurrentValue(Grid.RowProperty, 0);
            title.SetCurrentValue(Grid.ColumnProperty, 0);
            title.SetCurrentValue(Grid.ColumnSpanProperty, 2);
            cGrid.Children.Add(title);

            cGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock name = new TextBlock { Text = "1. Naziv" };
            name.SetCurrentValue(Grid.RowProperty, 1);
            name.SetCurrentValue(Grid.ColumnProperty, 0);
            cGrid.Children.Add(name);

            TextBlock nameD = new TextBlock { Text = _company.Naziv };
            nameD.SetCurrentValue(Grid.RowProperty, 1);
            nameD.SetCurrentValue(Grid.ColumnProperty, 1);
            cGrid.Children.Add(nameD);

            cGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock address = new TextBlock { Text = "2. Adresa/Sjedište" };
            address.SetCurrentValue(Grid.RowProperty, 2);
            address.SetCurrentValue(Grid.ColumnProperty, 0);
            cGrid.Children.Add(address);

            TextBlock addressD = new TextBlock 
            {
                Text = _company.Ulica + " " + _company.Broj + ", " + _company.Mjesto,
                TextWrapping = TextWrapping.Wrap
            };
            addressD.SetCurrentValue(Grid.RowProperty, 2);
            addressD.SetCurrentValue(Grid.ColumnProperty, 1);
            cGrid.Children.Add(addressD);

            cGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock oib = new TextBlock { Text = "3. OIB" };
            oib.SetCurrentValue(Grid.RowProperty, 3);
            oib.SetCurrentValue(Grid.ColumnProperty, 0);
            cGrid.Children.Add(oib);

            TextBlock oibD = new TextBlock { Text = _company.Oib };
            oibD.SetCurrentValue(Grid.RowProperty, 3);
            oibD.SetCurrentValue(Grid.ColumnProperty, 1);
            cGrid.Children.Add(oibD);

            cGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock iban = new TextBlock { Text = "4. IBAN" };
            iban.SetCurrentValue(Grid.RowProperty, 4);
            iban.SetCurrentValue(Grid.ColumnProperty, 0);
            cGrid.Children.Add(iban);

            TextBlock ibanD = new TextBlock { Text = _company.Iban };
            ibanD.SetCurrentValue(Grid.RowProperty, 4);
            ibanD.SetCurrentValue(Grid.ColumnProperty, 1);
            cGrid.Children.Add(ibanD);

            cBorder.Child = cGrid;

            hGrid.Children.Add(cBorder);
        }
        
        private void AddDocumentTimePeriod(Grid grid)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Border tBorder = new();
            tBorder.Style = BorderHeaderStyle;
            tBorder.SetCurrentValue(Grid.RowProperty, 2);

            Grid tGrid = new();

            TextBlock tText = new TextBlock { Text = $"RAZDOBLJE NA KOJE SE OBRAČUN ODNOSI: {_period[0].Date.ToShortDateString()} - {_period[1].Date.ToShortDateString()}" };
            tText.Style = TextBoxCustom;

            tGrid.Children.Add(tText);

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }
    }
}
