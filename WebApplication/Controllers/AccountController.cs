using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailSender _mailSender;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMailSender mailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailSender = mailSender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                // Store user data in AspNetUsers database table
                var result = await _userManager.CreateAsync(user, model.Password);

                
                if (result.Succeeded)
                {
                   
                    
                    if (_userManager.Users.Count() == 1)
                    {
                        MakeUserAdmin(user);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                    
                    var confirmationLink = GetConfirmationLink(user);
                    
                    
                    await _mailSender.SendEmailAsync(model.Email, "Confirm your account",
                        $"Welcome to my painting site ! \nConfirm your account by clicking the link below:\n <a href='{confirmationLink.Result}'>link</a>");
                    
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    
                   
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("index", "Artists");
                }

                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "Artists");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                    (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("index", "Artists");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ViewData["ErrorMessage"] = $"User cannot be found";
                return View("NotFound");
            }
            
            AccountSettingsViewModel accountSettingsViewModel = new AccountSettingsViewModel
            {
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            };
            return View(accountSettingsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(AccountSettingsViewModel accountSettings)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ViewData["ErrorMessage"] = $"User cannot be found";
                return View("NotFound");
            }

            user.Email = accountSettings.Email;
            user.Address = accountSettings.Address;
            user.PhoneNumber = accountSettings.PhoneNumber;
            
            var result = await _userManager.UpdateAsync(user);
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            
            return View(accountSettings);


        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmationRequired()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action
            (
                nameof(ExternalLoginCallback),
                "Account",
                new {ReturnUrl = returnUrl}
            );

            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }


        public async Task<IActionResult>
            ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ??
                        Url.Content("~/"); // if return URL is null then the start route path is returned (home page)

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                    (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            // Get the login information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            // If the user already has a login (i.e if there is a record in AspNetUserLogins
            // table) then sign-in the user with this external login provider
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            // If there is no record in AspNetUserLogins table, the user may not have
            // a local account
            else
            {
                
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new AppUser()
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        
                        await _userManager.CreateAsync(user);
                        if (_userManager.Users.Count() == 1)
                        {
                            MakeUserAdmin(user);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, "User");
                        }
                        var confirmationLink = GetConfirmationLink(user);
                        
                        
                    
                        await _mailSender.SendEmailAsync(user.Email, "Confirm your account",
                            $"Welcome to my painting site ! \nConfirm your account by clicking the link below:\n <a href='{confirmationLink.Result}'>link</a>");
                    }

                    
                    await _userManager.AddLoginAsync(user, info);
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                return View("Error");
            }
        }

        public async Task<string> GetConfirmationLink(AppUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return Url.Action("ConfirmEmail", "Account",
                new { userId = user.Id, token = token }, Request.Scheme);
        }
        
        
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "Artists");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }

        public async void MakeUserAdmin(AppUser user)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        
    }
}