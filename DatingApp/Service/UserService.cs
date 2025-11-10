using AutoMapper;
using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services;
using DatingApp.Contracts.Services.HelperService;
using DatingApp.Contracts.Validators;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.User;
using DatingApp.Exceptions;
using System.Data;

namespace DatingApp.Service
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestValidator _requestValidator;
        private readonly IMapper _mapper;
        private readonly IPasswordHasherService _passwordHasher;

        /// <summary>
        /// Initializes a new instance of <see cref="UserService"/> class.
        /// </summary>
        /// <param name="logger">Writes log messages.</param>
        /// <param name="unitOfWork">Used for database migrations.</param>
        /// <param name="requestValidator">Used for validating requests.</param>
        /// <param name="mapper">Used for mapping objects.</param>
        /// <param name="passwordHasher">Used for hashing passwords.</param>
        public UserService(ILogger<UserService> logger, IUnitOfWork unitOfWork, IRequestValidator requestValidator, IMapper mapper, IPasswordHasherService passwordHasher)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _requestValidator = requestValidator;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> AddUserAsync(AddUserRequest request)
        {
            _logger.LogTrace("Add user called");

            _requestValidator.Validate(request);

            User? userWithSameEmail = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.Email == request.Email);
            if (userWithSameEmail != null)
            {
                throw new BadRequestException("User with provided email address already exists.");
            }

            User user = await _unitOfWork.UserRepository.AddAsync(User.Create(
                request.FirstName,
                request.LastName,
                request.Email,
                _passwordHasher.GenerateHashedPassword(request.Password),
                request.IsAdmin));

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteUserAsync(DeleteUserRequest request)
        {
            _logger.LogTrace("Delete user called");

            _requestValidator.Validate(request);

            User user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(User), request.Id);

            await _unitOfWork.UserRepository.RemoveByIdAsync(user.Id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<UserDto> EditUserAsync(EditUserRequest request)
        {
            _logger.LogTrace("Edit user called");

            _requestValidator.Validate(request);

            User user = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.Id == request.Id) ?? throw new NotFoundException(nameof(User), request.Id);

            if (user.Email != request.Email)
            {
                User? userWithSameEmail = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.Email == request.Email && u.Id != request.Id);
                if (userWithSameEmail != null)
                {
                    throw new BadRequestException("User with provided email address already exists.");
                }
                user.UpdateEmail(request.Email);
            }

            user.UpdateFirstName(request.FirstName);
            user.UpdateLastName(request.LastName);  

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            _logger.LogTrace("Get all users called");

            IEnumerable<User> users = await _unitOfWork.UserRepository.GetAllAsync();

            if (!users.Any())
            {
                throw new NotFoundException("There are no users");
            }

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserAsync(GetUserRequest request)
        {
            _logger.LogTrace("Get user called");

            _requestValidator.Validate(request);

            User user = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.Id == request.Id) ?? throw new NotFoundException(nameof(User), request.Id);

            return _mapper.Map<UserDto>(user);
        }
    }
}
