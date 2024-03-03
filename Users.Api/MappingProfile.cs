using AutoMapper;
using Common.Domain;
using Users.Service.DTO;

namespace Users.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDTO, User>(); 
        }
    }
}
