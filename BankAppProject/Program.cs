using BankAppProject.Profiles;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.AccountRepositories;
using DataAccessLayer.Repositories.CustomerRepositories;
using DataAccessLayer.Repositories.CustomerrRepositories;
using DataAccessLayer.Repositories.DispositionRepositories;
using DataAccessLayer.Repositories.StatisticsRepositories;
using DataAccessLayer.Repositories.TransactionRepositories;
using DataAccessLayer.Repositories.UserRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Infrastructure.Profiles;
//using AutoMapper;
//using AutoMapper.Extensions.Microsoft.DependencyInjection;

namespace BankAppProject;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<BankAppDataContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly("DataAccessLayer"))); // Viktigt!


        builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false) //Ändrat ifrån True
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BankAppDataContext>();
        builder.Services.AddRazorPages();

        builder.Services.AddTransient<DataInitializer>();

        // Repositories
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IDispositionRepository, DispositionRepository>();
        builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        // Services
        builder.Services.AddTransient<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IStatisticsService, StatisticsService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();

        //AutoMapper
        //builder.Services.AddAutoMapper(
        //    typeof(CustomerProfiles),
        //    typeof(AccountProfiles),
        //    typeof(StatisticsProfiles),
        //    typeof(UserProfiles),
        //    typeof(CustomerServiceProfiles),
        //    typeof(AccountServiceProfiles));

        builder.Services.AddAutoMapper(
            typeof(CustomerProfiles).Assembly,
            typeof(AccountProfiles).Assembly,
            typeof(StatisticsProfiles).Assembly,
            typeof(UserProfiles).Assembly,
            typeof(CustomerServiceProfiles).Assembly,
            typeof(AccountServiceProfiles).Assembly
        );



        //SQL LOGGING
        builder.Services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
        });

        // Response caching
        builder.Services.AddResponseCaching();

        var app = builder.Build();

        //Seed data and migrate
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BankAppDataContext>();
            dbContext.Database.Migrate();

            scope.ServiceProvider.GetRequiredService<DataInitializer>().SeedData();
        }


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        // Response caching
        app.UseResponseCaching();

        app.Run();
    }
}
