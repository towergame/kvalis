using System.ComponentModel.DataAnnotations;

namespace Kvalis.Entities;

public class Option
{
    [Key]
    public Guid Id { get; set; } 
    
    public string Text { get; set; }
    
    public virtual Question Question { get; set; }
}