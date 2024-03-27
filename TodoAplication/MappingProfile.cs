using AutoMapper;
using TodoAplication.Commands.CreateTodo;
using TodoAplication.DTO;
using Todos.Domain;

namespace Todos.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateTodoDTO, Todo>();
            CreateMap<CreateTodoDTO, Todo>();
            CreateMap<CreateTodoCommand, Todo>();
        }
    }
}
