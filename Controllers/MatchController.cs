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

}
