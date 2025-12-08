﻿using DatingApp.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Dtos.User
{
    /// <summary>
    /// Request used for user registration with complete profile.
    /// </summary>
    public class RegisterUserRequest
    {
        // Basic Information (Step 1)
        /// <summary>
        /// Gets or sets the user age.
        /// </summary>
        [Required]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the user height in centimeters.
        /// </summary>
        [Required]
        [Range(100, 250, ErrorMessage = "Height must be between 100 and 250 cm")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the user gender.
        /// </summary>
        [Required]
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the user city.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string Location { get; set; } = null!;

        /// <summary>
        /// Gets or sets the languages spoken by the user.
        /// </summary>
        [Required]
        [MinLength(1, ErrorMessage = "At least one language is required")]
        public List<Language> Languages { get; set; } = new();

        // Photos (Step 2)
        /// <summary>
        /// Gets or sets the user photos.
        /// </summary>
        [Required]
        [MinLength(2, ErrorMessage = "At least 2 photos are required")]
        [MaxLength(6, ErrorMessage = "Maximum 6 photos allowed")]
        public List<IFormFile>? Photos { get; set; } = new();

        // Passions & Interests (Step 3)
        /// <summary>
        /// Gets or sets the user interests.
        /// </summary>
        [Required]
        [MinLength(1, ErrorMessage = "At least one interest is required")]
        [MaxLength(5, ErrorMessage = "Maximum 5 interests allowed")]
        public List<Interest> Hobbies { get; set; } = new();

        /// <summary>
        /// Gets or sets the bio/about you text.
        /// </summary>
        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Bio must be between 10 and 500 characters")]
        public string Bio { get; set; } = null!;

        // Preferences (Step 4)
        /// <summary>
        /// Gets or sets the relationship goal.
        /// </summary>
        [Required]
        public RelationshipGoal RelationshipGoal { get; set; }

        /// <summary>
        /// Gets or sets the sexual orientation.
        /// </summary>
        [Required]
        public SexualOrientation SexualOrientation { get; set; }

        /// <summary>
        /// Gets or sets the minimum preferred age.
        /// </summary>
        [Required]
        [Range(18, 100, ErrorMessage = "Minimum age must be between 18 and 100")]
        public int PreferredAgeMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum preferred age.
        /// </summary>
        [Required]
        [Range(18, 120, ErrorMessage = "Maximum age must be between 18 and 120")]
        public int PreferredAgeMax { get; set; }

        // Login credentials
        /// <summary>
        /// Gets or sets the user first name.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user last name.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = null!;
    }
}

