namespace GetFit.Data;

public class MemberClass : BaseEntity
{
    public int MemberId { get; set; }
    public MemberDetail Member { get; set; } = default!;

    public int FitnessClassId { get; set; }
    public FitnessClass FitnessClass { get; set; } = default!;

}