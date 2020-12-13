using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShopItemsController : Controller
    {
        private readonly IPaintingRepository _paintingRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IArtistRepository _artistRepository;

        public ShopItemsController(
            IPaintingRepository paintingRepository,
            IWebHostEnvironment hostEnvironment,
            IArtistRepository artistRepository)
        {
            _paintingRepository = paintingRepository;
            _hostEnvironment = hostEnvironment;
            _artistRepository = artistRepository;
        }
        
        
        public async Task<IActionResult> ManagePaintings()
        {
            var paintings = await _paintingRepository.GetAll();
            return View(paintings);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePainting()
        {
            var newPainting = new CreatePaintingViewModel
            {
                Artists = await _artistRepository.GetAll(),
                Description = new Description()
            };

            return View(newPainting);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePainting(CreatePaintingViewModel newPaintingVM)
        {
            // var errors = ModelState
            //     .Where(x => x.Value.Errors.Count > 0)
            //     .Select(x => new {x.Key, x.Value.Errors})
            //     .ToArray();
            
            if (ModelState.IsValid)
            {
                var imageUniqueName = CreteUniqueImageName(newPaintingVM.Image);
                var newPainting = new Painting
                {
                    ArtistId = newPaintingVM.SelectedArtistId,
                    Description = newPaintingVM.Description,
                    Name = newPaintingVM.Name,
                    Price = newPaintingVM.Price,
                    NumberAvailable = newPaintingVM.NumberAvailable,
                    ImageName = imageUniqueName
                };
                await _paintingRepository.Add(newPainting);
                return RedirectToAction("ManagePaintings");
            }

            newPaintingVM.Artists = await _artistRepository.GetAll();

            return View(newPaintingVM);
        }


        [HttpGet]
        public async Task<IActionResult> EditPainting(int id)
        {
            var painting = await _paintingRepository.GetById(id);
            var editPaintingViewModel = new EditPaintingViewModel
            {
                Id = painting.Id,
                Description = painting.Description,
                Name = painting.Name,
                NumberAvailable = painting.NumberAvailable,
                Price = painting.Price,
                PrevImageName = painting.ImageName
            };
            return View(editPaintingViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPainting(EditPaintingViewModel updPaintingVM)
        {
            if (ModelState.IsValid)
            {
                var painting = await _paintingRepository.GetById(updPaintingVM.Id);
                painting.Name = updPaintingVM.Name;
                painting.Price = updPaintingVM.Price;
                painting.NumberAvailable = updPaintingVM.NumberAvailable;
                painting.Description.BigDescription = updPaintingVM.Description.BigDescription;
                painting.Description.SmallDescription = updPaintingVM.Description.SmallDescription;
                
                var imageUniqueName = CreteUniqueImageName(updPaintingVM.Image);
                
                if (imageUniqueName != null)
                {
                    painting.ImageName = imageUniqueName;
                }
                
                await _paintingRepository.Update(painting);
                return RedirectToAction("ManagePaintings");
            }

            return View(updPaintingVM);
        }

        //https://www.c-sharpcorner.com/article/upload-and-display-image-in-asp-net-core-3-1/
        private string CreteUniqueImageName(IFormFile image)
        {
            string uniqueFileName = null;

            if (image != null)
            {
                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid() + "_" + image.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                image.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        [HttpPost]
        public async Task<IActionResult> DeletePainting(int id)
        {
            var painting = await _paintingRepository.GetById(id);
            await _paintingRepository.Delete(painting);
            return RedirectToAction("ManagePaintings");
        }
        
        [HttpGet]
        public async Task<IActionResult> ManageArtists()
        {
            var artists = await _artistRepository.GetAll();
            return View(artists);
        }

        [HttpGet]
        public  IActionResult CreateArtist()
        {
            var artistVM = new EditArtistViewModel()
            {
                Description = new Description()
            };

            return View(artistVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtist(EditArtistViewModel newArtistVM)
        {
            if (ModelState.IsValid)
            {
                var imageUniqueName = CreteUniqueImageName(newArtistVM.Image);
                var artist = new Artist
                {
                    Name = newArtistVM.Name,
                    ImageName = imageUniqueName,
                    Quote = newArtistVM.Quote,
                    Description = newArtistVM.Description
                };

                await _artistRepository.Add(artist);
                return RedirectToAction("ManageArtists");
            }

            return View(newArtistVM);
        }

        [HttpGet]
        public async Task<IActionResult> EditArtist(int id)
        {
            var artist = await _artistRepository.GetById(id);
            var editArtistViewModel = new EditArtistViewModel
            {
                Id = artist.Id,
                Name = artist.Name,
                Description = artist.Description,
                Quote = artist.Quote,
            };
            return View(editArtistViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditArtist(EditArtistViewModel updArtistVM)
        {
            var imageUniqueName = CreteUniqueImageName(updArtistVM.Image);
            if (ModelState.IsValid)
            {
                var artist = await _artistRepository.GetById(updArtistVM.Id);
                artist.Name = updArtistVM.Name;
                artist.ImageName = imageUniqueName;
                artist.Quote = updArtistVM.Quote;
                artist.Description.BigDescription = updArtistVM.Description.BigDescription;
                artist.Description.SmallDescription = updArtistVM.Description.SmallDescription;

                await _artistRepository.Update(artist);
                return RedirectToAction("ManageArtists");
            }

            return View(updArtistVM);
        }

        public async Task<IActionResult> DeleteArtist(int id)
        {
            var artist = await _artistRepository.GetById(id);
            await _artistRepository.Delete(artist);
            return RedirectToAction("ManageArtists");
        }

        public List<bool> CreateListOfFalse(int capacity)
        {
            var list = new List<bool>();
            for (var i = 0; i < capacity; i++)
            {
                list.Add(false);
            }

            return list;
        }
    }
}