using System.Threading.Tasks;
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

        public PaintingsController(
            IArtistRepository artistRepository,
            IOrderRepository orderRepository,
            UserManager<AppUser> userManager)
        {
            _artistRepository = artistRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
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

            // todo add adding order
            await _orderRepository.Add(GetOrder(user, id.Value));
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