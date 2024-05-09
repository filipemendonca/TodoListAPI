using AutoMapper;
using TodoList.Domain.Entities;
using TodoList.Dto.Notes;

namespace TodoList.MapperProfiles
{
    public class NotesProfile : Profile
    {
        public NotesProfile()
        {            
            CreateMap<NotesAddOrUpdateDto, Notes>();
        }
    }
}
