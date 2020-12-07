using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistRepository _artistRepository;
        // GET
        public ArtistsController(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
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
        public IActionResult BuyPainting(int ?id)
        {
            // todo add adding order 
            return RedirectToAction("orders", "ShoppingList");
        }
    }
}