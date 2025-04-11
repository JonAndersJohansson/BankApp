using BankAppTransactionMonitor;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        // Lägg till din connection string här
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<BankAppDataContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.UseLazyLoadingProxies();
        });

        //services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<TransactionMonitor>();

        //ProgressTracker
        services.AddSingleton<ProgressTracker>();

        //ReportWriter
        services.AddSingleton<ReportWriter>();
    })
    .Build();

var runner = host.Services.GetRequiredService<TransactionMonitor>();
await runner.StartAsync();