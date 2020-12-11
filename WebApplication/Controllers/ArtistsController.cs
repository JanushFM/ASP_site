using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistRepository _artistRepository;

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
    }
}