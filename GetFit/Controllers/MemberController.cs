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

        var member = new MemberDetail
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
            TrainerId = model.TrainerId
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
    public async Task<IActionResult> ViewMemberDetails(MemberDetailsModel model)
    {
        var user = await _userManager.GetUserAsync(User);

        var member = await _gfContext.MemberDetails
            .Include(m => m.MembershipType)
            .Include(m => m.PreferredTrainer)
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
                FitnessClasses = member.FitnessClasses.Select(fc => fc.Name).ToList(), //implicit convert error
                MembershipTypeId = member.MembershipTypeId.ToString(), //implicit convert error
                TrainerId = member.TrainerId.ToString(),

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
            return RedirectToAction("Index", "Member");
        }
    }


    public async Task<IActionResult> EditMemberDetails(int id)
    {
        var member = await _gfContext.MemberDetails.FindAsync(id);

        if (member == null)
        {
            _notyfService.Error("Member details not found");
            return RedirectToAction("Index", "Member");
        }

        var model = new MemberViewModel
        {
            Name = member.Name,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber,
            Age = member.Age,
            Gender = member.Gender,
            Address = member.Address,
            EmergencyContact = member.EmergencyContact,
            FitnessGoals = member.FitnessGoals,
            MembershipTypeId = member.MembershipTypeId,
            TrainerId = member.TrainerId,
            FitnessClasses = [.. _gfContext.FitnessClasses.Select(fc => new SelectListItem
            {
                Text = fc.Name,
                Value = fc.Id.ToString(),
                Selected = fc.Id == member.TrainerId
            })],
            MembershipTypes = [.. _gfContext.MembershipTypes.Select(mt => new SelectListItem
            {
                Text = mt.Name,
                Value = mt.Id.ToString(),
                Selected = mt.Id == member.MembershipTypeId
            })],
            Trainers = [.. _gfContext.Trainers.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString(),
                Selected = t.Id == member.TrainerId
            })]
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditMemberDetails(string phoneNumber, MemberViewModel model, string @string)
    {
        if (@string != model.PhoneNumber)
        {
            _notyfService.Error("Invalid member details");
            return RedirectToAction("Index", "Member");
        }

        var member = await _gfContext.MemberDetails.FindAsync(phoneNumber);

        if (member == null)
        {
            _notyfService.Error("Member details not found");
            return RedirectToAction("Index", "Member");
        }

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

        _gfContext.Update(member);
        var result = await _gfContext.SaveChangesAsync();

        if (result > 0)
        {
            _notyfService.Success("Member details updated successfully");
            return RedirectToAction("Index", "Member");
        }

        _notyfService.Error("An error occurred while updating member details");
        return View(model);
    }


    public async Task<IActionResult> DeleteMemberDetails(int id)
    {
        var member = await _gfContext.MemberDetails.FindAsync(id);

        if (member == null)
        {
            _notyfService.Error("Member details not found");
            return RedirectToAction("Index", "Member");
        }

        _gfContext.Remove(member);
        var result = await _gfContext.SaveChangesAsync();

        if (result > 0)
        {
            _notyfService.Success("Member details deleted successfully");
            return RedirectToAction("MemberRegistration", "Member");
        }

        _notyfService.Error("An error occurred while deleting member details");
        return RedirectToAction("Index", "Member");
    }
}
