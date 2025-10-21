using DatingApp.Dtos.User;

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
    }
}
