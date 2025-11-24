using AutoMapper;
using DatingApp.Contracts.Services;
using DatingApp.Dtos.Image;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ImageController(IImageService ImageService, IMapper mapper)
        {
            _imageService = ImageService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ImageDto>>> GetAll()
        {
            var images = await _imageService.GetAllImagesAsync();
            if (images == null || !images.Any()) return NotFound();
            return Ok(images);
        }

        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ImageDto>>> GetAllByUserId(long id)
        {
            var images = await _imageService.GetAllImagesByUserIdAsync(new GetImageRequest { Id = id });
            if (images == null || !images.Any()) return NotFound();
            return Ok(images);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _imageService.GetImageAsync(new GetImageRequest{Id = id});
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AddImageRequest request)
        {
            var dto = await _imageService.AddImageAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            await _imageService.DeleteImageAsync(new DeleteImageRequest { Id = id });
            return NoContent();
        }
    }
}
