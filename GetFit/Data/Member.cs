﻿using GetFit.Data.Enum;

namespace GetFit.Data;

public class Member : BaseEntity
{
    public string UserId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public int Age { get; set; }
    public Gender Gender { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string EmergencyContact { get; set; } = default!;
    public string FitnessGoals { get; set; } = default!;

    public int MembershipTypeId { get; set; }
    public MembershipType MembershipType { get; set; } = default!;

    public int TrainerId { get; set; }
    public Trainer PreferredTrainer { get; set; } = default!;

    public int FitnessClassId { get; set; }
    public FitnessClass FitnessClass { get; set; } = default!;

    /*public ICollection<Payment> Payments { get; set; } = default!;*/
    /*public ICollection<FitnessClassName> FitnessClassName { get; set; } = new List<FitnessClassName>();*/

   // public List<int> FitnessClassIds { get; set; } = new List<int>();
}
