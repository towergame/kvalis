using System.ComponentModel.DataAnnotations;

namespace Kvalis.Entities;

public class Ballot
{
    [Key]
    public Guid Id { get; set; }
    
    public byte[]? PublicKey { get; set; } // If this is not null, the ballot has been received
    
    public bool Submitted { get; set; } // If this is true, the ballot has been submitted

    public virtual Election Election { get; set; }
}