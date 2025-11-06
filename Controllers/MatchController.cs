using BLL.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BLL.Interfaces;

public class MatchController : Controller
{
    private readonly IMatchService _matchService;
    private readonly IWalletService _walletService;
    public MatchController(IMatchService matchService, IWalletService walletService)
    {
        _matchService = matchService;
        _walletService = walletService;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Username") == null)
            return RedirectToAction("Login", "User");

        var username = HttpContext.Session.GetString("Username");

        var wallet = await _walletService.GetWalletAsync(username);
        ViewBag.Balance = wallet?.Balance ?? 0;

        IEnumerable<MatchDto> matches = await _matchService.GetUpcomingMatchesDtoAsync();
        return View("~/Views/Match.cshtml", matches);
    }
}
