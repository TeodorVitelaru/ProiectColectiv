using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.Match;
using AutoMapper;

namespace DatingApp.Service
{
    /// <summary>
    /// Service for Match operations.
    /// </summary>
    internal sealed class MatchService : IMatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public MatchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new match (like) between two users.
        /// </summary>
        /// <param name="userId">The ID of the user creating the match.</param>
        /// <param name="request">The match request containing the matched user ID.</param>
        /// <returns>The created match response.</returns>
        public async Task<MatchResponse> AddMatchAsync(long userId, AddMatchRequest request)
        {
            // Check if users exist
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
            {
                throw new ArgumentException("Current user not found.", nameof(userId));
            }

            var matchedUser = await _unitOfWork.UserRepository.GetByIdAsync(request.MatchedUserId);
            if (matchedUser is null)
            {
                throw new ArgumentException("User to match with not found.", nameof(request.MatchedUserId));
            }

            // Prevent self-matching
            if (userId == request.MatchedUserId)
            {
                throw new InvalidOperationException("Users cannot match with themselves.");
            }

            // Check if match already exists
            var existingMatch = await _unitOfWork.MatchRepository.FindByUserIdsAsync(userId, request.MatchedUserId);
            if (existingMatch is not null)
            {
                // If one-way match exists and other user has also liked, mark as mutual
                if (!existingMatch.IsMutual)
                {
                    existingMatch.MarkAsMutual();
                    _unitOfWork.MatchRepository.Update(existingMatch);
                    await _unitOfWork.SaveChangesAsync();

                    return new MatchResponse
                    {
                        Id = existingMatch.Id,
                        IsMutual = true,
                        Message = "Mutual match!"
                    };
                }

                throw new InvalidOperationException("Match already exists between these users.");
            }

            // Create new match
            var match = Match.Create(userId, request.MatchedUserId, false);

            // Check if the other user has already liked this user
            var reverseMatch = await _unitOfWork.MatchRepository.FindByUserIdsAsync(request.MatchedUserId, userId);
            if (reverseMatch is not null)
            {
                // Mark both as mutual
                match.MarkAsMutual();
                reverseMatch.MarkAsMutual();
                _unitOfWork.MatchRepository.Update(reverseMatch);
            }

            _unitOfWork.MatchRepository.Add(match);
            await _unitOfWork.SaveChangesAsync();

            return new MatchResponse
            {
                Id = match.Id,
                IsMutual = match.IsMutual,
                Message = match.IsMutual ? "Mutual match!" : "Like sent successfully."
            };
        }

        /// <summary>
        /// Gets all matches for a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Collection of match DTOs.</returns>
        public async Task<IEnumerable<MatchDto>> GetUserMatchesAsync(long userId)
        {
            var matches = await _unitOfWork.MatchRepository.GetUserMatchesAsync(userId);
            
            var matchDtos = matches.Select(m =>
            {
                var dto = _mapper.Map<MatchDto>(m);
                
                // Determine which user is the matched one from current user's perspective
                var matchedUser = m.UserId == userId ? m.MatchedUser : m.User;
                
                if (matchedUser is not null)
                {
                    dto.MatchedUserDetails = new MatchedUserDto
                    {
                        Id = matchedUser.Id,
                        FirstName = matchedUser.FirstName,
                        LastName = matchedUser.LastName,
                        Age = matchedUser.Age,
                        Location = matchedUser.City,
                        Bio = matchedUser.Bio,
                        ProfilePhotoUrl = matchedUser.Images?.FirstOrDefault() != null ? 
                            Convert.ToBase64String(matchedUser.Images.First().ImageData) : null
                    };
                }

                return dto;
            });

            return matchDtos;
        }

        /// <summary>
        /// Gets only mutual matches for a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Collection of mutual match DTOs.</returns>
        public async Task<IEnumerable<MatchDto>> GetMutualMatchesAsync(long userId)
        {
            var matches = await _unitOfWork.MatchRepository.GetMutualMatchesAsync(userId);
            
            var matchDtos = matches.Select(m =>
            {
                var dto = _mapper.Map<MatchDto>(m);
                
                // Determine which user is the matched one from current user's perspective
                var matchedUser = m.UserId == userId ? m.MatchedUser : m.User;
                
                if (matchedUser is not null)
                {
                    dto.MatchedUserDetails = new MatchedUserDto
                    {
                        Id = matchedUser.Id,
                        FirstName = matchedUser.FirstName,
                        LastName = matchedUser.LastName,
                        Age = matchedUser.Age,
                        Location = matchedUser.City,
                        Bio = matchedUser.Bio,
                        ProfilePhotoUrl = matchedUser.Images?.FirstOrDefault() != null ? 
                            Convert.ToBase64String(matchedUser.Images.First().ImageData) : null
                    };
                }

                return dto;
            });

            return matchDtos;
        }

        /// <summary>
        /// Checks if two users have matched.
        /// </summary>
        /// <param name="userId1">First user ID.</param>
        /// <param name="userId2">Second user ID.</param>
        /// <returns>True if users have matched, false otherwise.</returns>
        public async Task<bool> AreMatchedAsync(long userId1, long userId2)
        {
            var match = await _unitOfWork.MatchRepository.FindByUserIdsAsync(userId1, userId2);
            return match is not null;
        }

        /// <summary>
        /// Checks if users have a mutual match.
        /// </summary>
        /// <param name="userId1">First user ID.</param>
        /// <param name="userId2">Second user ID.</param>
        /// <returns>True if mutual, false otherwise.</returns>
        public async Task<bool> IsMutualMatchAsync(long userId1, long userId2)
        {
            var match = await _unitOfWork.MatchRepository.FindByUserIdsAsync(userId1, userId2);
            return match?.IsMutual ?? false;
        }

        /// <summary>
        /// Deletes a match (unlike).
        /// </summary>
        /// <param name="matchId">The match ID to delete.</param>
        /// <returns>True if deleted successfully, false otherwise.</returns>
        public async Task<bool> DeleteMatchAsync(long matchId)
        {
            var match = await _unitOfWork.MatchRepository.GetByIdAsync(matchId);
            if (match is null)
            {
                return false;
            }

            _unitOfWork.MatchRepository.Remove(match);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets random unmatched users for a specific user.
        /// </summary>
        /// <param name="userId">The current user ID.</param>
        /// <param name="count">Number of random users to return.</param>
        /// <returns>Collection of unmatched users.</returns>
        public async Task<IEnumerable<User>> GetRandomUnmatchedUsersAsync(long userId, int count = 1)
        {
            return await _unitOfWork.MatchRepository.GetRandomUnmatchedUsersAsync(userId, count);
        }
    }
}
