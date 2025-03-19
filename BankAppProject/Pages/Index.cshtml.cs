using DataAccessLayer.DTO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Statistics;

namespace BankAppProject.Pages;

public class IndexModel : PageModel
{

    public IndexModel(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    private readonly IStatisticsService _statisticsService;

    public List<CountryStatisticsDto> CountryStatistics { get; set; }

    public async Task OnGetAsync()
    {
        CountryStatistics = await _statisticsService.GetCountryStatisticsAsync();
    }
}
