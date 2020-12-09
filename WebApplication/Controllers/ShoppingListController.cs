using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class ShoppingListController : Controller
    {
        // GET
        [Authorize(Roles = "User")]
        public IActionResult Orders()
        {
            return View();
        }
    }
}