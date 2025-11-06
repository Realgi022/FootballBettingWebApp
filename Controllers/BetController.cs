using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class BetController : Controller
{
    private readonly IBetService _betService;
    private readonly IMatchService _matchService;
    private readonly IWalletService _walletService;


    public BetController(IBetService betService, IMatchService matchService, IWalletService walletService)
    {
        _betService = betService;
        _matchService = matchService;
        _walletService = walletService;
    }

    // Show page with upcoming matches to place bets
    public async Task<IActionResult> Place()
    {
        if (HttpContext.Session.GetString("Username") == null)
            return RedirectToAction("Login", "User");

        var username = HttpContext.Session.GetString("Username");

        var wallet = await _walletService.GetWalletAsync(username);
        ViewBag.Balance = wallet?.Balance ?? 0;

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
