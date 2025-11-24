using DatingApp.Dtos.Hobbie;
using DatingApp.Dtos.Review;
using DatingApp.Dtos.User;

namespace DatingApp.Contracts.Services
{
    public interface IHobbieService
    {
        Task<IEnumerable<HobbieDto>> GetAllHobbiesAsync();

        Task<HobbieDto> GetHobbieAsync(GetHobbieRequest request);

        Task<HobbieDto> AddHobbieAsync(AddHobbieRequest request);

        Task<HobbieDto> EditHobbieAsync(EditHobbieRequest request);

        Task DeleteReviewAsync(DeleteHobbieRequest request);

        Task<IEnumerable<HobbieDto>> GetAllHobbiesForUserAsync(GetAllHobbiesForUserRequest request);

        Task<UserDto> AddHobbieToUserAsync(AddHobbieToUserRequest request);
    }
}
