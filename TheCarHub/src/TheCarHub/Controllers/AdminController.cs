using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCarHub.Models.Admin;

namespace TheCarHub.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            this._adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
        }

        [AllowAnonymous]
        [HttpGet(Routes.Login)]
        public IActionResult Login() => View();

        [AllowAnonymous]
        [HttpPost(Routes.Login)]
        public async Task<IActionResult> Login([FromForm]string username,
            [FromForm]string password, [FromQuery]string? returnUrl)
        {
            var result = await this._adminService.Login(username, password);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.FailureUserMessage!);

                return View();
            }

            return Redirect(returnUrl ?? Routes.Inventory);
        }

        [HttpPost(Routes.Logout)]
        public async Task<IActionResult> Logout()
        {
            await this._adminService.Logout();

            return Redirect(MainController.Routes.Inventory);
        }

        [HttpGet(Routes.Inventory)]
        public async Task<IActionResult> Inventory()
        {
            var inventory = await this._adminService.GetInventory();

            return View(new InventoryViewModel(inventory));
        }

        [HttpGet(Routes.Create)]
        public IActionResult Create() => View(new CarPoco());

        [HttpPost(Routes.Create)]
        public async Task<IActionResult> Create(CarPoco car)
        {
            if (!ModelState.IsValid)
                return View(car);

            await this._adminService.CreateCar(car);

            return Redirect(Routes.Inventory);
        }

        [HttpGet(Routes.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid id)
        {
            var car = await this._adminService.GetCar(id);

            return car != null ? View(car) : Redirect(Routes.Inventory);
        }

        [HttpPost(Routes.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid id,
            [FromForm]CarPoco car)
        {
            if (!ModelState.IsValid)
                return View(car);

            car.Id = id;

            await this._adminService.UpdateCar(car);

            return Redirect(Routes.Inventory);
        }

        [HttpGet(Routes.DeleteConfirm)]
        public async Task<IActionResult> DeleteConfirm([FromRoute]Guid id)
        {
            var car = await this._adminService.GetCar(id);

            return car != null ? View(car) : Redirect(Routes.Inventory);
        }

        [HttpPost(Routes.Delete)]
        public async Task<IActionResult> Delete([FromForm]Guid id)
        {
            await this._adminService.DeleteCar(id);

            return Redirect(Routes.Inventory);
        }

        public static class Routes
        {
            public const string Login = "/admin/login";
            public const string Logout = "/admin/logout";
            public const string Inventory = "/admin";
            public const string Create = "/admin/add";
            public const string Update = "/admin/update/{id}";
            public const string DeleteConfirm = "/admin/delete/{id}";
            public const string Delete = "/admin/delete";
        }
    }
}