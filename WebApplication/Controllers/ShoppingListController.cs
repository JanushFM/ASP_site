using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebApplication.Hubs;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "User")]
    public class ShoppingListController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaintingRepository _paintingRepository;
        private readonly IHubContext<OrderHub> _orderHub;

        public ShoppingListController(
            UserManager<AppUser> userManager,
            IOrderRepository orderRepository,
            IPaintingRepository paintingRepository,
            IHubContext<OrderHub> orderHub)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _paintingRepository = paintingRepository;
            _orderHub = orderHub;
        }


        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            var totalPrice = await _orderRepository.LoadOrdersWithArtistId(user.Id);

            ShoppingListViewModel shoppingList;
            if (user.Orders == null)
            {
                shoppingList = new ShoppingListViewModel
                {
                    Orders = new List<Order>(),
                    TotalPrice = totalPrice,
                    IsUnconfOrdersAvailable = false
                };
            }
            else
            {
                shoppingList = new ShoppingListViewModel
                {
                    Orders = user.Orders,
                    TotalPrice = totalPrice,
                    IsUnconfOrdersAvailable = IsUnconfOrdersAvlb(user.Orders)
                };
            }

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
            
            if (order.IsConfirmedByUser)
            {
                ViewData["ErrorMessage"] = "You have already confirmed this order. Our assistant will call you soon !";
                return View("NotFound");
            }

            var orderViewModel = new UpdOrderViewModel
            {
                Id = order.Id,
                Amount = order.Amount,
                Painting = order.Painting,
                PhoneNumber = order.PhoneNumber,
                ShippingAddress = order.ShippingAddress,
                MaxAmount = order.Painting.NumberAvailable + order.Amount
            };

            return View(orderViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdOrder(UpdOrderViewModel updatedOrder)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _orderRepository.GetById(user.Id, updatedOrder.Id);

            if (order == null)
            {
                ViewData["ErrorMessage"] = "This page is unavailable.";
                return View("NotFound");
            }
            if (order.IsConfirmedByUser)
            {
                ViewData["ErrorMessage"] = "You have already confirmed this order. Our assistant will call you soon !";
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
            
            if (order.IsConfirmedByUser)
            {
                ViewData["ErrorMessage"] = "You have already confirmed this order. Our assistant will call you soon !";
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
            if (order.IsConfirmedByUser)
            {
                ViewData["ErrorMessage"] = "You have already confirmed this order. Our assistant will call you soon !";
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
            await _orderRepository.LoadOrdersWithArtistId(user.Id);

            if (!await _orderRepository.IsPhoneNumberAssignedInOrders(user.Id))
            {
                ViewData["ErrorMessage"] = "You have to assign phone number before confirming order !";
                return View("NotFound");
            }

            var ordersToConfirm = GetListOrdersToConfirm(user.Orders);
            var jsonOrdersToConfirm = JsonConvert.SerializeObject(ordersToConfirm);

            await _orderHub.Clients.All.SendAsync("ReceiveMessage", jsonOrdersToConfirm);

            await _orderRepository.ConfirmOrders(ordersToConfirm);
            return RedirectToAction("Orders");
        }

        public bool IsUnconfOrdersAvlb(List<Order> orders)
        {
            return orders.Any(order => !order.IsConfirmedByUser);
        }


        public List<Order> GetListOrdersToConfirm(List<Order> orders)
        {
            return orders.Where(order => !order.IsConfirmedByUser).ToList();
            // все заказы, что ещё не
            // подтверждены должны быть подтвержденны
        }
    }
}