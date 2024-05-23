using GetFit.ActionFilter;
using GetFit.Context;
using GetFit.Models.Auth;
using GetFit.Utility;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Notyf;
using GetFit.Data;

namespace GetFit.Controllers;

public class AuthController(
UserManager<IdentityUser> userManager,
SignInManager<IdentityUser> signInManager,
INotyfService notyf,
GFContext gfContext,
IHttpContextAccessor httpContextAccessor) : Controller
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly INotyfService _notyfService = notyf;
    private readonly GFContext _gfContext = gfContext;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    
    [RedirectAuthenticatedUsers]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Username) ?? await _userManager.FindByEmailAsync(model.Username);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var userDetails = await Helper.GetCurrentUserIdAsync(_httpContextAccessor, _userManager);
                    var memberDetail = await _gfContext.MemberDetails.AnyAsync(x => x.UserId == userDetails.userId);

                    var redirectResult = memberDetail ? RedirectToAction("Index", "Member") : RedirectToAction("MemberRegistration", "Member");

                    _notyfService.Success("Login succesful");
                    return redirectResult;
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            _notyfService.Error("Invalid login attempt");
            return View(model);
        }
        return View(model);
    }

/*
    [RedirectAuthenticatedUsers]
    public IActionResult AdminLogin()
    {
        return View();
    }*/

    /*[HttpPost]
    public async Task<IActionResult> AdminLogin(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Username) ?? await _userManager.FindByEmailAsync(model.Username);
            
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var userDetails = await Helper.GetCurrentUserIdAsync(_httpContextAccessor, _userManager);
                    var trainer = await _gfContext.MemberDetails.AnyAsync(x => x.UserId == userDetails.userId);

                    var redirectResult = trainer ? RedirectToAction("AdminIndex", "Member") : RedirectToAction("AdminIndex", "Member");

                    _notyfService.Success("Login succesful");
                    return redirectResult;
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            _notyfService.Error("Invalid login attempt");
            return View(model);
        }
        return View(model);
    }*/
    /*
        [HttpPost]
        public async Task<IActionResult> AdminLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var trainer = await _gfContext.Trainers.FirstOrDefaultAsync(t => t.Email == model.Username);
                if (trainer == null)
                {
                    _notyfService.Warning("Trainer not found!");
                    return View();
                }
                if (trainer != null && model.Password == "getfit438%")
                {
                    var memberDetail = await _gfContext.Trainers.AnyAsync(x => x.Name == trainer.Name);
                    var redirectResult = memberDetail ? RedirectToAction("Index", "Member") : RedirectToAction("MemberRegistration", "Member");
                    *//*var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
                    _notyfService.Success("fuul");*//*

                    _notyfService.Success("Login succesful");
                    return redirectResult;

                }

                ModelState.AddModelError("", "Invalid login attempt");
                _notyfService.Error("Invalid login attempt");
            }

            return View(model);
        }*/


    [RedirectAuthenticatedUsers]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost("/Auth/SignUp")]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == model.Email || u.UserName == model.Username);

            if (existingUser != null)
            {
                _notyfService.Warning("User already exist!");
                return View();
            }

            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                _notyfService.Error("An error occured while registering user!");
                return View();
            }

            _notyfService.Success("Registration was successful");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("MemberRegistration", "Member");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Login", "Auth");
    }
}




