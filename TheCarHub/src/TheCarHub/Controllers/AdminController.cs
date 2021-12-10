using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

        [HttpGet(Routes.CreateCar)]
        public IActionResult CreateCar() => View(new CarPoco());

        [HttpPost(Routes.CreateCar)]
        public async Task<IActionResult> CreateCar([FromForm]CarPoco car,
            [FromForm]IFormFile? picture)
        {
            if (!ModelState.IsValid)
                return View(car);

            await this._adminService.CreateCar(car, picture);

            return Redirect(Routes.Inventory);
        }

        [HttpGet(Routes.UpdateCar)]
        public async Task<IActionResult> UpdateCar([FromRoute]Guid id)
        {
            var car = await this._adminService.GetCar(id);

            return car != null
                ? View(car)
                : Redirect(Routes.Inventory);
        }

        [HttpPost(Routes.UpdateCar)]
        public async Task<IActionResult> UpdateCar([FromRoute]Guid id,
            [FromForm]CarPoco car, [FromForm]IFormFile? picture)
        {
            if (!ModelState.IsValid)
                return View(car);

            car.Id = id;

            await this._adminService.UpdateCar(car, picture);

            return Redirect(Routes.Inventory);
        }

        [HttpGet(Routes.DeleteCarConfirm)]
        public async Task<IActionResult> DeleteCarConfirm([FromRoute]Guid id)
        {
            var car = await this._adminService.GetCar(id);

            return car != null ? View(car) : Redirect(Routes.Inventory);
        }

        [HttpPost(Routes.DeleteCar)]
        public async Task<IActionResult> DeleteCar([FromForm]Guid id)
        {
            await this._adminService.DeleteCar(id);

            return Redirect(Routes.Inventory);
        }

        public static class Routes
        {
            public const string Login = "/admin/login";
            public const string Logout = "/admin/logout";
            public const string Inventory = "/admin";
            public const string CreateCar = "/admin/add";
            public const string UpdateCar = "/admin/update/{id}";
            public const string DeleteCarConfirm = "/admin/delete/{id}";
            public const string DeleteCar = "/admin/delete";
        }
    }
}