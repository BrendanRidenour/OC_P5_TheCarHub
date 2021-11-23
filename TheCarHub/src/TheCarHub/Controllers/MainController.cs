using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TheCarHub.Models;
using TheCarHub.Models.Main;

namespace TheCarHub.Controllers
{
    public class MainController : Controller
    {
        [HttpGet(Routes.Inventory)]
        public async Task<IActionResult> Inventory([FromServices]IDealershipService dealership)
        {
            var inventory = await dealership.GetInventory();

            return View(new InventoryViewModel(inventory));
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