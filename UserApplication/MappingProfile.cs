﻿using AutoMapper;
using Common.Domain;
using Users.Service.DTO;

namespace Users.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetUserDTO, ApplicationUser>();
            CreateMap<CreateUserDTO, ApplicationUser>();
            CreateMap<UpdateUserDTO, ApplicationUser>();
            CreateMap<ApplicationUser, GetUserDTO>();
            CreateMap<ChangePasswordDTO, ApplicationUser>();
        }
    }
}
