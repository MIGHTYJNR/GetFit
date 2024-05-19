using GetFit.Context;
using GetFit.Data;
using GetFit.Models.Member;
using GetFit.Utility;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCore;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace GetFit.Controllers;

[Authorize]
public class MemberController(UserManager<IdentityUser> userManager,
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

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult MemberRegistration()
    {
        var membershipTypes = _gfContext.MembershipTypes.Select(mt => new SelectListItem
        {
            Text = mt.Name,
            Value = mt.Id.ToString()
        }).ToList();

        var trainers = _gfContext.Trainers.Select(t => new SelectListItem
        {
            Text = t.Name,
            Value = t.Id.ToString()
        }).ToList();

        var fitnessClasses = _gfContext.FitnessClasses.Select(fc => new SelectListItem
        {
            Text = fc.Name,
            Value = fc.Id.ToString()
        }).ToList();

        var viewModel = new MemberViewModel
        {
            MembershipTypes = membershipTypes,
            Trainers = trainers,
            FitnessClasses = fitnessClasses
        };

        return View(viewModel);
    }

    [HttpPost("Member/MemberRegistration")]
    public async Task<IActionResult> MemberRegistration(MemberViewModel model)
    {
        var memberExist = await _gfContext.MemberDetails.AnyAsync(x => x.Name == model.Name || x.Email == model.Email);

        var userDetail = await Helper.GetCurrentUserIdAsync(_httpContextAccessor, _userManager);

        if (memberExist)
        {
            _notyfService.Warning("Member already exists");
            return View(model);
        }

        var member = new Member
        {
            UserId = userDetail.userId,
            Name = model.Name,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Age = model.Age,
            Gender = model.Gender,
            Address = model.Address,
            EmergencyContact = model.EmergencyContact,
            FitnessGoals = model.FitnessGoals,
            MembershipTypeId = model.MembershipTypeId,
            TrainerId = model.TrainerId,
            FitnessClassId = model.FitnessClassId
        };

        await _gfContext.AddAsync(member);
        var result = await _gfContext.SaveChangesAsync();

        if (result > 0)
        {
            _notyfService.Success("Member detail registered successfully");
            return RedirectToAction("Index", "Member");
        }

        _notyfService.Error("An error occurred while creating member detail");
        return View();
    }


    [HttpGet("Member/ViewMemberDetails")]
    public async Task<IActionResult> ViewMemberDetails()
    {
        var user = await _userManager.GetUserAsync(User);

        var member = await _gfContext.MemberDetails
            .Include(m => m.MembershipType)
            .Include(m => m.PreferredTrainer)
            .Include(m => m.FitnessClass)
            .FirstOrDefaultAsync(m => m.UserId == user!.Id);


        if (member != null)
        {
            var memberDetailsModel = new MemberDetailsModel
            {
                Name = member.Name.ToUpper(),
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                Age = member.Age,
                Gender = member.Gender,
                Address = member.Address,
                EmergencyContact = member.EmergencyContact,
                FitnessGoals = member.FitnessGoals,
                MembershipTypeId = member.MembershipTypeId.ToString(),
                TrainerId = member.TrainerId.ToString(),
                FitnessClassName = member.FitnessClass.Name.ToUpper(),

                FitnessClassSchedule = member.FitnessClass.Schedule,
                MembershipTypeName = member.MembershipType.Name.ToUpper(),
                MembershipTypeBenefits = member.MembershipType.Benefits,
                TrainerName = member.PreferredTrainer.Name.ToUpper(),
                TrainerSpecialization = member.PreferredTrainer.Specialization

            };

            return View(memberDetailsModel);
        }
        else
        {
            _notyfService.Error("Member details not found");
            return RedirectToAction("MemberRegistration", "Member");
        }
    }

    public IActionResult UpdateMemberDetails()
    {
        var membershipTypes = _gfContext.MembershipTypes.Select(mt => new SelectListItem
        {
            Text = mt.Name,
            Value = mt.Id.ToString()
        }).ToList();

        var trainers = _gfContext.Trainers.Select(t => new SelectListItem
        {
            Text = t.Name,
            Value = t.Id.ToString()
        }).ToList();

        var fitnessClasses = _gfContext.FitnessClasses.Select(fc => new SelectListItem
        {
            Text = fc.Name,
            Value = fc.Id.ToString()
        }).ToList();

        var ViewModel = new MemberViewModel
        {
            MembershipTypes = membershipTypes,
            Trainers = trainers,
            FitnessClasses = fitnessClasses
        };

        return View(ViewModel);
        //return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateMemberDetails(MemberViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);

            var member = await _gfContext.MemberDetails
                .FirstOrDefaultAsync(m => m.UserId == user!.Id);

            if (member != null)
            {
                member.Name = model.Name;
                member.Email = model.Email;
                member.PhoneNumber = model.PhoneNumber;
                member.Age = model.Age;
                member.Gender = model.Gender;
                member.Address = model.Address;
                member.EmergencyContact = model.EmergencyContact;
                member.FitnessGoals = model.FitnessGoals;
                member.MembershipTypeId = model.MembershipTypeId;
                member.TrainerId = model.TrainerId;
                member.FitnessClassId = model.FitnessClassId;

                _gfContext.Update(member);
                await _gfContext.SaveChangesAsync();

                _notyfService.Success("Member details updated successfully");
                return RedirectToAction("Index", "Member");
            }
            else
            {
                _notyfService.Error("Member details not found");
                return RedirectToAction("Index", "Member");
            }
        }

        _notyfService.Error("Invalid model state");
        return View(model);
    }


    public IActionResult DeleteMember()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteMember(MemberDetailsModel model)
    {
        var user = await _userManager.GetUserAsync(User);

        var member = await _gfContext.MemberDetails
            .FirstOrDefaultAsync(m => m.UserId == user!.Id);

        if (member != null)
        {
            _gfContext.Remove(member);
            await _gfContext.SaveChangesAsync();

            _notyfService.Success("Member details deleted successfully");
            return RedirectToAction("MemberRegistration", "Member");
        }
        else
        {
            _notyfService.Error("Member details not found");
            return RedirectToAction("DeleteMember", "Member");
        }

        
    }
}
