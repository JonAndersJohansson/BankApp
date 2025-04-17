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

namespace BankAppProject;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Program started...");

        var builder = WebApplication.CreateBuilder(args);

        Console.WriteLine("Builder created...");

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        Console.WriteLine("Connection string: " + connectionString);

        builder.Services.AddDbContext<BankAppDataContext>(options =>
        {
            Console.WriteLine("Configuring DbContext...");
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly("DataAccessLayer");
            });
        });
        //builder.Services.AddDbContext<BankAppDataContext>(options =>
        //    options.UseSqlServer(
        //        builder.Configuration.GetConnectionString("DefaultConnection"),
        //        sqlOptions => sqlOptions.MigrationsAssembly("DataAccessLayer"))); // Viktigt!

        Console.WriteLine("Registering Identity...");

        builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false) //Ändrat ifrån True
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BankAppDataContext>();

        Console.WriteLine("Registering RazorPages");

        builder.Services.AddRazorPages();

        Console.WriteLine("Registering DataInitializer");

        builder.Services.AddTransient<DataInitializer>();


        Console.WriteLine("Registering Repositories");
        // Repositories
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IDispositionRepository, DispositionRepository>();
        builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        Console.WriteLine("Registering Services");
        // Services
        builder.Services.AddTransient<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IStatisticsService, StatisticsService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();

        Console.WriteLine("Registering AutoMapper");
        //AutoMapper
        builder.Services.AddAutoMapper(
            typeof(CustomerProfiles).Assembly,
            typeof(AccountProfiles).Assembly,
            typeof(StatisticsProfiles).Assembly,
            typeof(UserProfiles).Assembly,
            typeof(CustomerServiceProfiles).Assembly,
            typeof(AccountServiceProfiles).Assembly
        );

        Console.WriteLine("Add SQL LOGGING");
        //SQL LOGGING
        builder.Services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
        });
        Console.WriteLine("Add Response caching");
        // Response caching
        builder.Services.AddResponseCaching();

        Console.WriteLine("Try build");

        var app = builder.Build();

        Console.WriteLine("App built...");

        //Behövs för Azure!
        //using (var scope = app.Services.CreateScope())
        //{
        //    var dbContext = scope.ServiceProvider.
        //         GetRequiredService<BankAppDataContext>();
        //    if (dbContext.Database.IsRelational())
        //    {
        //        dbContext.Database.Migrate();
        //    }
        //}

        //Seed data and migrate
        //using (var scope = app.Services.CreateScope())
        //{
        //    var dbContext = scope.ServiceProvider.GetRequiredService<BankAppDataContext>();
        //    dbContext.Database.Migrate();

        //    scope.ServiceProvider.GetRequiredService<DataInitializer>().SeedData();
        //}

        // Endast Migrate om det är relational (för Azure)
        try
        {
            Console.WriteLine("🌱 Startar appens setup...");

            using (var scope = app.Services.CreateScope())
            {
                Console.WriteLine("🔄 Hämtar BankAppDataContext...");
                var dbContext = scope.ServiceProvider.GetRequiredService<BankAppDataContext>();

                Console.WriteLine("🔍 Kollar om databasen är relational...");
                if (dbContext.Database.IsRelational())
                {
                    Console.WriteLine("🧱 Kör migration...");
                    dbContext.Database.Migrate();
                    Console.WriteLine("✅ Migration klar.");
                }

                Console.WriteLine("🧪 Hämtar DataInitializer...");
                var dataInitializer = scope.ServiceProvider.GetRequiredService<DataInitializer>();

                Console.WriteLine("🚀 Kör SeedData...");
                dataInitializer.SeedData();
                Console.WriteLine("✅ Seeding klart.");
            }

            Console.WriteLine("🎉 Appen är färdiginitierad.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Startup error:");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            throw;
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
