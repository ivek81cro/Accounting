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

namespace PayrollModule.PrintFlow
{
    public class PayrollReportViewModel : BindableBase, IDialogAware
    {
        private readonly ICompanyEndpoint _companyEndpoint;
        private readonly IPayrollArchiveEndpoint _archiveEndpoint;
        private readonly IEmployeeEndpoint _employeeEndpoint;
        private readonly ICityEndpoint _cityEndpoint;

        private CompanyModel _company;
        private PayrollArchiveHeaderModel _header;
        private PayrollArchivePayrollModel _selectedPayroll;
        private List<PayrollArchiveSupplementModel> _supplement;
        private EmployeeModel _employee;

        public PayrollReportViewModel(ICompanyEndpoint companyEndpoint,
                                      IPayrollArchiveEndpoint archiveEndpoint,
                                      IEmployeeEndpoint employeeEndpoint,
                                      ICityEndpoint cityEndpoint)
        {
            _companyEndpoint = companyEndpoint;
            _archiveEndpoint = archiveEndpoint;
            _employeeEndpoint = employeeEndpoint;
            _cityEndpoint = cityEndpoint;

            PrintCommand = new DelegateCommand(CreateReportPreview);
            SelectEmployeeCommand = new DelegateCommand(SelectedEmployeeReport);
            SelectAllCommand = new DelegateCommand(AllEmployeesReport);

            ReadStyles();
        }

        public DelegateCommand PrintCommand { get; private set; }
        public DelegateCommand SelectEmployeeCommand { get; private set; }
        public DelegateCommand SelectAllCommand { get; private set; }

        public string Title => "Ispis obračuna plaće";

        public event Action<IDialogResult> RequestClose;

        public Style BorderTitleStyle { get; private set; }
        public Style BorderHeaderStyle { get; private set; }
        public Style TextBoxCustom { get; private set; }


        private List<PayrollArchivePayrollModel> _payroll;
        public List<PayrollArchivePayrollModel> Payroll
        {
            get { return _payroll; }
            set { SetProperty(ref _payroll, value); }
        }

        private FixedDocumentSequence _printDocument;
        public FixedDocumentSequence PrintDocument
        {
            get { return _printDocument; }
            set { SetProperty(ref _printDocument, value); }
        }

        private List<EmployeeModel> _employees = new();
        public List<EmployeeModel> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }

        private EmployeeModel _selectedEmployee;
        public EmployeeModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { SetProperty(ref _selectedEmployee, value); }
        }

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
            LoadInitData();            
        }

        private async void LoadInitData()
        {
            await LoadDataFromDatabase(_header.Id);
        }

        private async Task LoadDataFromDatabase(int idArchive)
        {
            _payroll = await _archiveEndpoint.GetArchivePayrolls(idArchive);
            foreach (var emp in Payroll)
            {
                Employees.Add(new EmployeeModel
                {
                    Ime = emp.Ime,
                    Prezime = emp.Prezime,
                    Oib = emp.Oib
                });
            }
        }

        private async void AllEmployeesReport()
        {
            if(_payroll.Count <= 1)
            {
                await LoadDataFromDatabase(_header.Id);
            }
            CreateReportPreview();
        }

        private void SelectedEmployeeReport()
        {
            if (SelectedEmployee != null && SelectedEmployee.Ime != null)
            {
                Payroll = Payroll.Where(x => x.Oib == SelectedEmployee.Oib).ToList();
            }
            CreateReportPreview();
        }

        private async void CreateReportPreview()
        {
            PrintDialog pd = new PrintDialog();
            pd.PrintTicket.PageOrientation = PageOrientation.Portrait;
            pd.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);

            FixedDocument doc = new FixedDocument();

            #region Document Pages

            foreach (var payroll in _payroll)
            {
                _selectedPayroll = payroll;
                //Page 1
                await AddPage1(pd, doc);

                //Page 2
                await AddPage2(pd, doc);
            }
            #endregion

            CreateDocument(doc.DocumentPaginator);
        }

        #region Generate document preview
        private async Task AddPage1(PrintDialog pd, FixedDocument doc)
        {
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
        }

        private async Task AddPage2(PrintDialog pd, FixedDocument doc)
        {
            Grid gridPage2 = new Grid { Margin = new Thickness(70, 50, 0, 0) };

            AddPayedTaxSection(gridPage2, 0);

            AddLocalTaxSection(gridPage2, 1);

            await AddTaxDeductionSection(gridPage2, 2);

            await AddSupplementsSection(gridPage2, 3);

            AddSuspensionsSection(gridPage2, 4);

            AddTotalPayoutSection(gridPage2, 5);

            AddTaxAddedToPayroll(gridPage2, 6);

            AddTotalCostSection(gridPage2, 7);

            AddSignatureSection(gridPage2, 8);

            PageContent pc2 = CreatePage(gridPage2, pd, doc);
            doc.Pages.Add(pc2);
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

            _employee = await _employeeEndpoint.GetByOib(_selectedPayroll.Oib);

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

            #region ROW 0
            TextBlock tText = new TextBlock { Text = "III. OPĆI PODACI POTREBNI ZA OBRAČUN PLAĆE" };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.Style = TextBoxCustom;
            tGrid.Children.Add(tText);
            #endregion

            #region Data Rows
            #region ROW 1
            AddDataToRowCell(tGrid, "1.1. Ugovorena bruto plaća", 1, 0, new Thickness(0, 1, 0, 0), TextAlignment.Left);
            #endregion

            #region ROW 2
            AddDataToRowCell(tGrid, "1.n.", 2, 0, new Thickness(0, 1, 0, 0), TextAlignment.Left);
            #endregion
            #endregion

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddDateSection(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new();
            for (int i = 0; i < 4; i++)
            {
                tGrid.RowDefinitions.Add(new RowDefinition());
            }
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock tText = new TextBlock
            {
                Text = "IV. DATUM ISPLATE/OBRAČUNA"
            };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.SetCurrentValue(Grid.ColumnProperty, 0);
            tText.SetCurrentValue(Grid.ColumnSpanProperty, 2);
            tText.Style = TextBoxCustom;
            tGrid.Children.Add(tText);

            #region Data Rows
            #region ROW 1
            AddDataToRowCell(tGrid, "1.1. Datum isplate plaće/naknade plaće u cijelosti",
                1, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_header.DatumObracuna.Value.ToShortDateString()}",
                1, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 2
            AddDataToRowCell(tGrid, "1.2. Datum djelomične isplate plaće/naknade plaće u cijelosti",
                2, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 2, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 3
            AddDataToRowCell(tGrid, "1.3. Datum obračuna u slučaju neisplate plaće/naknade plaće",
                3, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 3, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion
            #endregion

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
            AddDataToRowCell(tGrid, "OPIS", 1, 0, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "SATI", 1, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "ELEMENT\n OBRAČUNA", 1, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "IZNOS", 1, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 2
            AddDataToRowCell(tGrid, "1", 2, 0, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "2", 2, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "3", 2, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "4", 2, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 3
            AddDataToRowCell(tGrid, "REDOVNI MJESEČNI FOND SATI", 3, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_header.SatiRada}", 3, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 3, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 3, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 4
            AddDataToRowCell(tGrid,
                "PODACI O VRSTI I IZNOSIMA OSTVARENE PLAĆE / NAKNADE I BROJU OSTVARENIH SATI RADA / NAKNADE NA TERET POSLODAVCA",
                4, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 4, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 4, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 4, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 5
            AddDataToRowCell(tGrid, "1.1. redoviti rad prema rasporedu dnevnog vremena", 5, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_header.SatiRada}", 5, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 5, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 5, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 6
            AddDataToRowCell(tGrid, "1.2. redoviti rad nedjeljom", 6, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 6, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 6, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 6, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 7
            AddDataToRowCell(tGrid,
                "1.3. redoviti rad blagdanom i neradnim danom utvrđenim posebnim zakonom",
                7, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 7, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 7, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 7, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 8
            AddDataToRowCell(tGrid, "1.4. redoviti rad noću", 8, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 8, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 8, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 8, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 9
            AddDataToRowCell(tGrid, "1.5. prekovremeni rad", 9, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 9, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 9, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 9, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 10
            AddDataToRowCell(tGrid, "1.6. prekovremeni rad nedjeljom", 10, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 10, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 10, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 10, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 11
            AddDataToRowCell(tGrid,
                "1.7. prekovremeni rad blagdanom i neradnim danom utvrđenim posebnim zakonom",
                11, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 11, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 11, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 11, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 12
            AddDataToRowCell(tGrid, "1.8. prekovremeni rad noću", 12, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 12, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 12, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 12, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 13
            AddDataToRowCell(tGrid, "1.9. propravnost", 13, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 13, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 13, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 13, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 14
            AddDataToRowCell(tGrid, "2.1. naknada za godišnji odmor", 14, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 14, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 14, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 14, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 15
            AddDataToRowCell(tGrid,
                "2.2. naknada za vrijeme privremene nesposobnosti za rad zbog bolesti na teret poslodavca",
                15, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 15, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 15, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 15, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 16
            AddDataToRowCell(tGrid,
                "2.3. naknada za vrijeme privremene nesposobnosti za rad zbog bolesti na teret HZZO",
                16, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 16, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 16, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 16, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 17
            AddDataToRowCell(tGrid,
                "2.4. naknada za dane blagdana i neradne dane utvrđene posebnim zakonom",
                17, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 17, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 17, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 17, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 18
            AddDataToRowCell(tGrid,
                "2.5. naknada za vrijeme u kojem je radnik odbio raditi zbog neprovedenih mjera zaštite zdravlja i sigurnosti na radu",
                18, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 18, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 18, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 18, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 19
            AddDataToRowCell(tGrid,
                "2.6. naknada za vrijeme prekida rada do kojeg je došlo krivnjom poslodavca ili uslijed drugih okolnosti za koje radnik nije odgovoran",
                19, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 19, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 19, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 19, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 20
            AddDataToRowCell(tGrid,
                "2.7. naknada za vrijeme kada radnik ne radi zbog drugih opravdanih razloga određenih zakonom, dr. propisom, kol. ugovorom",
                20, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 20, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 20, 2, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 20, 3, new Thickness(0, 1, 0, 0), TextAlignment.Center);
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
            AddDataToRowCell(tGrid, "OPIS", 1, 0, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "IZNOS", 1, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 2
            AddDataToRowCell(tGrid, "1", 2, 0, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "2", 2, 1, new Thickness(0, 1, 0, 0), TextAlignment.Center);
            #endregion

            #region ROW 3
            AddDataToRowCell(tGrid, "OSNOVICA ZA UTVRĐIVANJE DOPRINOSA PREMA OPOREZIVIM NAKNADAMA", 3, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.Bruto}", 3, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 4
            AddDataToRowCell(tGrid,
                "1.1. doprinos za mirovinsko osiguranje na temelju generacijske solidarnosti",
                4, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.Mio1}", 4, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 5
            AddDataToRowCell(tGrid,
                "1.2. doprinos za mirovinsko osiguranje na temelju individsualne kapitalizirane štednje",
                5, 0, new Thickness(0, 1, 1, 1), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.Mio2}", 5, 3, new Thickness(0, 1, 0, 1), TextAlignment.Right);
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
            tText.SetCurrentValue(Grid.ColumnSpanProperty, 2);
            tText.Style = TextBoxCustom;
            tGrid.Children.Add(tText);

            #region Data Rows
            #region ROW 1
            AddDataToRowCell(tGrid, "1. IZNOS OSTVARENOG OPOREZIVOG PRIMITKA", 1, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.Bruto}", 1, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 2
            AddDataToRowCell(tGrid, "2. IZDACI (VIII.2.1. + VIII.2.2.)",
                2, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.Mio1 + _selectedPayroll.Mio2}", 2, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 3
            AddDataToRowCell(tGrid,
                "2.1. plaćeni iznosi doprinosa za mirovinsko osiguranje na temelju generacijske solidarnosti",
                3, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.Mio1}", 3, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 4
            AddDataToRowCell(tGrid,
                "2.2. plaćeni iznosi doprinosa za mirovinsko osiguranje na temelju individualne kapitalizirane štednje",
                4, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.Mio2}", 4, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 5
            AddDataToRowCell(tGrid, "3. DOHODAK (VIII.1. - VIII.2.)",
                5, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.Dohodak}", 5, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 6
            AddDataToRowCell(tGrid, "4. NEOPOREZIVI ODBITAK (UKUPAN FAKTOR OSOBNOG ODBITKA 2.22)",
                6, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.Odbitak}", 6, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 7
            AddDataToRowCell(tGrid, "5. POREZNA OSNOVICA (VIII.3. - VIII.4.)",
                7, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.PoreznaOsnovica}", 7, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 8
            AddDataToRowCell(tGrid, "6. UKUPAN IZNOS POREZA (VIII.6.1. + VIII.6.2. + VIII.6.3.)",
                8, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.UkupnoPorez}", 8, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion
            #endregion

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddLocalTaxSection(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new();
            for (int i = 0; i < 4; i++)
            {
                tGrid.RowDefinitions.Add(new RowDefinition());
            }
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300, GridUnitType.Pixel) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300, GridUnitType.Pixel) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            #region Data Rows
            #region ROW 0
            AddDataToRowCell(tGrid, "STOPA (%)", 0, 0, new Thickness(0, 0, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "OSNOVICA", 0, 1, new Thickness(0, 0, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 0, 2, new Thickness(0, 0, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 1
            AddDataToRowCell(tGrid, "6.1. 20%", 1, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 1, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.PoreznaStopa1}", 1, 2, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 2
            AddDataToRowCell(tGrid, "6.2. 30%", 2, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 2, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.PoreznaStopa2}", 2, 2, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 3
            AddDataToRowCell(tGrid, "6.3.", 3, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 3, 1, new Thickness(0, 1, 1, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "", 3, 2, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion
            #endregion

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private async Task AddTaxDeductionSection(Grid grid, int rowIndex)
        {
            var city = await _cityEndpoint.GetByName(_employee.Mjesto);
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new();
            for (int i = 0; i < 7; i++)
            {
                tGrid.RowDefinitions.Add(new RowDefinition());
            }
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            #region Data Rows
            #region ROW 0
            AddDataToRowCell(tGrid, "7.1. IZNOS UMANJENJA ZA PREBIVALIŠTE I. SKUPINA I GRAD VUKOVAR (0.00%)",
                0, 0, new Thickness(0, 0, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 0, 1, new Thickness(0, 0, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 1
            AddDataToRowCell(tGrid, "7.2. IZNOS UMANJENJA OBVEZE POREZA ZA POSTOTAK INVALIDNOSTI HRVI-a (0.00%)",
                1, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 1, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 2
            AddDataToRowCell(tGrid, $"8. IZNOS PRIREZA ({city.Prirez}%)",
                2, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.Prirez}", 2, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 3
            AddDataToRowCell(tGrid, $"9. KOREKCIJE POREZA I PRIREZA +/- (VIII.9.1 + VIII.9.2)",
                3, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 3, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 4
            AddDataToRowCell(tGrid, $"9.1.",
                4, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 4, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 5
            AddDataToRowCell(tGrid, $"9.2.",
                5, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 5, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 6
            AddDataToRowCell(tGrid, $"10. UKUPNO POREZ I PRIREZ (VIII.6. - VIII.7. + VIII.8. + VIII.9.)",
                6, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.UkupnoPorezPrirez}", 6, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion
            #endregion

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private async Task AddSupplementsSection(Grid grid, int rowIndex)
        {
            _supplement = await _archiveEndpoint.GetArchiveSupplements(_header.Id);
            var supp = _supplement.Where(x => x.Oib == _selectedPayroll.Oib).ToList();
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new();
            for (int i = 0; i < 7; i++)
            {
                tGrid.RowDefinitions.Add(new RowDefinition());
            }
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(600, GridUnitType.Pixel) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            #region ROW 0
            Border bottom = new Border() { BorderThickness = new Thickness(0, 0, 1, 0), BorderBrush = Brushes.Black };
            TextBlock tText = new TextBlock
            {
                Text = "X. VRSTE I IZNOSI NEOPOREZIVIH NAKNADA (X.1. + ... + X.n)"
            };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.SetCurrentValue(Grid.ColumnProperty, 0);
            tText.Style = TextBoxCustom;
            bottom.Child = tText;
            tGrid.Children.Add(bottom);

            AddDataToRowCell(tGrid, $"{supp.Sum(x => x.Iznos)}", 0, 1, new Thickness(0, 0, 0, 0), TextAlignment.Right);
            #endregion

            #region Data Rows
            #region ROWS 1-n
            int j = 1;
            foreach (var item in supp)
            {
                AddDataToRowCell(tGrid, $"{j}. {item.Naziv}", j, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
                AddDataToRowCell(tGrid, $"{item.Iznos}", j, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
                j++;
            }
            #endregion
            #endregion

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddSuspensionsSection(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new();
            tGrid.RowDefinitions.Add(new RowDefinition());

            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(600, GridUnitType.Pixel) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            #region ROW 0
            Border bottom = new Border() { BorderThickness = new Thickness(0, 0, 1, 0), BorderBrush = Brushes.Black };
            TextBlock tText = new TextBlock
            {
                Text = "XI. VRSTE I IZNOSI OBUSTAVA IZ PLAĆE (XI.1. + ... + XI.n)"
            };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.SetCurrentValue(Grid.ColumnProperty, 0);
            tText.Style = TextBoxCustom;
            bottom.Child = tText;
            tGrid.Children.Add(bottom);

            AddDataToRowCell(tGrid, "", 0, 1, new Thickness(0, 0, 0, 0), TextAlignment.Right);
            #endregion

            //No data rows

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddTotalPayoutSection(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);

            Grid tGrid = new();
            for (int i = 0; i < 4; i++)
            {
                tGrid.RowDefinitions.Add(new RowDefinition());
            }

            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(600, GridUnitType.Pixel) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            #region ROW 0
            Border bottom = new Border() { BorderThickness = new Thickness(0, 0, 1, 0), BorderBrush = Brushes.Black };
            TextBlock tText = new TextBlock
            {
                Text = "XII. IZNOS ZA ISPLATU NAKON OBUSTAVA (IX. + X. - XI.)"
            };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.SetCurrentValue(Grid.ColumnProperty, 0);
            tText.Style = TextBoxCustom;
            bottom.Child = tText;
            tGrid.Children.Add(bottom);

            var sumSupp = _supplement.Where(x => x.Oib == _selectedPayroll.Oib).Sum(y => y.Iznos);
            var ammount = _selectedPayroll.Neto + sumSupp;
            AddDataToRowCell(tGrid, $"{ammount}", 0, 1, new Thickness(0, 0, 0, 0), TextAlignment.Right);
            #endregion

            #region Data Rows
            #region ROW 1
            AddDataToRowCell(tGrid, "1. Iznos za isplatu isplaćen radniku na redovan račun", 1, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{ammount}", 1, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 2
            AddDataToRowCell(tGrid, "1. Iznos plaće/naknade plaće isplaćen radniku za račun iz čl.212 Ovršnog zakona", 2, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 2, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 3
            AddDataToRowCell(tGrid, "1. Iznos za isplatu isplaćen radniku u gotovini", 3, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, "", 3, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion
            #endregion

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddTaxAddedToPayroll(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);
            Grid tGrid = new();
            for (int i = 0; i < 7; i++)
            {
                tGrid.RowDefinitions.Add(new RowDefinition());
            }

            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(600, GridUnitType.Pixel) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            #region ROW 0
            TextBlock tText = new TextBlock
            {
                Text = "XIII. DOPRINOSI NA OSNOVICU"
            };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.SetCurrentValue(Grid.ColumnProperty, 0);
            tText.SetCurrentValue(Grid.ColumnSpanProperty, 2);
            tText.Style = TextBoxCustom;
            tGrid.Children.Add(tText);
            #endregion

            #region DataRows
            #region ROW 1
            AddDataToRowCell(tGrid, "1. OSNOVICA ZA UTVRĐIVANJE DOPRINOSA NA OSNOVICU", 1, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.Bruto}", 1, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 2
            AddDataToRowCell(tGrid, "2. IZNOS DOPRINOSA NA OSNOVICU (XIII.2.1. + ... + XIII.2.n)", 2, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.DoprinosZdravstvo}", 2, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion

            #region ROW 3
            AddDataToRowCell(tGrid, "2.1. Doprinos za zdravstveno osiguranje prema plaći/naknadi plaće", 3, 0, new Thickness(0, 1, 1, 0), TextAlignment.Left);
            AddDataToRowCell(tGrid, $"{_selectedPayroll.DoprinosZdravstvo}", 3, 1, new Thickness(0, 1, 0, 0), TextAlignment.Right);
            #endregion
            #endregion

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddTotalCostSection(Grid grid, int rowIndex)
        {
            Border tBorder = AddRowToMainGrid(grid, rowIndex);
            tBorder.BorderThickness = new Thickness(1);
            Grid tGrid = new();
            tGrid.RowDefinitions.Add(new RowDefinition());
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(600, GridUnitType.Pixel) });
            tGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            #region ROW 0
            Border bottom = new Border() { BorderThickness = new Thickness(0, 0, 1, 0), BorderBrush = Brushes.Black };
            TextBlock tText = new TextBlock
            {
                Text = "XIV. UKUPNI TROŠAK RADA (V.2. + X. + VIII.2.)"
            };
            tText.SetCurrentValue(Grid.RowProperty, 0);
            tText.SetCurrentValue(Grid.ColumnProperty, 0);
            tText.SetCurrentValue(Grid.ColumnSpanProperty, 2);
            tText.Style = TextBoxCustom;
            bottom.Child = tText;
            tGrid.Children.Add(bottom);

            AddDataToRowCell(tGrid, $"{_selectedPayroll.Bruto + _selectedPayroll.DoprinosZdravstvo}", 0, 1, new Thickness(0, 0, 0, 0), TextAlignment.Right);
            #endregion

            tBorder.Child = tGrid;

            grid.Children.Add(tBorder);
        }

        private void AddSignatureSection(Grid grid, int rowIndex)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Pixel) });

            Grid tGrid = new();
            tGrid.SetCurrentValue(Grid.RowProperty, rowIndex);
            tGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Pixel) });
            tGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Pixel) });

            #region ROWS
            AddDataToRowCell(tGrid, "Potpis ovlaštene osobe poslodavca", 0, 0, new Thickness(0, 0, 0, 0), TextAlignment.Center);
            AddDataToRowCell(tGrid, "_________________________________", 1, 0, new Thickness(0, 0, 0, 0), TextAlignment.Center);
            #endregion

            grid.Children.Add(tGrid);
        }

        private static void AddDataToRowCell(Grid tGrid, string data, int rowIndex, int colIndex, Thickness thickness, TextAlignment alignment)
        {
            Border rBorder = new Border { BorderThickness = thickness, BorderBrush = Brushes.Black };
            TextBlock rText = new TextBlock
            {
                Text = data,
                TextAlignment = alignment,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center
            };
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
        #endregion

        #region Create page
        private PageContent CreatePage(Grid gridPage, PrintDialog pd, FixedDocument doc)
        {
            doc.DocumentPaginator.PageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
            FixedPage fxpg = new FixedPage();
            fxpg.Width = doc.DocumentPaginator.PageSize.Width;
            fxpg.Height = doc.DocumentPaginator.PageSize.Height;
            fxpg.Children.Add(gridPage);

            PageContent pc = new PageContent();
            ((IAddChild)pc).AddChild(fxpg);
            return pc;
        }
        #endregion

        #region Create document
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
