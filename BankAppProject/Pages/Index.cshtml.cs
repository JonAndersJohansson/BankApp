using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Statistics;

namespace BankAppProject.Pages;

public class IndexModel : PageModel
{
    // För att kontrollera onormala händelser men inte i index

    //private readonly ILogger<IndexModel> _logger;

    //public IndexModel(ILogger<IndexModel> logger)
    //{
    //    _logger = logger;
    //}

    public IndexModel(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    private readonly IStatisticsService _statisticsService;

    public List<CountryStatisticsDto> CountryStatistics { get; set; }

    //public void OnGet()
    //{
    //    CountryStatistics = _statisticsService.GetCountryStatistics();
    //}
    //public void OnGet()
    //{
    //    try
    //    {
    //        CountryStatistics = _statisticsService.GetCountryStatistics();

    //        if (CountryStatistics == null || !CountryStatistics.Any())
    //        {
    //            Console.WriteLine(" Ingen statistik hämtad – GetCountryStatistics() returnerade null eller en tom lista.");
    //        }
    //        else
    //        {
    //            Console.WriteLine($" Statistik hämtad: {CountryStatistics.Count} länder.");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($" Fel vid hämtning av statistik: {ex.Message}");
    //        Console.WriteLine($"StackTrace: {ex.StackTrace}");
    //    }
    //}
    public async Task OnGetAsync()
    {
        CountryStatistics = await _statisticsService.GetCountryStatisticsAsync();
    }


}
