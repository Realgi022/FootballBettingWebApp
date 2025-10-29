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
        if (HttpContext.Session.GetString("Username") == null)
            return RedirectToAction("Login", "User");

        var matches = await _matchService.GetUpcomingMatchesDtoAsync();
        return View("~/Views/Bet/PlaceBet.cshtml", matches);
    }

    // Post a new bet
    [HttpPost]
    public async Task<IActionResult> PlaceBet(BetDto bet)
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login", "User");
        }

        try
        {
            await _betService.AddBetAsync(bet, username); // pass username to service
        }
        catch (System.Exception ex)
        {
            // Optionally log ex.Message
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Place");
    }
}
