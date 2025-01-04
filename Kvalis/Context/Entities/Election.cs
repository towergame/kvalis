using System.ComponentModel.DataAnnotations;

namespace Kvalis.Entities;

public class Election
{
    [Key]
    public Guid Id { get; set; } 
    
    public DateTime EndDate { get; set; }
    
    public byte[] PublicKey { get; set; }
    
    public virtual ICollection<Question> Questions { get; set; }
    public virtual ICollection<Vote> Votes { get; set; }
    public virtual ICollection<Ballot> Ballots { get; set; }
}