using EventSwirl.Domain.Entities;

namespace EventSwirl.DataAccess.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        public IGenericRepository<Event> EventRepository { get; }

        public IGenericRepository<User> UserRepository { get; }

        public IGenericRepository<UserEvent> UserEventRepository { get; }

        public void Save();

        public void Dispose();
    }
}
