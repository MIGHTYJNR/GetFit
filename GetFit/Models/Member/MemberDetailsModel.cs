using GetFit.Data;
using GetFit.Data.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GetFit.Models.Member;

public class MemberDetailsModel
{
    public int Id { get; set; }

    [Display(Name = "Full Name")]
    public string Name { get; set; } = default!;

    [Display(Name = "Email")]
    public string Email { get; set; } = default!;

    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = default!;

    [Display(Name = "Age")]
    public int Age { get; set; }

    [Display(Name = "Gender")]
    public Gender Gender { get; set; } = default!;

    [Display(Name = "Address")]
    public string Address { get; set; } = default!;

    [Display(Name = "Emergency Contact")]
    public string EmergencyContact { get; set; } = default!;

    [Display(Name = "Fitness Goals")]
    public string FitnessGoals { get; set; } = default!;

    [Display(Name = "Fitness Class")]
    /*  public List<int> FitnessClasses { get; set; } = new List<int>();*/
    public List<string> FitnessClasses { get; set; } = [];

    [Display(Name = "Membership Type")]
    public string MembershipTypeId { get; set; } = default!;

    [Display(Name = "Preferred Trainer")]
    public string TrainerId { get; set; } = default!;

    public string MembershipTypeName { get; set; } = default!;
    public string MembershipTypeBenefits { get; set; } = default!;

    public string TrainerName { get; set; } = default!;
    public string TrainerSpecialization { get; set; } = default!;
}