using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTO;
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

    public void OnGet()
    {
        CountryStatistics = _statisticsService.GetCountryStatistics();
    }
}
