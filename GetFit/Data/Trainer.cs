namespace GetFit.Data;

public class Trainer : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Specialization { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;

    public ICollection<Member> Members { get; set; } = default!;
    public ICollection<FitnessClass> FitnessClasses { get; set; } = default!;
}
