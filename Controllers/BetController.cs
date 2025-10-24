using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Interfaces;

public class BetController : Controller
{
    private readonly IBetService _betService;
    private readonly IMatchService _matchService;

    public BetController(IBetService betService, IMatchService matchService)
    {
        _betService = betService;
        _matchService = matchService;
    }

    // Show page with upcoming matches to place bets
    public async Task<IActionResult> Place()
    {
        var matches = await _matchService.GetUpcomingMatchesDtoAsync();
        return View("~/Views/Bet/PlaceBet.cshtml", matches);
    }

    // Post a new bet
    [HttpPost]
    public async Task<IActionResult> PlaceBet(BetDto bet)
    {
        await _betService.AddBetAsync(bet);
        return RedirectToAction("Place");
    }
}
