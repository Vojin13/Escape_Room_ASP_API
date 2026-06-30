using Application.DTO.Rooms;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile() 
        {
            CreateMap<CreateRoomDTO, Room>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
        }
    }
}
