using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using System.Threading.Tasks;

public class WalletController : Controller
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    // Display wallet page
    public async Task<IActionResult> Index()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
            return RedirectToAction("Login", "User");

        var wallet = await _walletService.GetWalletAsync(username);

        // Determine if user can claim
        wallet.CanClaim = !wallet.LastClaimed.HasValue ||
                          (DateTime.UtcNow - wallet.LastClaimed.Value).TotalHours >= 24;

        return View("~/Views/Wallet/UserWallet.cshtml", wallet);
    }

    // Claim daily money
    [HttpPost]
    public async Task<IActionResult> Claim()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
            return RedirectToAction("Login", "User");

        bool claimed = await _walletService.ClaimDailyRewardAsync(username);

        TempData["Message"] = claimed
            ? "You’ve claimed your free daily 100 coins! 💰"
            : "You can only claim once every 24 hours.";

        return RedirectToAction("Index");
    }
}
