using Microsoft.AspNetCore.Mvc;

namespace LeaderBoard.Controllers
{
    internal class LeaderBoardController(LeaderBoardService leaderBoardService) : Controller
    {
        /// <summary>
        /// Service that reads leaderboard
        /// </summary>
        private LeaderBoardService LeaderBoardService { get; } = leaderBoardService;
        public IActionResult Index()
        {
            return View(LeaderBoardService.LeaderBoardModel);
        }
    }
}
