﻿using AutoMapper;
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

        public async Task<UserDto> RegisterUserAsync(RegisterUserRequest request)
        {
            _logger.LogTrace("Register user called");

            _requestValidator.Validate(request);

            // Check if email already exists
            User? userWithSameEmail = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.Email == request.Email);
            if (userWithSameEmail != null)
            {
                throw new BadRequestException("User with provided email address already exists.");
            }

            // Create user with complete profile
            User user = await _unitOfWork.UserRepository.AddAsync(Domain.Entities.User.CreateWithProfile(
                request.FirstName,
                request.LastName,
                request.Email,
                _passwordHasher.GenerateHashedPassword(request.Password),
                request.Age,
                request.Height,
                request.Gender,
                request.Location,
                request.Bio,
                request.RelationshipGoal,
                request.SexualOrientation,
                request.PreferredAgeMin,
                request.PreferredAgeMax));

            await _unitOfWork.SaveChangesAsync();

            // Add languages
            foreach (var language in request.Languages)
            {
                var userLanguage = Domain.Entities.UserLanguage.Create(user.Id, language);
                await _unitOfWork.UserLanguageRepository.AddAsync(userLanguage);
            }

            // Add interests (hobbies)
            foreach (var interest in request.Hobbies)
            {
                var userInterest = Domain.Entities.UserInterest.Create(user.Id, interest);
                await _unitOfWork.UserInterestRepository.AddAsync(userInterest);
            }

            // Process and add photos
            if (request.Photos != null && request.Photos.Any())
            {
                foreach (var photo in request.Photos)
                {
                    try
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await photo.CopyToAsync(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            
                            // Validate image size (max 5MB)
                            if (imageBytes.Length > 5 * 1024 * 1024)
                            {
                                throw new BadRequestException($"Image {photo.FileName} is too large. Maximum size is 5MB.");
                            }
                            
                            // Create image entity
                            var image = Domain.Entities.Image.Create(imageBytes, user.Id);
                            await _unitOfWork.ImageRepository.AddAsync(image);
                        }
                    }
                    catch (Exception ex) when (ex is not BadRequestException)
                    {
                        throw new BadRequestException($"Failed to process image {photo.FileName}. Error: {ex.Message}");
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();

            // Reload user with related data
            User? registeredUser = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(
                u => u.Id == user.Id,
                u => u.UserLanguages,
                u => u.UserInterests,
                u => u.Images);

            return _mapper.Map<UserDto>(registeredUser);
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

            IEnumerable<User> users = await _unitOfWork.UserRepository.FindAsync(
                u => true,
                u => u.UserLanguages,
                u => u.UserInterests,
                u => u.Images);

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

            User user = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(
                u => u.Id == request.Id,
                u => u.UserLanguages,
                u => u.UserInterests,
                u => u.Images) ?? throw new NotFoundException(nameof(User), request.Id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetRandomUserAsync(int currentUserId)
        {
            _logger.LogTrace("Get random user called");

            User user = await _unitOfWork.UserRepository.GetRandomUserAsync(currentUserId) ?? throw new NotFoundException("No users found");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<SetupProfileResponse> SetupProfileAsync(long userId, SetupProfileRequest request)
        {
            _logger.LogTrace($"Setup profile for user {userId} called");

            // Validate age range
            if (request.AgeRangeMin > request.AgeRangeMax)
            {
                throw new BadRequestException("Minimum age cannot be greater than maximum age");
            }

            // Get the user
            User user = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.Id == userId) 
                ?? throw new NotFoundException(nameof(User), userId);

            // Update basic information
            user.UpdateAge(request.Age);
            user.UpdateHeight(request.Height);
            user.UpdateGender(request.Gender);
            user.UpdateCity(request.Location);
            user.UpdateBio(request.Bio);
            user.UpdateRelationshipGoal(request.RelationshipType);
            user.UpdateSexualOrientation(request.SexualOrientation);
            user.UpdatePreferredAgeRange(request.AgeRangeMin, request.AgeRangeMax);

            // Remove existing languages and add new ones
            var existingLanguages = await _unitOfWork.UserLanguageRepository.FindAsync(ul => ul.UserId == userId);
            foreach (var lang in existingLanguages)
            {
                await _unitOfWork.UserLanguageRepository.RemoveByIdAsync(lang.Id);
            }

            foreach (var language in request.Languages)
            {
                var userLanguage = Domain.Entities.UserLanguage.Create(userId, language);
                await _unitOfWork.UserLanguageRepository.AddAsync(userLanguage);
            }

            // Remove existing interests and add new ones
            var existingInterests = await _unitOfWork.UserInterestRepository.FindAsync(ui => ui.UserId == userId);
            foreach (var interest in existingInterests)
            {
                await _unitOfWork.UserInterestRepository.RemoveByIdAsync(interest.Id);
            }

            foreach (var hobby in request.Hobbies)
            {
                var userInterest = Domain.Entities.UserInterest.Create(userId, hobby);
                await _unitOfWork.UserInterestRepository.AddAsync(userInterest);
            }

            // Remove existing photos
            var existingPhotos = await _unitOfWork.ImageRepository.GetImagesByUserIdAsync(userId);
            foreach (var photo in existingPhotos)
            {
                await _unitOfWork.ImageRepository.RemoveByIdAsync(photo.Id);
            }

            // Add new photos
            if (request.Photos != null && request.Photos.Any())
            {
                foreach (var photo in request.Photos)
                {
                    using var memoryStream = new MemoryStream();
                    await photo.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    
                    var image = Domain.Entities.Image.Create(imageBytes, userId);
                    await _unitOfWork.ImageRepository.AddAsync(image);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return new SetupProfileResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfileCompleted = true
            };
        }
    }
}
