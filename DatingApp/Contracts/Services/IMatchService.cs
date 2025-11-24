using DatingApp.Dtos.Match;
using DatingApp.Dtos.User;

namespace DatingApp.Contracts.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDto>> GetAllMatchesAsync();
        Task<IEnumerable<MatchDto>> GetAllMatchesOfUser(long userId);
        Task<MatchDto> AddMatchAsync(AddMatchRequest request);
        Task<MatchDto> GetMatchAsync(long id);
        Task DeleteMatchAsync(DeleteMatchRequest request);
    }
}
