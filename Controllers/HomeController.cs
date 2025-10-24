using BLL.DTOs;
using FootballBettingWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using BLL.Interfaces;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMatchService _matchService;

    public HomeController(ILogger<HomeController> logger, IMatchService matchService)
    {
        _logger = logger;
        _matchService = matchService;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<MatchDto> upcomingMatches = await _matchService.GetUpcomingMatchesDtoAsync();
        return View(upcomingMatches);
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
