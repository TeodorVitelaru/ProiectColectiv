using AutoMapper;
using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services;
using DatingApp.Contracts.Validators;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.Match;
using DatingApp.Dtos.Message;
using DatingApp.Exceptions;

namespace DatingApp.Service
{
    public class MatchService :IMatchService
    {
        private readonly ILogger<MatchService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestValidator _requestValidator;
        private readonly IMapper _mapper;
        public MatchService(ILogger<MatchService> logger, IUnitOfWork unitOfWork, IRequestValidator requestValidator, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _requestValidator = requestValidator;
            _mapper = mapper;
        }

        public async Task<MatchDto> AddMatchAsync(AddMatchRequest request)
        {
            _logger.LogTrace("Add match called");


            // validate users exist
            var User1 = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId1);
            if (User1 == null) throw new NotFoundException("User", request.UserId1);
            var User2 = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId2);
            if (User2 == null) throw new NotFoundException("User", request.UserId2);

            Match? matchWSameUsers = await _unitOfWork.MatchRepository.FindFirstOrDefaultAsync(u => (u.UserId1 == request.UserId1 && u.UserId2 == request.UserId2) || (u.UserId2 == request.UserId1 && u.UserId1 == request.UserId2));
            if (matchWSameUsers != null)
            {
                throw new BadRequestException("The match betwen these 2 Users already exists!");
            }

            var match = await _unitOfWork.MatchRepository.AddAsync(Match.Create(request.UserId1, request.UserId2, DateTime.Now));
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MatchDto>(match);
        }

        public async Task DeleteMatchAsync(DeleteMatchRequest request)
        {
            var existing = await _unitOfWork.MatchRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException("Match", request.Id);
            _unitOfWork.MatchRepository.Remove(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<MatchDto>> GetAllMatchesAsync()
        {
            var matches = await _unitOfWork.MatchRepository.GetAllAsync();
            return matches.Select(m => _mapper.Map<MatchDto>(m));
        }

        public async Task<IEnumerable<MatchDto>> GetAllMatchesOfUser(long UserId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(UserId);
            if (user == null) throw new NotFoundException("User", UserId);

            // Get all matches where user is either side of the match
            var matches = await _unitOfWork.MatchRepository.FindAsync(
                m => m.UserId1 == UserId || m.UserId2 == UserId
            );

            return matches.Select(m => _mapper.Map<MatchDto>(m));
        }

        public async Task<MatchDto> GetMatchAsync(long id)
        {
            var match = await _unitOfWork.MatchRepository.FindFirstOrDefaultAsync(m => m.Id == id)
                ?? throw new NotFoundException("Match", id);
            return _mapper.Map<MatchDto>(match);
        }
    }
}
