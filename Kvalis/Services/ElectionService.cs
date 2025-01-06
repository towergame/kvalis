using System.Text;
using Kvalis.Entities;

namespace Kvalis.Services;

public class ElectionService(KeyService keyService, KvalisContext context)
{
    private readonly KeyService _keyService = keyService;
    private readonly KvalisContext _context = context;
    
    public async Task<Election> GetElection(Guid id)
    {
        var election = _context.Elections.Find(id);
        if (election == null)
        {
            throw new Exception("Election not found");
        }
        else if (keyService.Validate(await keyService.Sign(Encoding.ASCII.GetBytes(election.Id.ToString())), election.PublicKey) == null)
        {
            throw new Exception("Invalid election signature");
        }

        return election;
    }

    public class ElectionResults
    {
        public Guid ElectionId { get; set; }
        public Dictionary<string, Dictionary<string, int>> Results { get; set; }
        public List<Ballot> Ballots { get; set; }
    }
    
    public ElectionResults GetResults(Guid electionId)
    {
        var election = _context.Elections.Find(electionId);
        if (election == null)
        {
            throw new Exception("Election not found");
        }
        if (election.EndDate > DateTime.Now)
        {
            throw new Exception("Election has not ended yet");
        }
        
        var results = new ElectionResults
        {
            ElectionId = electionId,
            Results = new Dictionary<string, Dictionary<string, int>>()
        };
        
        foreach (var question in election.Questions)
        {
            var questionResults = new Dictionary<string, int>();
            foreach (var option in question.Options)
            {
                questionResults[option.Text] = election.Votes.Count(vote => vote.Question.Id == question.Id && vote.Option.Id == option.Id);
            }
            results.Results[question.Prompt] = questionResults;
        }
        
        results.Ballots = _context.Ballots.Where(ballot => ballot.Election.Id == electionId).ToList();
        
        return results;
    }
}