namespace DatingApp.Dtos.User.Login
{
    public class JwtOptions
    {
        public string Issuer { get; set; }

        /// <summary>
        /// Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// ClientId.
        /// </summary>
        public string DefaultDuration { get; set; }
    }
}
