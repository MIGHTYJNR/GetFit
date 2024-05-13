namespace GetFit.Data;

public class Payment : BaseEntity
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }

    public int MemberId { get; set; }
    public MemberDetail Member { get; set; } = default!;

}
