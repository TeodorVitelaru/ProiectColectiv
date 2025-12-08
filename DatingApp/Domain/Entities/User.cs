﻿using DatingApp.Domain.Primitives;
using DatingApp.Enums;

namespace DatingApp.Domain.Entities
{
    public class User : Entity<long>
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public bool IsAdmin { get; private set; }

        // New registration fields
        public int? Age { get; private set; }

        public int? Height { get; private set; }

        public Gender? Gender { get; private set; }

        public string? City { get; private set; }

        public string? Bio { get; private set; }

        public RelationshipGoal? RelationshipGoal { get; private set; }

        public SexualOrientation? SexualOrientation { get; private set; }

        public int? PreferredAgeMin { get; private set; }

        public int? PreferredAgeMax { get; private set; }

        // Navigation properties
        public ICollection<UserLanguage> UserLanguages { get; private set; } = new List<UserLanguage>();

        public ICollection<UserInterest> UserInterests { get; private set; } = new List<UserInterest>();

        public ICollection<Image> Images { get; private set; } = new List<Image>();

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

        public static User CreateWithProfile(
            string firstName,
            string lastName,
            string email,
            string password,
            int age,
            int height,
            Gender gender,
            string city,
            string bio,
            RelationshipGoal relationshipGoal,
            SexualOrientation sexualOrientation,
            int preferredAgeMin,
            int preferredAgeMax)
        {
            User user = new()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                IsAdmin = false,
                Age = age,
                Height = height,
                Gender = gender,
                City = city,
                Bio = bio,
                RelationshipGoal = relationshipGoal,
                SexualOrientation = sexualOrientation,
                PreferredAgeMin = preferredAgeMin,
                PreferredAgeMax = preferredAgeMax
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

        public void UpdateAge(int age)
        {
            if (age < 18 || age > 100)
                throw new ArgumentException("Age must be between 18 and 100.", nameof(age));

            Age = age;
        }

        public void UpdateHeight(int height)
        {
            if (height < 100 || height > 250)
                throw new ArgumentException("Height must be between 100 and 250 cm.", nameof(height));

            Height = height;
        }

        public void UpdateGender(Gender gender)
        {
            Gender = gender;
        }

        public void UpdateCity(string city)
        {
            City = city;
        }

        public void UpdateBio(string bio)
        {
            Bio = bio;
        }

        public void UpdateRelationshipGoal(RelationshipGoal relationshipGoal)
        {
            RelationshipGoal = relationshipGoal;
        }

        public void UpdateSexualOrientation(SexualOrientation sexualOrientation)
        {
            SexualOrientation = sexualOrientation;
        }

        public void UpdatePreferredAgeRange(int min, int max)
        {
            if (min < 18 || max > 100 || min > max)
                throw new ArgumentException("Invalid age range.");

            PreferredAgeMin = min;
            PreferredAgeMax = max;
        }
    }
}
