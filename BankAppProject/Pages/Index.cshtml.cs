using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace BankAppProject.Pages;

public class IndexModel : PageModel
{
    private readonly IStatisticsService _statisticsService;
    private readonly IMapper _mapper;

    public IndexModel(IStatisticsService statisticsService, IMapper mapper)
    {
        _statisticsService = statisticsService;
        _mapper = mapper;
    }

    public List<CountryStatisticsViewModel> CountryStatistics { get; set; }

    public async Task OnGetAsync()
    {
        var dtos = await _statisticsService.GetCountryStatisticsAsync();
        CountryStatistics = _mapper.Map<List<CountryStatisticsViewModel>>(dtos);
    }
}
