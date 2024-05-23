using GetFit.Data.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GetFit.Models.Member;

public class MemberModel
{
    [Display(Name = "FullName")]
    [Required]
    public string Name { get; set; } = default!;

    [Display(Name = "Email")]
    [Required] 
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;

    [Display(Name = "Phone Number")]
    [Required]
    public string PhoneNumber { get; set; } = default!;

    [Display(Name = "Age")]
    [Required]
    public int Age { get; set; } = default!;

    [Display(Name = "Gender")]
    [Required(ErrorMessage = "Please select a gender")]
    public Gender Gender { get; set; } = default!;

    [Display(Name = "Address")]
    [Required]
    public string Address { get; set; } = default!;

    [Display(Name = "Emergency Contact")]
    [Required]
    public string EmergencyContact { get; set; } = default!;

    [Display(Name = "Fitness Goal")]
    [Required]
    public string FitnessGoals { get; set; } = default!;

    [Display(Name = "Membership Type")]
    [Required(ErrorMessage = "Please select a Membership Type")]
    public int MembershipTypeId { get; set; }
    public List<SelectListItem> MembershipTypes { get; set; } = new List<SelectListItem>();

    [Display(Name = "Trainer")]
    [Required(ErrorMessage = "Please select a trainer")]
    public int TrainerId { get; set; }
    public List<SelectListItem> Trainers { get; set; } = new List<SelectListItem>();

    [Display(Name = "Classes")]
    [Required(ErrorMessage = "Please select at least one class")]
    public int FitnessClassId { get; set; }
    public List<int> FitnessClassIds { get; set; } = new List<int>();

    public List<SelectListItem> FitnessClasses { get; set; } = new List<SelectListItem>();

}
