using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
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
    }
}