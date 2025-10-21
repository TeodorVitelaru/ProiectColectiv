namespace DatingApp.Contracts.Services.HelperService
{
    public interface IPasswordHasherService
    {
        /// <summary>
        /// Generates hashed password based on <paramref name="password"/>.
        /// </summary>
        /// <param name="password">Plain password.</param>
        /// <returns>Hashed password.</returns>
        public string GenerateHashedPassword(string password);

        /// <summary>
        /// Checks if plain and hashed passwords match.
        /// </summary>
        /// <param name="password">Plain password.</param>
        /// <param name="hashedPassword">Hashed password.</param>
        /// <returns>True if <paramref name="password"/> and <paramref name="hashedPassword"/> match, otherwise false.</returns>

        public bool CheckPasswordMatch(string password, string hashedPassword);
    }
}
