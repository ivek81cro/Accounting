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
        private PayrollArchiveHeaderModel _header;
        private List<PayrollArchivePayrollModel> _payroll;
        private List<PayrollArchiveSupplementModel> _supplement;
        private EmployeeModel _employee;

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
            _header = parameters.GetValue<PayrollArchiveHeaderModel>("archiveHeader");
            LoadDataFromDatabase(_header.Id);
        }

        private async void LoadDataFromDatabase(int idArchive)
        {
            _payroll = await _archiveEndpoint.GetArchivePayrolls(idArchive);
            _supplement = await _archiveEndpoint.GetArchiveSupplements(idArchive);
        }

        private async void PrintGridView()
        {
            PrintDialog pd = new PrintDialog();
            pd.PrintTicket.PageOrientation = PageOrientation.Portrait;
            pd.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);

            FixedDocument doc = new FixedDocument();

            #region Main grid
            //Page 1
            Grid gridPage1 = new Grid { Margin = new Thickness(70, 50, 0, 0) };

            AddDocumentTitle(gridPage1, 0);

            await AddDocumentHeader(gridPage1, 1);

            AddGeneralData(gridPage1, 2);

            AddDateSection(gridPage1, 3);

            AddPeriodSection(gridPage1, 4);

            AddWorkHoursSection(gridPage1, 5);

            AddRetirementTaxSection(gridPage1, 6);

            PageContent pc1 = CreatePage(gridPage1, pd, doc);
            doc.Pages.Add(pc1);

            //Page 2
            Grid gridPage2 = new Grid { Margin = new Thickness(70, 50, 0, 0) };
            AddPayedTaxSection(gridPage2, 0);

            PageContent pc2 = CreatePage(gridPage2, pd, doc);
            doc.Pages.Add(pc2);
            #endregion

            CreateDocument(doc.DocumentPaginator);
        }

        private void AddDocumentTitle(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new Grid { Width = 700 };
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock tLeft = new TextBlock
            {
                Text = "OBRAČUN ISPLAĆENE PLAĆENE NAKNADE PLAĆE"
            };
            tLeft.Style = TextBoxCustom;
            tLeft.SetCurrentValue(Grid.ColumnProperty, 0);

            TextBlock tRight = new TextBlock
            {
                Text = "OBRAZAC IP1",
                TextAlignment = TextAlignment.Right,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            tRight.Style = TextBoxCustom;
            tRight.SetCurrentValue(Grid.ColumnProperty, 1);

            tGrid.Children.Add(tLeft);
            tGrid.Children.Add(tRight);

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private async Task AddDocumentHeader(Grid grid, int rowIndex)
        {
            Border hBorder = AddRowToMainGrid(grid, rowIndex);

            Grid hGrid = new();
            hGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            hGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            await AddDocumentHeaderEmployee(hGrid);

            await AddDocumentHeaderCompany(hGrid);

            hBorder.Child = hGrid;

            grid.Children.Add(hBorder);
        }

        private async Task AddDocumentHeaderEmployee(Grid hGrid)
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
            title.Style = TextBoxCustom;
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

        private async Task AddDocumentHeaderCompany(Grid hGrid)
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
            title.Style = TextBoxCustom;
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

        private void AddGeneralData(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new();
            tGrid.RowDefinitions.Add(new RowDefinition());
            tGrid.RowDefinitions.Add(new RowDefinition());
            tGrid.RowDefinitions.Add(new RowDefinition());

            TextBlock tText = new TextBlock { Text = "III. OPĆI PODACI POTREBNI ZA OBRAČUN PLAĆE" };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.Style = TextBoxCustom;
            tGrid.Children.Add(tText);

            Border r1Border = new Border { BorderThickness = new Thickness(0, 1, 0, 0), BorderBrush = Brushes.Black };
            TextBlock r1Text = new TextBlock { Text = "1.1. Ugovorena bruto plaća" };
            r1Border.SetCurrentValue(Grid.RowProperty, 1);
            r1Border.Child = r1Text;
            tGrid.Children.Add(r1Border);

            Border r2Border = new Border { BorderThickness = new Thickness(0, 1, 0, 0), BorderBrush = Brushes.Black };
            TextBlock r2Text = new TextBlock { Text = "1.n." };
            r2Border.SetCurrentValue(Grid.RowProperty, 2);
            r2Border.Child = r2Text;
            tGrid.Children.Add(r2Border);

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddDateSection(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new();
            tGrid.RowDefinitions.Add(new RowDefinition());
            tGrid.RowDefinitions.Add(new RowDefinition());
            tGrid.RowDefinitions.Add(new RowDefinition());
            tGrid.RowDefinitions.Add(new RowDefinition());

            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock tText = new TextBlock { Text = "IV. DATUM ISPLATE/OBRAČUNA" };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.SetCurrentValue(Grid.ColumnProperty, 0);
            tText.SetCurrentValue(Grid.ColumnSpanProperty, 2);
            tText.Style = TextBoxCustom;
            tGrid.Children.Add(tText);

            Border r1Border = new Border { BorderThickness = new Thickness(0, 1, 1, 0), BorderBrush = Brushes.Black };
            TextBlock r1Text = new TextBlock { Text = "1.1. Datum isplate plaće/naknade plaće u cijelosti" };
            r1Border.SetCurrentValue(Grid.RowProperty, 1);
            r1Border.SetCurrentValue(Grid.ColumnProperty, 0);
            r1Border.Child = r1Text;
            tGrid.Children.Add(r1Border);

            Border r1Border2 = new Border { BorderThickness = new Thickness(0, 1, 0, 0), BorderBrush = Brushes.Black };
            TextBlock r1Text2 = new TextBlock
            {
                Text = $"{_header.DatumObracuna.Value.ToShortDateString()}",
                Padding = new Thickness(5, 0, 0, 0),
                TextAlignment = TextAlignment.Center
            };
            r1Border2.SetCurrentValue(Grid.RowProperty, 1);
            r1Border2.SetCurrentValue(Grid.ColumnProperty, 1);
            r1Border2.Child = r1Text2;
            tGrid.Children.Add(r1Border2);

            Border r2Border = new Border { BorderThickness = new Thickness(0, 1, 1, 0), BorderBrush = Brushes.Black };
            TextBlock r2Text = new TextBlock { Text = "1.2. Datum djelomične isplate plaće/naknade plaće u cijelosti" };
            r2Border.SetCurrentValue(Grid.RowProperty, 2);
            r2Border.SetCurrentValue(Grid.ColumnProperty, 0);
            r2Border.Child = r2Text;
            tGrid.Children.Add(r2Border);

            Border r2Border2 = new Border { BorderThickness = new Thickness(0, 1, 0, 0), BorderBrush = Brushes.Black };
            TextBlock r2Text2 = new TextBlock { Text = "" };
            r2Border2.SetCurrentValue(Grid.RowProperty, 2);
            r2Border2.SetCurrentValue(Grid.ColumnProperty, 1);
            r2Border2.Child = r2Text2;
            tGrid.Children.Add(r2Border2);

            Border r3Border = new Border { BorderThickness = new Thickness(0, 1, 1, 0), BorderBrush = Brushes.Black };
            TextBlock r3Text = new TextBlock { Text = "1.3. Datum obračuna u slučaju neisplate plaće/naknade plaće" };
            r3Border.SetCurrentValue(Grid.RowProperty, 3);
            r3Border.SetCurrentValue(Grid.ColumnProperty, 0);
            r3Border.Child = r3Text;
            tGrid.Children.Add(r3Border);

            Border r3Border2 = new Border { BorderThickness = new Thickness(0, 1, 0, 0), BorderBrush = Brushes.Black };
            TextBlock r3Text2 = new TextBlock { Text = "" };
            r3Border2.SetCurrentValue(Grid.RowProperty, 3);
            r3Border2.SetCurrentValue(Grid.ColumnProperty, 1);
            r3Border2.Child = r3Text2;
            tGrid.Children.Add(r3Border2);

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddPeriodSection(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new();

            TextBlock tText = new TextBlock
            {
                Text = $"V. RAZDOBLJE NA KOJE SE PLAĆA ODNOSI: {_header.DatumOd.Value.ToShortDateString()} - " +
                $"{_header.DatumDo.Value.ToShortDateString()}"
            };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.SetCurrentValue(Grid.ColumnProperty, 0);
            tText.SetCurrentValue(Grid.ColumnSpanProperty, 2);
            tText.Style = TextBoxCustom;
            tGrid.Children.Add(tText);

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddWorkHoursSection(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new();
            for (int i = 0; i < 21; i++)
            {
                tGrid.RowDefinitions.Add(new RowDefinition());
            }
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(500, GridUnitType.Pixel) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock tText = new TextBlock
            {
                Text = "VI. PODACI O RADNOM VREMENU I OSTVARENOJ PLAĆI/NAKNADE PLAĆE"
            };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.SetCurrentValue(Grid.ColumnProperty, 0);
            tText.SetCurrentValue(Grid.ColumnSpanProperty, 4);
            tText.Style = TextBoxCustom;
            tGrid.Children.Add(tText);

            #region Data Rows
            #region ROW 1
            AddRowToSection(tGrid, "OPIS", 1, 0, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "SATI", 1, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "ELEMENT\n OBRAČUNA", 1, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "IZNOS", 1, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 2
            AddRowToSection(tGrid, "1", 2, 0, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "2", 2, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "3", 2, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "4", 2, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 3
            AddRowToSection(tGrid, "REDOVNI MJESEČNI FOND SATI", 3, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_header.SatiRada}", 3, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 3, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 3, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 4
            AddRowToSection(tGrid,
                "PODACI O VRSTI I IZNOSIMA OSTVARENE PLAĆE / NAKNADE I BROJU OSTVARENIH SATI RADA / NAKNADE NA TERET POSLODAVCA",
                4, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 4, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 4, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 4, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 5
            AddRowToSection(tGrid, "1.1. redoviti rad prema rasporedu dnevnog vremena", 5, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_header.SatiRada}", 5, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 5, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 5, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 6
            AddRowToSection(tGrid, "1.2. redoviti rad nedjeljom", 6, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 6, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 6, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 6, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 7
            AddRowToSection(tGrid,
                "1.3. redoviti rad blagdanom i neradnim danom utvrđenim posebnim zakonom",
                7, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 7, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 7, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 7, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 8
            AddRowToSection(tGrid, "1.4. redoviti rad noću", 8, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 8, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 8, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 8, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 9
            AddRowToSection(tGrid, "1.5. prekovremeni rad", 9, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 9, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 9, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 9, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 10
            AddRowToSection(tGrid, "1.6. prekovremeni rad nedjeljom", 10, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 10, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 10, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 10, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 11
            AddRowToSection(tGrid,
                "1.7. prekovremeni rad blagdanom i neradnim danom utvrđenim posebnim zakonom",
                11, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 11, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 11, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 11, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 12
            AddRowToSection(tGrid, "1.8. prekovremeni rad noću", 12, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 12, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 12, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 12, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 13
            AddRowToSection(tGrid, "1.9. propravnost", 13, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 13, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 13, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 13, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 14
            AddRowToSection(tGrid, "2.1. naknada za godišnji odmor", 14, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 14, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 14, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 14, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 15
            AddRowToSection(tGrid,
                "2.2. naknada za vrijeme privremene nesposobnosti za rad zbog bolesti na teret poslodavca",
                15, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 15, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 15, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 15, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 16
            AddRowToSection(tGrid,
                "2.3. naknada za vrijeme privremene nesposobnosti za rad zbog bolesti na teret HZZO",
                16, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 16, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 16, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 16, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 17
            AddRowToSection(tGrid,
                "2.4. naknada za dane blagdana i neradne dane utvrđene posebnim zakonom",
                17, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 17, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 17, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 17, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 18
            AddRowToSection(tGrid,
                "2.5. naknada za vrijeme u kojem je radnik odbio raditi zbog neprovedenih mjera zaštite zdravlja i sigurnosti na radu",
                18, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 18, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 18, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 18, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 19
            AddRowToSection(tGrid,
                "2.6. naknada za vrijeme prekida rada do kojeg je došlo krivnjom poslodavca ili uslijed drugih okolnosti za koje radnik nije odgovoran",
                19, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 19, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 19, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 19, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 20
            AddRowToSection(tGrid,
                "2.7. naknada za vrijeme kada radnik ne radi zbog drugih opravdanih razloga određenih zakonom, dr. propisom, kol. ugovorom",
                20, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 20, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 20, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "", 20, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion
            #endregion

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddRetirementTaxSection(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new();
            for (int i = 0; i < 21; i++)
            {
                tGrid.RowDefinitions.Add(new RowDefinition());
            }
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(600, GridUnitType.Pixel) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock tText = new TextBlock
            {
                Text = "VII. UTVRĐIVANJE DOPRINOSA IZ OSNOVICE"
            };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.SetCurrentValue(Grid.ColumnProperty, 0);
            tText.SetCurrentValue(Grid.ColumnSpanProperty, 4);
            tText.Style = TextBoxCustom;
            tGrid.Children.Add(tText);

            #region Data Rows Determined ammount
            #region ROW 1
            AddRowToSection(tGrid, "OPIS", 1, 0, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "IZNOS", 1, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 2
            AddRowToSection(tGrid, "1", 2, 0, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddRowToSection(tGrid, "2", 2, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 3
            AddRowToSection(tGrid, "OSNOVICA ZA UTVRĐIVANJE DOPRINOSA PREMA OPOREZIVIM NAKNADAMA", 3, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_payroll[0].Bruto}", 3, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 4
            AddRowToSection(tGrid,
                "1.1. doprinos za mirovinsko osiguranje na temelju generacijske solidarnosti",
                4, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_payroll[0].Mio1}", 4, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 5
            AddRowToSection(tGrid,
                "1.2. doprinos za mirovinsko osiguranje na temelju individsualne kapitalizirane štednje",
                5, 0, new Thickness(0, 1, 1, 1), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_payroll[0].Mio2}", 5, 3, new Thickness(0, 1, 0, 1), TextAlignment.Center);
            #endregion
            #endregion

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddPayedTaxSection(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new Grid { Width = 700 };
            for (int i = 0; i < 21; i++)
            {
                tGrid.RowDefinitions.Add(new RowDefinition());
            }
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(600, GridUnitType.Pixel) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock tText = new TextBlock
            {
                Text = "VIII. UTVRĐIVANJE POREZA NA DOHODAK I PRIREZA POREZU NA DOHODAK"
            };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.SetCurrentValue(Grid.ColumnProperty, 0);
            tText.SetCurrentValue(Grid.ColumnSpanProperty, 4);
            tText.Style = TextBoxCustom;
            tGrid.Children.Add(tText);

            #region Data Rows Determined ammount
            #region ROW 1
            AddRowToSection(tGrid, "1. IZNOS OSTVARENOG OPOREZIVOG PRIMITKA", 1, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_payroll[0].Bruto}", 1, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 2
            AddRowToSection(tGrid, "2. IZDACI (VIII.2.1. + VIII.2.2.)",
                2, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_payroll[0].Mio1 + _payroll[0].Mio2}", 2, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 3
            AddRowToSection(tGrid,
                "2.1. plaćeni iznosi doprinosa za mirovinsko osiguranje na temelju generacijske solidarnosti",
                3, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_payroll[0].Mio1}", 3, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 4
            AddRowToSection(tGrid,
                "2.2. plaćeni iznosi doprinosa za mirovinsko osiguranje na temelju individualne kapitalizirane štednje",
                4, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_payroll[0].Mio2}", 4, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 5
            AddRowToSection(tGrid, "3. DOHODAK (VIII.1. - VIII.2.)",
                5, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_payroll[0].Dohodak}", 5, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 6
            AddRowToSection(tGrid, "4. NEOPOREZIVI ODBITAK (UKUPAN FAKTOR OSOBNOG ODBITKA 2.22)",
                6, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, "", 6, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 7
            AddRowToSection(tGrid, "5. POREZNA OSNOVICA (VIII.3. - VIII.4.)",
                7, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_payroll[0].PoreznaOsnovica}", 7, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 8
            AddRowToSection(tGrid, "6. UKUPAN IZNOS POREZA (VIII.6.1. + VIII.6.2. + VIII.6.3.)",
                8, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddRowToSection(tGrid, $"{_payroll[0].UkupnoPorez}", 8, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion
            #endregion

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddRowToSection(Grid tGrid, string data, int rowIndex, int colIndex, Thickness thickness, TextAlignment alignment)
        {
            Border rBorder = new Border { BorderThickness = thickness, BorderBrush = Brushes.Black };
            TextBlock rText = new TextBlock { Text = data, TextAlignment = alignment, TextWrapping = TextWrapping.Wrap };
            rBorder.SetCurrentValue(Grid.RowProperty, rowIndex);
            rBorder.SetCurrentValue(Grid.ColumnProperty, colIndex);
            rBorder.Child = rText;

            tGrid.Children.Add(rBorder);
        }

        private Border AddRowToMainGrid(Grid grid, int rowIndex)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Border tBorder = new();
            tBorder.Style = BorderHeaderStyle;
            tBorder.SetCurrentValue(Grid.RowProperty, rowIndex);
            return tBorder;
        }

        #region Create page
        private PageContent CreatePage(Grid gridPage1, PrintDialog pd, FixedDocument doc)
        {
            doc.DocumentPaginator.PageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
            FixedPage fxpg = new FixedPage();
            fxpg.Width = doc.DocumentPaginator.PageSize.Width;
            fxpg.Height = doc.DocumentPaginator.PageSize.Height;
            fxpg.Children.Add(gridPage1);

            PageContent pc = new PageContent();
            ((IAddChild)pc).AddChild(fxpg);
            return pc;
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
        #endregion
    }
}
