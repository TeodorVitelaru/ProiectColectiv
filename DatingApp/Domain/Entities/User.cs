using DatingApp.Domain.Primitives;

namespace DatingApp.Domain.Entities
{
    public class User : Entity<long>
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public bool IsAdmin { get; private set; }

        protected User(long id) : base(id) { }

        protected User() : base() { }

        public static User Create(string firstName, string lastName, string email, string password, bool isAdmin)
        {
            User user = new ()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                IsAdmin = isAdmin
            };

            return user;
        }

        public void UpdateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty.", nameof(firstName));

            FirstName = firstName;
        }

        public void UpdateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

            LastName = lastName;
        }

        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            Email = email;
        }

        public void UpdatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            Password = password;
        }

        public void UpdateIsAdmin(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }
    }
}
