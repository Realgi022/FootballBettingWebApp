using BLL.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BLL.Interfaces;

public class MatchController : Controller
{
    private readonly IMatchService _matchService;

    public MatchController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<MatchDto> matches = await _matchService.GetUpcomingMatchesDtoAsync();
        return View("~/Views/Match.cshtml", matches);
    }
}
