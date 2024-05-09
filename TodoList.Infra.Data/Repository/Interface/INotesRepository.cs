
using TodoList.Domain.Entities;

namespace TodoList.Infra.Data.Repository.Interface
{
    public interface INotesRepository
    {
        Task<List<Notes>> GetAsync();
        Task<Notes> GetByIdAsync(int id);
        Task<Notes> CreateOrEdit(Notes entity);
        Task Delete(Notes entity);
    }
}
