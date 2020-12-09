using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistRepository _artistRepository;
        private readonly UserManager<AppUser> _userManager;

        public ArtistsController(IArtistRepository artistRepository,
            UserManager<AppUser> userManager)
        {
            _artistRepository = artistRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _artistRepository.GetAll());
        }

        public async Task<IActionResult> Biography(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _artistRepository.GetById(id.Value);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        public async Task<IActionResult> Paintings(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _artistRepository.GetById(id.Value);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        [Authorize]
        public async Task<IActionResult> BuyPainting(int? id)
        {
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
            return RedirectToAction("orders", "ShoppingList");
        }
    }
}