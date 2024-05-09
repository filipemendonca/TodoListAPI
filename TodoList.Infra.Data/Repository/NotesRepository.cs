using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities;
using TodoList.Infra.Data.Repository.Interface;

namespace TodoList.Infra.Data.Repository
{
    public class NotesRepository : INotesRepository
    {
        private readonly TodoListContext _context;
        public NotesRepository(TodoListContext context)
        {
            _context = context;
        }

        public async Task<List<Notes>> GetAsync() => await _context.Notes.AsNoTracking().ToListAsync();

        public async Task<Notes> GetByIdAsync(int id) => await _context.Notes.FindAsync(id);

        public async Task<Notes> CreateOrEdit(Notes entity)
        {
            using (var transacao = _context.Database.BeginTransaction())
            {
                if (entity.Id is 0)
                {
                    entity.CreatedAt = DateTime.Now;
                    await _context.AddAsync(entity);
                }
                else
                {
                    entity.UpdatedAt = DateTime.Now;
                    _context.Update(entity);
                }

                await _context.SaveChangesAsync();

                transacao.Commit();

                return entity;
            }
        }

        public async Task Delete(Notes entity)
        {
            using (var transacao = _context.Database.BeginTransaction())
            {
                _context.Notes.Remove(entity);

                await _context.SaveChangesAsync();

                transacao.Commit();
            }
        }
    }
}
