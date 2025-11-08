using AutoMapper;
using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services;
using DatingApp.Contracts.Validators;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.Image;
using DatingApp.Dtos.User;
using DatingApp.Exceptions;
using Microsoft.AspNetCore.Components.Sections;

namespace DatingApp.Service
{
    public class ImageService: IImageService
    {
        private readonly ILogger<ImageService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestValidator _requestValidator;
        private readonly IMapper _mapper;

        public ImageService(ILogger<ImageService> logger, IUnitOfWork unitOfWork, IRequestValidator requestValidator,
        IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _requestValidator = requestValidator;
            _mapper = mapper;
        }

        public async Task<ImageDto> AddImageAsync(AddImageRequest request)
        {
            _logger.LogTrace("Add image called");

            //_requestValidator.Validate(request);


            //try
            //{
                Image image = await _unitOfWork.ImageRepository.AddAsync(Image.Create(
                Convert.FromBase64String(request.Image)));

                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<ImageDto>(image);
            //}
            //catch
            //{
            //    return null;
            //}
            
        }

        public async Task DeleteImageAsync(DeleteImageRequest request)
        {
            _logger.LogTrace("Delete image called");

            //_requestValidator.Validate(request);

            Image image = await _unitOfWork.ImageRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(request.Id);

            await _unitOfWork.ImageRepository.RemoveByIdAsync(image.Id);
            await _unitOfWork.SaveChangesAsync();
        }

    

        public async Task<IEnumerable<ImageDto>> GetAllImagesAsync()
        {
            _logger.LogTrace("Get all images called");

            IEnumerable<Image> images = await _unitOfWork.ImageRepository.GetAllAsync();

            if (!images.Any())
            {
                throw new NotFoundException("There are no images");
            }

            return _mapper.Map<IEnumerable<ImageDto>>(images);
        }

        public async Task<ImageDto> GetImageAsync(GetImageRequest request)
        {
            _logger.LogTrace("Get image called");

            //_requestValidator.Validate(request);

            Image image = await _unitOfWork.ImageRepository.FindFirstOrDefaultAsync(u => u.Id == request.Id) ?? throw new NotFoundException(request.Id);

            return _mapper.Map<ImageDto>(image);
        }
    }
}
