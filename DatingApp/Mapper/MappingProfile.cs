using AutoMapper;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.User;
using DatingApp.Dtos.Message;
using DatingApp.Dtos.Review;
using DatingApp.Dtos.Report;
using DatingApp.Dtos.Image;

namespace DatingApp.Mapper
{
    /// <summary>
    /// Defines AutoMapper profile.
    /// </summary>
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.FirstName, y => y.MapFrom(z => z.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(z => z.LastName))
                .ForMember(x => x.Email, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.Password, y => y.MapFrom(z => z.Password))
                .ForMember(x => x.IsAdmin, y => y.MapFrom(z => z.IsAdmin));

            CreateMap<Message, MessageDto>()
               .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
               .ForMember(x => x.SenderId, y => y.MapFrom(z => z.SenderId))
               .ForMember(x => x.RecipientId, y => y.MapFrom(z => z.RecipientId))
               .ForMember(x => x.Text, y => y.MapFrom(z => z.Text));

            CreateMap<Review, ReviewDto>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.ReviewerId, y => y.MapFrom(z => z.ReviewerId))
                .ForMember(x => x.RevieweeId, y => y.MapFrom(z => z.RevieweeId))
                .ForMember(x => x.Rating, y => y.MapFrom(z => z.Rating))
                .ForMember(x => x.Comment, y => y.MapFrom(z => z.Comment));

            CreateMap<Report, ReportDto>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.ReporterId, y => y.MapFrom(z => z.ReporterId))
                .ForMember(x => x.ReportedUserId, y => y.MapFrom(z => z.ReportedUserId))
                .ForMember(x => x.Reason, y => y.MapFrom(z => z.Reason));

            CreateMap<Image, ImageDto>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.userId))
                 .ForMember(dest => dest.ImageBase64, opt => opt.MapFrom(src =>
                     src.image != null ? Convert.ToBase64String(src.image) : null));

        }
    }
}