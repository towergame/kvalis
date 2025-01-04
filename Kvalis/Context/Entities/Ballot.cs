using System.ComponentModel.DataAnnotations;

namespace Kvalis.Entities;

public class Ballot
{
    [Key]
    public Guid Id { get; set; }
    
    public byte[]? PublicKey { get; set; }
    
    public bool Submitted { get; set; }

    public virtual Election Election { get; set; }
}