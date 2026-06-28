using Application.DTO.Auth;
using AutoMapper;
using Domain.Entities;

namespace Implementation.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDTO, User>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());
        }
    }
}
