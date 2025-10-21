using DatingApp.Contracts.Services.HelperService;
using DatingApp.Contracts.Validators;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.User.Login;
using DatingApp.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp.Service
{
    public class LoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestValidator _requestValidator;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IMapper _mapper;

        private readonly string _issuer;
        private readonly string _key;
        private readonly int _defaultDuration;

        public LoginService(ILogger<LoginService> logger, IUnitOfWork unitOfWork, IRequestValidator requestValidator, IPasswordHasherService passwordHasherService, IMapper mapper, JwtOptions? options)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _requestValidator = requestValidator;
            _passwordHasherService = passwordHasherService;
            _mapper = mapper;
\
            _issuer = Environment.GetEnvironmentVariable("APP_BASE_URL") ?? options.Issuer!;
            _key = Environment.GetEnvironmentVariable("LOGIN_TOKEN_KEY") ?? options.Key!;
            _defaultDuration = int.Parse(Environment.GetEnvironmentVariable("JWT_DEFAULT_DURATION") ?? options.DefaultDuration!);
        }

        /// <inheritdoc />
        public async Task<TokenDto> GetLoginToken(LoginUserRequest request)
        {
            _logger.LogTrace("Get login token called");

            _requestValidator.Validate(request);

            User user = await AuthenticateUserCredentials(request.Email, request.Password);

            TokenDto tokenDto = new();
            tokenDto.Token = GenerateJSONWebToken(user, _defaultDuration);

            return tokenDto;
        }

        /// <summary>
        /// Generates new JSON Web Token for provided <see cref="User"/>.
        /// </summary>
        /// <returns>JWT token.</returns>
        private string GenerateJSONWebToken(User user, double duration)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_key));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new Claim[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("isAdmin", user.IsAdmin.ToString())
            };

            JwtSecurityToken token = new(_issuer,
              _issuer,
              claims,
              expires: DateTime.Now.AddMinutes(duration),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Authenticates user credentials: <paramref name="email"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        /// <returns><see cref="User"/></returns>
        /// <exception cref="NotFoundException">If user with provided <paramref name="email"/> does not exists.</exception>
        /// <exception cref="BadRequestException">If <paramref name="password"/> not valid.</exception>
        private async Task<User> AuthenticateUserCredentials(string email, string password)
        {
            User? user = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.Email.Value == email, u => u.Role);

            if (user == null)
            {
                throw new Exception("User with provided email does not exist.");
            }

            bool correctPassword = _passwordHasherService.CheckPasswordMatch(password, user.Password);

            if (!correctPassword)
            {
                throw new BadRequestException("Invalid password.");
            }

            return user;
        }

    }
}
