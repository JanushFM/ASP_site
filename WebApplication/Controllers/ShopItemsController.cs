using System;
using System.IO;
using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplication.Helpers;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShopItemsController : Controller
    {
        private readonly IPaintingRepository _paintingRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly AzureStorageConfig _storageConfig;

        public ShopItemsController(
            IPaintingRepository paintingRepository,
            IArtistRepository artistRepository,
            IOptions<AzureStorageConfig> storageConfig)
        {
            _paintingRepository = paintingRepository;
            _artistRepository = artistRepository;
            _storageConfig = storageConfig.Value;
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
            if (ModelState.IsValid)
            {
                var newPainting = new Painting
                {
                    ArtistId = newPaintingVM.SelectedArtistId,
                    Description = newPaintingVM.Description,
                    Name = newPaintingVM.Name,
                    Price = newPaintingVM.Price,
                    NumberAvailable = newPaintingVM.NumberAvailable,
                };
                
                var blobUriResults = await LoadImageToAzure(newPaintingVM.Image);
                if (blobUriResults != null)
                {
                    newPainting.ImageName = newPaintingVM.Image.Name;
                    newPainting.ImageUri = blobUriResults.Item1;
                    newPainting.ThumbnailUri = blobUriResults.Item2;
                }
                
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

                var blobUriResults = await LoadImageToAzure(updPaintingVM.Image);
                if (blobUriResults != null)
                {
                    painting.ImageName = updPaintingVM.Image.Name;
                    painting.ImageUri = blobUriResults.Item1;
                    painting.ThumbnailUri = blobUriResults.Item2;
                }

                await _paintingRepository.Update(painting);
                return RedirectToAction("ManagePaintings");
            }

            return View(updPaintingVM);
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
        public IActionResult CreateArtist()
        {
            var artistVM = new EditArtistViewModel()
            {
                Description = new Description(),
            };

            return View(artistVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtist(EditArtistViewModel newArtistVM)
        {
            if (ModelState.IsValid)
            {
                var newArtist = new Artist
                {
                    Name = newArtistVM.Name,
                    Quote = newArtistVM.Quote,
                    Description = newArtistVM.Description,
                };
                
                var blobUriResults = await LoadImageToAzure(newArtistVM.Image);
                if (blobUriResults != null)
                {
                    newArtist.ImageName = newArtistVM.Image.Name;
                    newArtist.ImageUri = blobUriResults.Item1;
                    newArtist.ThumbnailUri = blobUriResults.Item2;
                }

                await _artistRepository.Add(newArtist);
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
            if (ModelState.IsValid)
            {
                var artist = await _artistRepository.GetById(updArtistVM.Id);
                artist.Name = updArtistVM.Name;
                artist.Quote = updArtistVM.Quote;
                artist.Description.BigDescription = updArtistVM.Description.BigDescription;
                artist.Description.SmallDescription = updArtistVM.Description.SmallDescription;

                
                var blobUriResults = await LoadImageToAzure(updArtistVM.Image);
                if (blobUriResults != null)
                {
                    artist.ImageName = updArtistVM.Image.Name;
                    artist.ImageUri = blobUriResults.Item1;
                    artist.ThumbnailUri = blobUriResults.Item2;
                }
                
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

        public async Task<Tuple<string, string>> LoadImageToAzure(IFormFile file)
        {
            if (StorageHelper.IsImage(file) && file.Length > 0)
            {
                await using Stream stream = file.OpenReadStream();
                return await StorageHelper.MyUploadFileToStorage(stream, file.FileName, _storageConfig);
            }
            return null;
        }
    }
}