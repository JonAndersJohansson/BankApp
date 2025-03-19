using BankAppProject.Profiles;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Customer;
using Services.Profiles;
using Services.Statistics;

namespace BankAppProject;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<BankAppDataContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BankAppDataContext>();
        builder.Services.AddRazorPages();

        builder.Services.AddTransient<DataInitializer>();

        // Lägger till CustomerRepository och CustomerService
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddTransient<ICustomerService, CustomerService>();


        // Lägger till StatisticsRepository och StatisticsService
        builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();
        builder.Services.AddScoped<IStatisticsService, StatisticsService>();

        builder.Services.AddAutoMapper(typeof(CustomersProfile), typeof(CustomerInfoProfile));

        //builder.Services.AddSingleton<IMapper>(new MapperConfiguration(cfg =>
        //{
        //    cfg.AddProfile<CustomersProfile>();  // Lägg till din profil här
        //}).CreateMapper());

        //SQL LOGGING
        builder.Services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
        });

        //builder.Services.AddAutoMapper(typeof(CustomersProfile));


        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            scope.ServiceProvider.GetService<DataInitializer>().SeedData();
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

        app.Run();
    }
}
