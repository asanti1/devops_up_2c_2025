using Microsoft.EntityFrameworkCore;

namespace Api.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public INoteRepository NoteRepository { get; }

        private readonly NoteContext _context;

        public UnitOfWork(NoteContext context, INoteRepository noteRepository)
        {
            _context = context;
            NoteRepository = noteRepository;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
        public Task<int> Save()
        {
            throw new NotImplementedException();
        }
    }
}