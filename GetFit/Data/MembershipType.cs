namespace GetFit.Data;

public class MembershipType : BaseEntity
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public string Benefits { get; set; } = default!;
    public int Duration { get; set; }

    public ICollection<MemberDetail> Members { get; set; } = default!;
}
