using DatingApp.Contracts.Services.HelperService;

namespace DatingApp.Service.HelperService
{
    public class PasswordHelperService : IPasswordHasherService
    {
        /// <inheritdoc />
        public string GenerateHashedPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        /// <inheritdoc />
        public bool CheckPasswordMatch(string password, string hashedPassword)
        {
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return passwordMatch;
        }
    }
}
