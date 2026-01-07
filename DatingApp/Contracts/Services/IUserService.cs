﻿using DatingApp.Dtos.User;

namespace DatingApp.Contracts.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Asynchronously gets the <see cref="UserDto"/> object based on provided <paramref name="request"/>.
        /// If <see cref="UserDto"/> is not found, throws <see cref="NotFoundException"/>
        /// </summary>
        /// <exception cref="NotFoundException">Thrown when <see cref="UserDto"/> is not found.</exception>
        Task<UserDto> GetUserAsync(GetUserRequest request);

        /// <summary>
        /// Asynchronously gets the collection <see cref="UserDto"/> objects.
        /// If no <see cref="UserDto"/> is found, throws <see cref="NotFoundException"/>
        /// </summary>
        /// <exception cref="NotFoundException">Thrown when no <see cref="UserDto"/> is found.</exception>
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        /// <summary>
        /// Asynchronously adds the <see cref="UserDto"/> object based on provided <paramref name="request"/>.
        /// </summary>
        /// <returns>Added <see cref="UserDto"/></returns>
        Task<UserDto> AddUserAsync(AddUserRequest request);

        /// <summary>
        /// Asynchronously registers a new user with complete profile based on provided <paramref name="request"/>.
        /// </summary>
        /// <returns>Registered <see cref="UserDto"/></returns>
        Task<UserDto> RegisterUserAsync(RegisterUserRequest request);

        /// <summary>
        /// Asynchronously registers a new user and returns JWT token for authentication.
        /// </summary>
        /// <param name="request">Registration request with complete profile.</param>
        /// <returns>JWT Token</returns>
        Task<DatingApp.Dtos.User.Login.TokenDto> RegisterUserWithTokenAsync(RegisterUserRequest request);

        /// <summary>
        /// Asynchronously edits the <see cref="UserDto"/> object based on provided <paramref name="request"/>.
        /// If <see cref="UserDto"/> is not found, throws <see cref="NotFoundException"/>
        /// </summary>
        /// <exception cref="NotFoundException">Thrown when <see cref="UserDto"/> is not found.</exception>
        /// <returns>Updated <see cref="UserDto"/></returns>
        Task<UserDto> EditUserAsync(EditUserRequest request);

        /// <summary>
        /// Asynchronously deletes the <see cref="UserDto"/> object based on provided <paramref name="request"/>.
        /// If <see cref="UserDto"/> is not found, throws <see cref="NotFoundException"/>
        /// </summary>
        /// <exception cref="NotFoundException">Thrown when <see cref="UserDto"/> is not found.</exception>
        Task DeleteUserAsync(DeleteUserRequest request);

        /// <summary>
        /// Asynchronously gets a random <see cref="UserDto"/> object.
        /// If no <see cref="UserDto"/> is found, throws <see cref="NotFoundException"/>
        /// </summary>
        /// <exception cref="NotFoundException">Thrown when no <see cref="UserDto"/> is found.</exception>
        Task<UserDto> GetRandomUserAsync(int currentUserId);

        /// <summary>
        /// Asynchronously sets up a user's profile based on provided <paramref name="userId"/> and <paramref name="request"/>.
        /// If <see cref="UserDto"/> is not found, throws <see cref="NotFoundException"/>
        /// </summary>
        /// <exception cref="NotFoundException">Thrown when <see cref="UserDto"/> is not found.</exception>
        /// <returns>Updated <see cref="SetupProfileResponse"/></returns>
        Task<SetupProfileResponse> SetupProfileAsync(long userId, SetupProfileRequest request);
    }
}
