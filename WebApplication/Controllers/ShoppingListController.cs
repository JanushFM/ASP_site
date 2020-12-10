using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "User")]
    public class ShoppingListController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaintingRepository _paintingRepository;

        public ShoppingListController(
            UserManager<AppUser> userManager,
            IOrderRepository orderRepository,
            IPaintingRepository paintingRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _paintingRepository = paintingRepository;
        }


        [Authorize(Roles = "User")]
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            var totalPrice = await _orderRepository.LoadOrdersWithArtistId(user.Id);

            var shoppingList = new ShoppingListViewModel
            {
                Orders = user.Orders,
                TotalPrice = totalPrice,
                IsUnconfOrdersAvailable = IsUnconfOrdersAvlb(user.Orders)
            };
            return View(shoppingList);
        }

        [HttpGet]
        public async Task<IActionResult> UpdOrder(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);

            var order = await _orderRepository.GetById(user.Id, orderId);
            if (order == null)
            {
                ViewData["ErrorMessage"] = "This page is unavailable.";
                return View("NotFound");
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdOrder(Order updatedOrder)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _orderRepository.GetById(user.Id, updatedOrder.Id);

            if (order == null)
            {
                ViewData["ErrorMessage"] = "This page is unavailable.";
                return View("NotFound");
            }
            
            
            await _paintingRepository.UpdNumPaintings(order.PaintingId,
                updatedOrder.Amount - order.Amount);


            order.Amount = updatedOrder.Amount;
            order.ShippingAddress = updatedOrder.ShippingAddress;
            order.PhoneNumber = updatedOrder.PhoneNumber;

            await _orderRepository.Update(order);
            return RedirectToAction("Orders");
        }
        
        [HttpGet]
        public async Task<IActionResult> DelOrder(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);

            var order = await _orderRepository.GetById(user.Id, orderId);
            if (order == null)
            {
                ViewData["ErrorMessage"] = "This page is unavailable.";
                return View("NotFound");
            }

            return View(order);
        }
        
        [HttpPost]
        public async Task<IActionResult> DelOrder(Order orderToDel)
        {
            var user = await _userManager.GetUserAsync(User);

            var order = await _orderRepository.GetById(user.Id, orderToDel.Id);
            if (order == null)
            {
                ViewData["ErrorMessage"] = "This page is unavailable.";
                return View("NotFound");
            }

            await _paintingRepository.UpdNumPaintings(order.PaintingId, -order.Amount);
            await _orderRepository.Delete(order);
            return RedirectToAction("Orders");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            await _orderRepository.ConfirmOrders(user.Id);
            return RedirectToAction("Orders");
        }

        public bool IsUnconfOrdersAvlb(List<Order> orders)
        {
            return orders.Any(order => !order.IsConfirmedByUser);
        }
    }
}