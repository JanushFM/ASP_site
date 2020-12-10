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
        private readonly IOrderRepository _orderRepository;

        public ArtistsController(IArtistRepository artistRepository,
            UserManager<AppUser> userManager,
            IOrderRepository orderRepository)
        {
            _artistRepository = artistRepository;
            _userManager = userManager;
            _orderRepository = orderRepository;
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
    }
}