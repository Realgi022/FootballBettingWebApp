using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FootballBettingWebApp.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchRepository _matchRepository;

        public MatchController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IActionResult> Index()
        {
            var matches = await _matchRepository.GetUpcomingAsync();
            return View("~/Views/Match.cshtml", matches); 
        }
    }
}
