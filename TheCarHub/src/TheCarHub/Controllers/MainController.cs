using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TheCarHub.Models;
using TheCarHub.Models.Main;

namespace TheCarHub.Controllers
{
    public class MainController : Controller
    {
        private readonly IDealershipService _dealershipService;

        public MainController(IDealershipService dealershipService)
        {
            this._dealershipService = dealershipService ?? throw new ArgumentNullException(nameof(dealershipService));
        }

        [HttpGet(Routes.Inventory)]
        public async Task<IActionResult> Inventory([FromServices]ISystemClock clock)
        {
            var inventory = await this._dealershipService.GetInventory();

            return View(new InventoryViewModel(inventory, clock));
        }

        [HttpGet(Routes.Error)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static class Routes
        {
            public const string Inventory = "/";
            public const string Error = "/error";
        }
    }
}