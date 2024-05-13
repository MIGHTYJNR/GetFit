namespace GetFit.Data;

public class FitnessClass : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Schedule { get; set; } = default!;
    public int Duration { get; set; }

    public int TrainerId { get; set; }
    public Trainer Instructor { get; set; } = default!;

    public ICollection<MemberClass> MemberClasses { get; set; } = default!;

}
