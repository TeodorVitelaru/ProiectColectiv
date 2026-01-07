using DatingApp.Contracts.Services;
using DatingApp.Contracts.Services.HelperService;
using DatingApp.Contracts.Persistence;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.User.Login;
using DatingApp.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp.Service
{
    /// <summary>
    /// Service for simple user registration with JWT token generation.
    /// </summary>
    public class RegisterSimpleService : IRegisterSimpleService
    {
        private readonly ILogger<RegisterSimpleService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasherService _passwordHasherService;

        private readonly string _issuer;
        private readonly string _key;
        private readonly int _defaultDuration;

        /// <summary>
        /// Initializes a new instance of RegisterSimpleService.
        /// </summary>
        public RegisterSimpleService(ILogger<RegisterSimpleService> logger, IUnitOfWork unitOfWork, IPasswordHasherService passwordHasherService, JwtOptions? options)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _passwordHasherService = passwordHasherService;

            _issuer = Environment.GetEnvironmentVariable("APP_BASE_URL_PROIECT") ?? options?.Issuer ?? "http://localhost:5098";
            _key = Environment.GetEnvironmentVariable("LOGIN_TOKEN_KEY_PROIECTa") ?? options?.Key ?? "super_secret_key_123456789asdasdasdasd";
            _defaultDuration = int.Parse(Environment.GetEnvironmentVariable("JWT_DEFAULT_DURATION") ?? options?.DefaultDuration ?? "60");
        }

        /// <inheritdoc />
        public async Task<TokenDto> RegisterUserAsync(RegisterSimpleUserRequest request)
        {
            _logger.LogTrace("Register simple user called");

            // Validate request
            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                throw new BadRequestException("All fields are required.");
            }

            // Check if email already exists
            User? userWithSameEmail = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.Email == request.Email);
            if (userWithSameEmail != null)
            {
                throw new BadRequestException("User with provided email address already exists.");
            }

            // Create new user with basic info only
            User newUser = await _unitOfWork.UserRepository.AddAsync(User.Create(
                request.FirstName,
                request.LastName,
                request.Email,
                _passwordHasherService.GenerateHashedPassword(request.Password),
                isAdmin: false));

            await _unitOfWork.SaveChangesAsync();

            // Generate JWT token
            TokenDto tokenDto = new();
            tokenDto.Token = GenerateJSONWebToken(newUser, _defaultDuration);

            return tokenDto;
        }

        /// <summary>
        /// Generates new JSON Web Token for provided User.
        /// </summary>
        /// <param name="user">User entity.</param>
        /// <param name="duration">Token duration in minutes.</param>
        /// <returns>JWT token string.</returns>
        private string GenerateJSONWebToken(User user, double duration)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_key));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new("userId", user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("role", user.IsAdmin ? "Admin" : "User"),
            ];

            JwtSecurityToken token = new(_issuer,
              _issuer,
              claims,
              expires: DateTime.Now.AddMinutes(duration),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
