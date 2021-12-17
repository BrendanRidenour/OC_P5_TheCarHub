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

            return Redirect(returnUrl ?? Routes.ManageCars);
        }

        [HttpPost(Routes.Logout)]
        public async Task<IActionResult> Logout()
        {
            await this._adminService.Logout();

            return Redirect(MainController.Routes.Inventory);
        }

        [HttpGet(Routes.ManageCars)]
        public async Task<IActionResult> ManageCars()
        {
            var inventory = await this._adminService.GetInventory();

            return View(new ManageCarsViewModel(inventory));
        }

        [HttpGet(Routes.CreateCar)]
        public IActionResult CreateCar() => View(new CarPoco());

        [HttpPost(Routes.CreateCar)]
        public async Task<IActionResult> CreateCar([FromForm]CarPoco car)
        {
            if (!ModelState.IsValid)
                return View(car);

            await this._adminService.CreateCar(car);

            return Redirect(Routes.ManageCars);
        }

        [HttpGet(Routes.UpdateCar)]
        public async Task<IActionResult> UpdateCar([FromRoute]Guid id)
        {
            var car = await this._adminService.GetCar(id);

            return car != null
                ? View(new CarPoco(car))
                : Redirect(Routes.ManageCars);
        }

        [HttpPost(Routes.UpdateCar)]
        public async Task<IActionResult> UpdateCar([FromRoute]Guid id,
            [FromForm]CarPoco car)
        {
            if (!ModelState.IsValid)
                return View(car);

            car.Id = id;

            await this._adminService.UpdateCar(car);

            return Redirect(Routes.ManageCars);
        }

        [HttpGet(Routes.UpdateCar_Pictures)]
        public async Task<IActionResult> UpdateCar_Pictures(
            [FromRoute]Guid id)
        {
            var car = await this._adminService.GetCar(id);

            return car != null
                ? View(new CarPoco(car))
                : Redirect(Routes.ManageCars);
        }

        [HttpPost(Routes.UpdateCar_PicturesAdd)]
        public async Task<IActionResult> UpdateCar_PicturesAdd(
            [FromRoute]Guid id, [FromForm]IFormFile picture)
        {
            if (picture is null)
            {
                return RedirectToAction(nameof(UpdateCar_Pictures), new { id });
            }

            await this._adminService.UpdateCar_AddPicture(id, picture);

            return RedirectToAction(nameof(UpdateCar_Pictures), new { id });
        }

        [HttpPost(Routes.UpdateCar_PicturesDelete)]
        public async Task<IActionResult> UpdateCar_PicturesDelete(
            [FromRoute] Guid id, [FromForm]string pictureUri)
        {
            await this._adminService.UpdateCar_DeletePicture(id, pictureUri);

            return RedirectToAction(nameof(UpdateCar_Pictures), new { id });
        }

        [HttpGet(Routes.DeleteCarConfirm)]
        public async Task<IActionResult> DeleteCarConfirm([FromRoute]Guid id)
        {
            var car = await this._adminService.GetCar(id);

            return car != null ? View(car) : Redirect(Routes.ManageCars);
        }

        [HttpPost(Routes.DeleteCar)]
        public async Task<IActionResult> DeleteCar([FromForm]Guid id)
        {
            await this._adminService.DeleteCar(id);

            return Redirect(Routes.ManageCars);
        }

        public static class Routes
        {
            public const string Login = "/admin/login";
            public const string Logout = "/admin/logout";
            public const string ManageCars = "/admin";
            public const string CreateCar = "/admin/add";
            public const string UpdateCar = "/admin/update/{id}";
            public const string UpdateCar_Pictures = "/admin/update/{id}/pictures";
            public const string UpdateCar_PicturesAdd = "/admin/update/{id}/pictures/add";
            public const string UpdateCar_PicturesDelete = "/admin/update/{id}/pictures/delete";
            public const string DeleteCarConfirm = "/admin/delete/{id}";
            public const string DeleteCar = "/admin/delete";
        }
    }
}