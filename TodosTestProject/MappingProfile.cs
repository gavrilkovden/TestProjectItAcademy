using AutoMapper;
using Todos.Domain;
using Todos.Service.DTO;

namespace Todos.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoDTO, Todo>();
        }
    }
}
