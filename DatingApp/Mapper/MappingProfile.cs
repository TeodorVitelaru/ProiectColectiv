using AutoMapper;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.User;
using DatingApp.Dtos.Message;

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
           .ForMember(x => x.UserId1, y => y.MapFrom(z => z.UserId1))
           .ForMember(x => x.UserId2, y => y.MapFrom(z => z.UserId2))
           .ForMember(x => x.Text, y => y.MapFrom(z => z.Text));
        }
    }
}
