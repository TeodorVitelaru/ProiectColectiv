using AutoMapper;
using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services;
using DatingApp.Contracts.Services.HelperService;
using DatingApp.Contracts.Validators;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.Hobbie;
using DatingApp.Dtos.User;
using DatingApp.Exceptions;
using Microsoft.Extensions.Logging;

namespace DatingApp.Service
{
    public class HobbieService : IHobbieService
    {
        private readonly ILogger<HobbieService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestValidator _requestValidator;
        private readonly IMapper _mapper;

        public HobbieService(
            ILogger<HobbieService> logger,
            IUnitOfWork unitOfWork,
            IRequestValidator requestValidator,
            IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _requestValidator = requestValidator;
            _mapper = mapper;
        }

        public async Task<HobbieDto> AddHobbieAsync(AddHobbieRequest request)
        {
            _logger.LogTrace("Add hobbie called");

            _requestValidator.Validate(request);

            Hobbie? hobbieWithSameName =
                await _unitOfWork.HobbieRepository.FindFirstOrDefaultAsync(h => h.HobbieName == request.HobbieName);

            if (hobbieWithSameName != null)
                throw new BadRequestException($"Hobbie '{request.HobbieName}' already exists.");

            Hobbie hobbie = Hobbie.Create(request.HobbieName);

            hobbie = await _unitOfWork.HobbieRepository.AddAsync(hobbie);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<HobbieDto>(hobbie);
        }

        public async Task DeleteReviewAsync(DeleteHobbieRequest request)
        {
            _logger.LogTrace("Delete hobbie called");

            _requestValidator.Validate(request);

            Hobbie hobbie =
                await _unitOfWork.HobbieRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Hobbie), request.Id);

            await _unitOfWork.HobbieRepository.RemoveByIdAsync(hobbie.Id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<HobbieDto> EditHobbieAsync(EditHobbieRequest request)
        {
            _logger.LogTrace("Edit hobbie called");

            _requestValidator.Validate(request);

            Hobbie hobbie =
                await _unitOfWork.HobbieRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Hobbie), request.Id);

            // verificare nume duplicat
            if (hobbie.HobbieName != request.HobbieName)
            {
                Hobbie? other =
                    await _unitOfWork.HobbieRepository.FindFirstOrDefaultAsync(h => h.HobbieName == request.HobbieName);

                if (other != null)
                    throw new BadRequestException($"Hobbie '{request.HobbieName}' already exists.");

                hobbie.UpdateHobbieName(request.HobbieName);
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<HobbieDto>(hobbie);
        }

        public async Task<IEnumerable<HobbieDto>> GetAllHobbiesAsync()
        {
            _logger.LogTrace("Get all hobbies called");

            var hobbies = await _unitOfWork.HobbieRepository.GetAllAsync();

            if (!hobbies.Any())
                throw new NotFoundException("There are no hobbies.");

            return _mapper.Map<IEnumerable<HobbieDto>>(hobbies);
        }

        public async Task<IEnumerable<HobbieDto>> GetAllHobbiesForUserAsync(GetAllHobbiesForUserRequest request)
        {
            _logger.LogTrace("Get all hobbies for user called");

            _requestValidator.Validate(request);

            User user = await _unitOfWork.UserRepository.GetUserWithAllHobbies(request.UserId)
                       ?? throw new NotFoundException(nameof(User), request.UserId);

            var hobbies = user.Hobbies;

            return _mapper.Map<IEnumerable<HobbieDto>>(hobbies);
        }

        public async Task<HobbieDto> GetHobbieAsync(GetHobbieRequest request)
        {
            _logger.LogTrace("Get hobbie called");

            _requestValidator.Validate(request);

            Hobbie hobbie =
                await _unitOfWork.HobbieRepository.GetByIdAsync(request.HobbieId)
                ?? throw new NotFoundException(nameof(Hobbie), request.HobbieId);

            return _mapper.Map<HobbieDto>(hobbie);
        }

        public async Task<UserDto> AddHobbieToUserAsync(AddHobbieToUserRequest request)
        {
            _logger.LogTrace("Add hobbie to user called");

            _requestValidator.Validate(request);

            // 1. Load user + Hobbies
            User user = await _unitOfWork.UserRepository
                .FindFirstOrDefaultAsync(u => u.Id == request.UserId, u => u.Hobbies)
                ?? throw new NotFoundException(nameof(User), request.UserId);

            // 2. Load hobbie
            Hobbie hobbie = await _unitOfWork.HobbieRepository.GetByIdAsync(request.HobbieId)
                ?? throw new NotFoundException(nameof(Hobbie), request.HobbieId);

            // 3. Check if hobby already linked
            if (user.Hobbies.Any(h => h.Id == hobbie.Id))
                throw new BadRequestException("User already has this hobbie.");

            // 4. Add hobby to user
            user.Hobbies.Add(hobbie);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}
