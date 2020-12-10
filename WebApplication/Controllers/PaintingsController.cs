﻿using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class PaintingsController : Controller
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPaintingRepository _paintingRepository;

        public PaintingsController(
            IArtistRepository artistRepository,
            IOrderRepository orderRepository,
            UserManager<AppUser> userManager,
            IPaintingRepository paintingRepository)
        {
            _artistRepository = artistRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _paintingRepository = paintingRepository;
        }

        public async Task<IActionResult> Index(int? artistId)
        {
            if (artistId == null)
            {
                return NotFound();
            }

            var artist = await _artistRepository.GetById(artistId.Value);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> BuyPainting(int? id)
        {
            if (id == null)
            {
                ViewData["ErrorMessage"] = $"Painting cannot be found";
                return View("NotFound");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ViewData["ErrorMessage"] = $"User cannot be found";
                return View("NotFound");
            }

            if (!user.EmailConfirmed)
            {
                return RedirectToAction("ConfirmationRequired", "Account");
            }

            var orderIdOrZero = await _orderRepository.IsPaintingInOrder(user.Id, id.Value);
            if (orderIdOrZero != 0)
            {
                var order = await _orderRepository.GetById(orderIdOrZero);
                order.Amount += 1;
                await _orderRepository.Update(order);
            }
            else
            {
                await _orderRepository.Add(GetOrder(user, id.Value));
            }
            await _paintingRepository.UpdNumPaintings(id.Value, 1);

            return RedirectToAction("orders", "ShoppingList");
        }

        public Order GetOrder(AppUser user, int paintingId)
        {
            return new Order
            {
                PhoneNumber = user.PhoneNumber,
                AppUserId = user.Id,
                PaintingId = paintingId,
                Amount = 1,
                ShippingAddress = user.Address
            };
        }
    }
}