﻿﻿using AutoMapper;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.User;
using DatingApp.Dtos.Message;
using DatingApp.Dtos.Review;
using DatingApp.Dtos.Report;
using DatingApp.Dtos.Image;
using DatingApp.Dtos.Match;

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
                .ForMember(x => x.IsAdmin, y => y.MapFrom(z => z.IsAdmin))
                .ForMember(x => x.Age, y => y.MapFrom(z => z.Age))
                .ForMember(x => x.Height, y => y.MapFrom(z => z.Height))
                .ForMember(x => x.Gender, y => y.MapFrom(z => z.Gender))
                .ForMember(x => x.City, y => y.MapFrom(z => z.City))
                .ForMember(x => x.Bio, y => y.MapFrom(z => z.Bio))
                .ForMember(x => x.RelationshipGoal, y => y.MapFrom(z => z.RelationshipGoal))
                .ForMember(x => x.SexualOrientation, y => y.MapFrom(z => z.SexualOrientation))
                .ForMember(x => x.PreferredAgeMin, y => y.MapFrom(z => z.PreferredAgeMin))
                .ForMember(x => x.PreferredAgeMax, y => y.MapFrom(z => z.PreferredAgeMax))
                .ForMember(x => x.Languages, y => y.MapFrom(z => z.UserLanguages.Select(ul => ul.Language).ToList()))
                .ForMember(x => x.Interests, y => y.MapFrom(z => z.UserInterests.Select(ui => ui.Interest).ToList()))
                .ForMember(x => x.Photos, y => y.MapFrom(z => z.Images.Select(i => Convert.ToBase64String(i.ImageData)).ToList()));

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
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                 .ForMember(dest => dest.ImageData, opt => opt.MapFrom(src =>
                     src.ImageData != null ? Convert.ToBase64String(src.ImageData) : null));

            CreateMap<Match, MatchDto>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserId))
                .ForMember(x => x.MatchedUserId, y => y.MapFrom(z => z.MatchedUserId))
                .ForMember(x => x.IsMutual, y => y.MapFrom(z => z.IsMutual))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => z.CreatedAt));
        }
    }
}