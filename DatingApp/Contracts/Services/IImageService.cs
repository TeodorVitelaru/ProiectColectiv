using DatingApp.Dtos.Image;

namespace DatingApp.Contracts.Services
{
    public interface IImageService
    {
        Task<ImageDto> GetImageAsync(GetImageRequest request);

        Task<ImageDto> AddImageAsync(AddImageRequest request);

        Task DeleteImageAsync(DeleteImageRequest request);

        Task<IEnumerable<ImageDto>> GetAllImagesAsync();


    }
}
