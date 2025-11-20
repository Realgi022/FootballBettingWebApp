using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
            return RedirectToAction("Login", "User");

        try
        {
            // Place the bet
            await _betService.AddBetAsync(bet, username);
            return RedirectToAction("Receipt");
        }
        catch (System.Exception ex)
        {
            TempData["Error"] = ex.Message;

            // Reload matches and wallet for view
            var matches = await _matchService.GetUpcomingMatchesDtoAsync();
            var wallet = await _walletService.GetWalletAsync(username);
            ViewBag.Balance = wallet?.Balance ?? 0;

            return View("~/Views/Bet/PlaceBet.cshtml", matches);
        }
    }

    // Show user bets / receipt page
    public async Task<IActionResult> Receipt()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
            return RedirectToAction("Login", "User");

        var allBets = await _betService.GetAllBetsAsync(username);

        // Only show bets that are pending or won (not yet claimed)
        var userBets = allBets
            .Where(b => b.Username == username && (b.Status == "Pending" || b.Status == "Won"));

        var wallet = await _walletService.GetWalletAsync(username);
        ViewBag.Balance = wallet?.Balance ?? 0;

        return View("~/Views/Bet/Receipt.cshtml", userBets);
    }


    // Claim a winning bet
    [HttpPost]
    public async Task<IActionResult> Claim(int betId)
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
            return RedirectToAction("Login", "User");

        var allBets = await _betService.GetAllBetsAsync(username);
        var bet = allBets.FirstOrDefault(b => b.BetId == betId && b.Username == username);

        if (bet == null)
            return NotFound();

        if (bet.Status != "Won")
            return BadRequest("Bet cannot be claimed.");

        // Update wallet balance
        var wallet = await _walletService.GetWalletAsync(username);
        if (wallet == null)
            return BadRequest("Wallet not found.");

        wallet.Balance += bet.Stake * bet.Odds;
        await _walletService.UpdateWalletAsync(wallet);

        // Mark bet as claimed
        bet.Status = "Claimed";
        await _betService.UpdateBetAsync(bet);

        return RedirectToAction("Receipt");
    }
}
