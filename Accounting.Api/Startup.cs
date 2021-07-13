using Accounting.Api.Data;
using Accounting.DataManager.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace Accounting.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Accounting.Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AccountingConnStr")));
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
                .AddJwtBearer("JwtBearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Secret:SecurityKey"))),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };
                });

            //Data services
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();
            services.AddTransient<ICompanyData, CompanyData>();
            services.AddTransient<IUserData, UserData>();
            services.AddTransient<IPartnersData, PartnersData>();
            services.AddTransient<IEmployeeData, EmployeeData>();
            services.AddTransient<ICityData, CityData>();
            services.AddTransient<ICountyData, CountyData>();
            services.AddTransient<IPayrollData, PayrollData>();
            services.AddTransient<IEmployeeSupplementData, EmployeeSupplementData>();
            services.AddTransient<ISupplementData, SupplementData>();
            services.AddTransient<IPayrollAccountingData, PayrollAccountingData>();
            services.AddTransient<IPayrollArchiveData, PayrollArchiveData>();
            services.AddTransient<IJoppdEmployeeData, JoppdEmployeeData>();
            services.AddTransient<IBookUraPrimkaData, BookUraPrimkaData>();
            services.AddTransient<IBookUraReproData, BookUraReproData>();
            services.AddTransient<IBookUraRestData, BookUraRestData>();
            services.AddTransient<IBookIraData, BookIraData>();
            services.AddTransient<IBookIraRetailData, BookIraRetailData>();
            services.AddTransient<IBookAccountData, BookAccountData>();
            services.AddTransient<IBookAccountSettingsData, BookAccountSettingsData>();
            services.AddTransient<IAccountPairData, AccountPairData>();
            services.AddTransient<IBankReportData, BankReportData>();
            services.AddTransient<IVatArchiveData, VatArchiveData>();
            services.AddTransient<IAssetsData, AssetsData>();
            services.AddTransient<ICashRegisterData, CashRegisterData>();
            services.AddTransient<IAccountingJournalData, AccountingJournalData>();
            services.AddTransient<IBalanceSheetData, BalanceSheetData>();
            services.AddTransient<IBookIraHzzoData, BookIraHzzoData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Accounting.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
