# Accounting Management System

A comprehensive accounting management system built with .NET, featuring a modular WPF desktop application and RESTful API backend. This system provides complete accounting functionality including journal entries, payroll management, VAT handling, asset tracking, and financial reporting.

## 🏗️ Architecture

The solution follows a modular architecture with clear separation of concerns:

- **AccountingUI.WPF** - Main WPF desktop application
- **Accounting.Api** - RESTful API backend service (.NET 8)
- **Accounting.DataManager** - Data access layer with repository pattern
- **AccountingUI.Core** - Core business logic and services (.NET 5)
- **Module Projects** - Feature-specific modules for modularity

## 🚀 Features

### Accounting & Journal
- **Journal Management** - Create, edit, and process accounting journal entries
- **General Ledger** - Complete ledger with filtering and balance tracking
- **Account Cards** - Detailed account history and balance information
- **Book Accounts** - Chart of accounts management
- **Balance Sheet** - Financial statements and balance sheet generation

### Business Operations
- **Purchase Orders (URA)** - Incoming goods and purchase invoice processing
  - Retail purchases (Primke)
  - Wholesale operations (Repro)
  - Other operations (Rest)
- **Sales Operations (IRA)** - Sales invoice management
  - Retail sales
  - HZZO (Health insurance) operations
- **VAT Management** - VAT tracking and reporting
- **Cash Register** - Daily cash operations

### Human Resources
- **Payroll Module** - Complete payroll processing
  - Employee salary calculation
  - Supplements and deductions
  - Payroll archives
  - JOPPD reporting (Croatian tax reporting)
- **Employee Management** - Employee records and information
- **Travel Orders** - Business travel expense management

### Other Modules
- **Partners Module** - Customer and supplier management
- **Company Module** - Company information and settings
- **Bank Reports** - Bank statement processing and reconciliation
- **Assets Module** - Fixed asset tracking and depreciation
- **Cities Module** - Location and address management
- **Database Backup** - Automated backup functionality

## 🛠️ Technology Stack

### Backend
- **.NET 8** - API backend
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM for database access
- **SQL Server** - Database
- **Dapper** - Micro-ORM for data access layer
- **JWT Authentication** - Secure API authentication
- **Swagger/OpenAPI** - API documentation

### Frontend
- **.NET 5/8** - Desktop application
- **WPF** - Windows Presentation Foundation
- **Prism** - MVVM framework with DryIoc container
- **AutoMapper** - Object-to-object mapping

### Additional Libraries
- **ExcelDataReader** - Excel file processing
- **Newtonsoft.Json** - JSON serialization

## 📋 Prerequisites

- Visual Studio 2022 or later
- .NET 8.0 SDK
- .NET 5.0 SDK
- SQL Server 2019 or later
- Windows 10/11 (for WPF application)

## ⚙️ Installation

### 1. Clone the Repository
```bash
git clone https://github.com/ivek81cro/Accounting.git
cd Accounting
```

### 2. Database Setup
1. Create a SQL Server database
2. Update connection strings in:
   - `Accounting.Api/appsettings.json`
   - `AccountingUI.WPF/appsettings.json`

```json
"ConnectionStrings": {
  "AccountingConnStr": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;"
}
```

3. Run Entity Framework migrations:
```bash
cd Accounting.Api
dotnet ef database update
```

### 3. Configure API Settings
Update `appsettings.json` in the API project:
```json
{
  "Secret": {
    "SecurityKey": "YOUR_SECURE_KEY_HERE"
  }
}
```

### 4. Build the Solution
```bash
dotnet restore
dotnet build
```

## 🚀 Running the Application

### Start the API Backend
```bash
cd Accounting.Api
dotnet run
```
The API will be available at `http://localhost:5050`

### Start the WPF Application
1. Set `AccountingUI.WPF` as the startup project in Visual Studio
2. Press F5 or click Start

Alternatively, from command line:
```bash
cd AccountingUI.WPF
dotnet run
```

### As Windows Service
The API can be deployed as a Windows Service:
```bash
sc create AccountingApiService binPath="C:\path\to\Accounting.Api.exe"
sc start AccountingApiService
```

## 📁 Project Structure

```
Accounting/
├── Accounting.Api/              # REST API backend
│   ├── Controllers/             # API endpoints
│   ├── Authentication/          # JWT authentication
│   └── Data/                    # EF Core DbContext
├── Accounting.DataManager/      # Data access layer
│   ├── DataAccess/              # Repository implementations
│   └── Models/                  # Data models
├── AccountingUI.Core/           # Core business logic
│   ├── Services/                # Business services
│   └── Models/                  # Domain models
├── AccountingUI.WPF/           # Main WPF application
│   ├── Views/                   # XAML views
│   ├── ViewModels/              # View models
│   └── Dialogs/                 # Dialog windows
├── AssetsModule/                # Asset management module
├── BackupModule/                # Database backup module
├── BalanceSheetModule/          # Balance sheet module
├── BankReportsModule/           # Bank statement module
├── BookAccountsModule/          # Chart of accounts module
├── BookIraModule/               # Sales operations module
├── BookJournalModule/           # Journal entry module
├── BookUraModule/               # Purchase operations module
├── CitiesModule/                # Location management module
├── CompanyModule/               # Company settings module
├── EmployeeModule/              # Employee management module
├── LoginModule/                 # Authentication module
├── PartnersModule/              # Partner management module
├── PayrollModule/               # Payroll processing module
├── TravelOrdersModule/          # Travel orders module
└── VATModule/                   # VAT management module
```

## 🔐 Authentication

The system uses JWT (JSON Web Tokens) for API authentication:
1. Login through the WPF application
2. Token is automatically managed by the ApiService
3. All API requests include the Bearer token in headers

## 📊 Key Workflows

### Processing Purchase Invoices
1. Import data from Excel file
2. Review and validate entries
3. Map accounts using account pairing
4. Process to journal automatically or manually
5. Verify double-entry bookkeeping (Dugovna = Potražna)

### Payroll Processing
1. Enter employee hours and supplements
2. Calculate payroll with tax deductions
3. Generate JOPPD reports
4. Archive processed payroll
5. Export to journal entries

### Journal Entry Management
1. Create manual or automated journal entries
2. Review unprocessed journals
3. Post to general ledger
4. Generate reports and account cards

## 🗃️ Database

The system uses SQL Server with stored procedures for data access. Key tables include:
- AccountingJournal - Journal entries
- BookAccounts - Chart of accounts
- Employees - Employee records
- Partners - Customers and suppliers
- Payroll - Payroll transactions
- VAT - VAT records
- Assets - Fixed assets

## 🔧 Configuration

### Development vs Production
The application supports environment-specific configuration:
- `appsettings.Development.json` - Development settings
- `appsettings.Production.json` - Production settings

### Localization
- Default culture: Croatian (hr-HR)
- Currency: HRK (Croatian Kuna)
- Date format: dd.MM.yyyy

## 🤝 Contributing

Contributions are welcome! Please follow these steps:
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 API Documentation

When running in development mode, Swagger UI is available at:
```
http://localhost:5050/swagger
```

### Key Endpoints
- `/api/Journal` - Journal entry operations
- `/api/Payroll` - Payroll management
- `/api/Partners` - Partner management
- `/api/Employee` - Employee management
- `/api/Vat` - VAT operations
- `/api/Assets` - Asset management
- `/api/Authentication` - User authentication

## 🐛 Troubleshooting

### Connection Issues
- Verify SQL Server is running
- Check connection strings in appsettings.json
- Ensure API service is running on port 5050

### Build Errors
- Restore NuGet packages: `dotnet restore`
- Clean and rebuild: `dotnet clean && dotnet build`
- Check that both .NET 5 and .NET 8 SDKs are installed

### Login Issues
- Verify user exists in AspNetUsers table
- Check JWT secret key configuration
- Ensure API authentication middleware is configured

## 📄 License

This project is available under the [MIT License](LICENSE) (if applicable - add your license file).

## 👤 Author

**ivek81cro**
- GitHub: [@ivek81cro](https://github.com/ivek81cro)

## 📧 Support

For issues, questions, or contributions, please open an issue on GitHub.

---

## 🎯 Future Enhancements

- [ ] Multi-company support
- [ ] Cloud deployment options
- [ ] Mobile application
- [ ] Advanced reporting with Power BI
- [ ] Electronic invoice integration (e-Račun)
- [ ] Advanced analytics dashboard
- [ ] Multi-language support
- [ ] Export to various accounting standards

---

**Note**: This is an accounting system primarily designed for Croatian accounting standards and regulations (HZZO, JOPPD, etc.). Adaptation may be required for other jurisdictions.
