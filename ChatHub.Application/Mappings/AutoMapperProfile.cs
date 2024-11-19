using AutoMapper;
using ChatHub.Application.Features.ChatRecords.Dtos;
using ChatHub.Application.Features.Users.Dtos;
using ChatHub.Domain.Entity.Chat;
using ChatHub.Domain.Entity.setup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHub.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Add all your individual mappings here
            CreateMap<User, UserDto>();
            CreateMap<ChatHistory, ChatHistoryReturnDto>()
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Message)); // Simple one-to-one for now


            //CreateMap<Role, RoleDto>();

            // Add more mappings as needed
        }
    }
}
