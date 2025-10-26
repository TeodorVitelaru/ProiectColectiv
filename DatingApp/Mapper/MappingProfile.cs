using AutoMapper;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.User;

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
        }
    }
}
