using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MatchController : Controller
{
    private readonly IMatchService _matchService;
    private readonly IWalletService _walletService;
    private readonly IBetService _betService;

    public MatchController(IMatchService matchService, IWalletService walletService, IBetService betService)
    {
        _matchService = matchService;
        _walletService = walletService;
        _betService = betService;
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

    [HttpGet]
    public IActionResult Create()
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return Unauthorized();

        return View("~/Views/AdminActions/Create.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Create(MatchDto matchDto)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return Unauthorized();

        if (!ModelState.IsValid)
            return View("~/Views/AdminActions/Create.cshtml", matchDto);

        await _matchService.CreateMatchAsync(matchDto);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> EditResult(int matchId)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return Unauthorized();

        var match = await _matchService.GetMatchByIdDtoAsync(matchId);
        if (match == null) return NotFound();

        return View("~/Views/AdminActions/EditResult.cshtml", match);
    }


    [HttpPost]
    public async Task<IActionResult> EditResult(int matchId, string result)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return Unauthorized();

        var match = await _matchService.GetMatchByIdDtoAsync(matchId);
        if (match == null) return NotFound();

        match.Result = result;
        await _matchService.UpdateMatchResult(match);
        await _betService.ResolveBetsAsync(match);

        return RedirectToAction("Index");
    }



}
