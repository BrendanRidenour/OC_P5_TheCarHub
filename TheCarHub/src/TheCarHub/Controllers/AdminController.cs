using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TheCarHub.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public static class Routes
        {
            public const string Login = "/admin/login";
            public const string Inventory = "/admin";
            public const string Create = "/admin/add";
            public const string Update = "/admin/update/{id}";
            public const string Remove = "/admin/remove/{id}";
        }
    }
}