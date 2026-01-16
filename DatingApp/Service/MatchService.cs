using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.Match;
using DatingApp.Hubs;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.Service
{
    /// <summary>
    /// Service for Match operations.
    /// </summary>
    internal sealed class MatchService : IMatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="hubContext">The SignalR hub context for notifications.</param>
        public MatchService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
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

            // Check if current user already liked this person (direct match: userId -> matchedUserId)
            var existingMatch = await _unitOfWork.MatchRepository.FindByUserIdsAsync(userId, request.MatchedUserId);
            if (existingMatch is not null)
            {
                // User already liked this person
                if (existingMatch.UserId == userId)
                {
                    throw new InvalidOperationException("You already liked this user.");
                }
                
                // This means the other user liked us first, now we're liking back
                // Mark the existing match as mutual and create our own match record
                if (!existingMatch.IsMutual)
                {
                    existingMatch.MarkAsMutual();
                    _unitOfWork.MatchRepository.Update(existingMatch);
                }
                
                // Create the reverse match also marked as mutual
                var mutualMatch = Match.Create(userId, request.MatchedUserId, true);
                _unitOfWork.MatchRepository.Add(mutualMatch);
                await _unitOfWork.SaveChangesAsync();

                // Get matched user's images for response
                var matchedUserImages = await _unitOfWork.ImageRepository.GetImagesByUserIdAsync(request.MatchedUserId);
                var firstImage = matchedUserImages.FirstOrDefault();

                // Get current user's images
                var currentUserImages = await _unitOfWork.ImageRepository.GetImagesByUserIdAsync(userId);
                var currentUserFirstImage = currentUserImages.FirstOrDefault();

                var matchedUserInfo = new MatchedUserInfo
                {
                    Id = matchedUser.Id,
                    FirstName = matchedUser.FirstName,
                    LastName = matchedUser.LastName,
                    ProfilePhotoUrl = firstImage != null
                        ? Convert.ToBase64String(firstImage.ImageData)
                        : null
                };

                var currentUserInfo = new MatchedUserInfo
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePhotoUrl = currentUserFirstImage != null
                        ? Convert.ToBase64String(currentUserFirstImage.ImageData)
                        : null
                };

                // Send SignalR notification to the matched user (the one who liked first)
                var matchedUserConnectionId = NotificationHub.GetConnectionId(request.MatchedUserId);
                if (matchedUserConnectionId != null)
                {
                    await _hubContext.Clients.Client(matchedUserConnectionId).SendAsync("ReceiveMatchNotification", new
                    {
                        isMutual = true,
                        matchedUser = currentUserInfo
                    });
                }

                return new MatchResponse
                {
                    Id = mutualMatch.Id,
                    IsMutual = true,
                    Message = "Mutual match!",
                    MatchedUser = matchedUserInfo
                };
            }

            // Create new one-way match (we liked them, they haven't liked us yet)
            var match = Match.Create(userId, request.MatchedUserId, false);
            _unitOfWork.MatchRepository.Add(match);
            await _unitOfWork.SaveChangesAsync();

            return new MatchResponse
            {
                Id = match.Id,
                IsMutual = false,
                Message = "Like sent successfully."
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
        /// Adds a dislike record for a user.
        /// </summary>
        /// <param name="userId">The ID of the user creating the dislike.</param>
        /// <param name="dislikedUserId">The ID of the user being disliked.</param>
        /// <returns>Response indicating the dislike was recorded.</returns>
        public async Task<MatchResponse> AddDislikeAsync(long userId, long dislikedUserId)
        {
            // Validate users
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
            {
                throw new ArgumentException("Current user not found.", nameof(userId));
            }

            var dislikedUser = await _unitOfWork.UserRepository.GetByIdAsync(dislikedUserId);
            if (dislikedUser is null)
            {
                throw new ArgumentException("User to dislike not found.", nameof(dislikedUserId));
            }

            // Prevent self-disliking
            if (userId == dislikedUserId)
            {
                throw new InvalidOperationException("Users cannot dislike themselves.");
            }

            // Check if already disliked
            var existingMatch = await _unitOfWork.MatchRepository.FindByUserIdsAsync(userId, dislikedUserId);
            if (existingMatch is not null && existingMatch.UserId == userId)
            {
                throw new InvalidOperationException("You already interacted with this user.");
            }

            // Create dislike record
            var dislike = Match.Create(userId, dislikedUserId, isMutual: false, isLiked: false);
            _unitOfWork.MatchRepository.Add(dislike);
            await _unitOfWork.SaveChangesAsync();

            return new MatchResponse
            {
                Id = dislike.Id,
                IsMutual = false,
                Message = "Dislike recorded."
            };
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
