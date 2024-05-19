namespace GetFit.Data;

public class FitnessClass : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Schedule { get; set; } = default!;
    public int Duration { get; set; }

    public int TrainerId { get; set; }
    public Trainer Trainer { get; set; } = default!;
    
  
    //public int MemberDetailId { get; set; } = default!;
 //   public Member Member { get; set; } = default!;

    /*public int MemberDetailId { get; set; } = default!;*/
    /*public Member Member { get; set; } = default!;*/

    public ICollection<Member> Members{ get; set; } = default!;
}
