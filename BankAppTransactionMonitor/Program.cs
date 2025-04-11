using BankAppTransactionMonitor;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.AccountRepositories;
using DataAccessLayer.Repositories.CustomerRepositories;
using DataAccessLayer.Repositories.CustomerrRepositories;
using DataAccessLayer.Repositories.DispositionRepositories;
using DataAccessLayer.Repositories.TransactionRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        // Connectionstring
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

        //services.AddDbContext<BankAppDataContext>(options =>
        //{
        //    options.UseSqlServer(connectionString);
        //    options.UseLazyLoadingProxies();
        //});

        services.AddDbContext<BankAppDataContext>(options =>
        {
            options.UseSqlServer(connectionString)
                .LogTo(Console.WriteLine,
                    (eventId, logLevel) =>
                        logLevel == LogLevel.Warning ||
                        logLevel == LogLevel.Error ||
                        logLevel == LogLevel.Critical)
                .EnableSensitiveDataLogging(false);

            options.UseLazyLoadingProxies();
        });

        // Automapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Services
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ITransactionService, TransactionService>();


        services.AddScoped<TransactionMonitor>();

        //repo
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IDispositionRepository, DispositionRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        //ProgressTracker
        services.AddSingleton<ProgressTracker>();

        //ReportWriter
        services.AddSingleton<ReportWriter>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Warning);
    })
    .Build();

var runner = host.Services.GetRequiredService<TransactionMonitor>();
await runner.StartAsync();