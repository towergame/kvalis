namespace Kvalis.Entities;

public class Vote
{
    public Guid Id { get; set; }
    
    public virtual Question Question { get; set; }
    public virtual Option Option { get; set; }
    
    public virtual Election Election { get; set; }
}