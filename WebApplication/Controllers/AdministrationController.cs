using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaintingRepository _paintingRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IArtistRepository _artistRepository;

        public AdministrationController(
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            IOrderRepository orderRepository,
            IPaintingRepository paintingRepository,
            IWebHostEnvironment hostEnvironment,
            IArtistRepository artistRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _orderRepository = orderRepository;
            _paintingRepository = paintingRepository;
            _hostEnvironment = hostEnvironment;
            _artistRepository = artistRepository;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            // Find the role by Role ID
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            // Retrieve all the Users
            foreach (var user in _userManager.Users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

// This action responds to HttpPost and receives EditRoleViewModel
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    return RedirectToAction("EditRole", new {Id = roleId});
                }
            }

            return RedirectToAction("EditRole", new {Id = roleId});
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewData["ErrorMessage"] = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            // GetRolesAsync returns the list of user Roles
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Address = user.Address,
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewData["ErrorMessage"] = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }

            user.Email = model.Email;
            user.UserName = model.UserName;
            user.Address = model.Address;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewData["ErrorMessage"] = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListRoles");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewData["ErrorMessage"] = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult>
            ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new {Id = userId});
        }

        [AllowAnonymous]
        [Authorize(Roles = "Sailor,Admin")] // at least sailor or Admin
        public async Task<IActionResult> ManageOrders()
        {
            var orders = await _orderRepository.GetAll();
            return View(orders);
        }
        

        public async Task<IActionResult> ReviewOrder(int orderId)
        {
            var order = await _orderRepository.GetById(orderId);
            order.IsReviewedBySailor = true;
            await _orderRepository.Update(order);
            return RedirectToAction("ManageOrders");
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
                var imageUniqueName = UploadedFile(newPaintingVM.Image);
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
                Price = painting.Price
            };
            return View(editPaintingViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPainting(EditPaintingViewModel updPaintingVM)
        {
            if (ModelState.IsValid)
            {
                string imageUniqueName = UploadedFile(updPaintingVM.Image);
                var painting = await _paintingRepository.GetById(updPaintingVM.Id);
                painting.Name = updPaintingVM.Name;
                painting.ImageName = imageUniqueName;
                painting.Price = updPaintingVM.Price;
                painting.NumberAvailable = updPaintingVM.NumberAvailable;
                painting.Description.BigDescription = updPaintingVM.Description.BigDescription;
                painting.Description.SmallDescription = updPaintingVM.Description.SmallDescription;

                await _paintingRepository.Update(painting);
                return RedirectToAction("ManagePaintings");
            }

            return View(updPaintingVM);
        }

        //https://www.c-sharpcorner.com/article/upload-and-display-image-in-asp-net-core-3-1/
        private string UploadedFile(IFormFile image)
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
                var imageUniqueName = UploadedFile(newArtistVM.Image);
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
            var imageUniqueName = UploadedFile(updArtistVM.Image);
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