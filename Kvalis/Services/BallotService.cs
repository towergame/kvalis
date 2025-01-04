using System.Text;
using Kvalis.Entities;

namespace Kvalis.Services;

public class BallotService(KeyService keyService, KvalisContext context)
{
    private readonly KeyService _keyService = keyService;
    private readonly KvalisContext _context = context;
    
    public async Task<Ballot> GetElection(Guid id)
    {
        var ballot = _context.Ballots.Find(id);
        
        if (ballot == null)
        {
            throw new Exception("Ballot not found");
        }
        if (ballot.PublicKey != null && _keyService.Validate(await _keyService.Sign(Encoding.ASCII.GetBytes(ballot.Id.ToString())), ballot.PublicKey) == null)
        {
            throw new Exception("Invalid ballot signature");
        }

        return ballot;
    }
}