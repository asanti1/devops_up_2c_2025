namespace Api.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        INoteRepository NoteRepository { get; }

        Task<int> Save();
    }
}