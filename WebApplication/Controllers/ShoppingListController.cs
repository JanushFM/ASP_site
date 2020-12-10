using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class ShoppingListController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderRepository _orderRepository;

        public ShoppingListController(
            UserManager<AppUser> userManager,
            IOrderRepository orderRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            var totalPrice = await _orderRepository.LoadOrdersWithArtistId(user.Id);

            var shoppingList = new ShoppingListViewModel
            {
                Orders = user.Orders,
                TotalPrice = totalPrice
            };
            return View(shoppingList);
        }
    }
}