using DAL;
using FootballBettingWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DatabaseConnection _dbConnection;

    public HomeController(ILogger<HomeController> logger, DatabaseConnection dbConnection)
    {
        _logger = logger;
        _dbConnection = dbConnection;
    }
    public IActionResult Index()
    {
        ViewBag.UpcomingMatch = "Real Madrid vs Barcelona";
        return View();
    }

    public IActionResult TestDb()
    {
        bool success = _dbConnection.TestConnection();
        if (success)
            return Content(" Database connection successful!");
        else
            return Content(" Database connection failed! Check console for details.");
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
