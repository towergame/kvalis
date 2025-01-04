using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kvalis.Entities;

public class Question
{
    [Key]
    public Guid Id { get; set; } 
    
    public string Prompt { get; set; }
    
    public virtual ICollection<Option> Options { get; set; }
    public virtual Election Election { get; set; }
    public virtual ICollection<Vote> Votes { get; set; }
}