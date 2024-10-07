using EventSwirl.DataAccess.Interfaces;
using EventSwirl.DataAccess.Repositories;
using EventSwirl.Domain.Entities;

namespace EventSwirl.DataAccess
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private DataContext _context;
        private bool disposed = false;
        private IGenericRepository<Event> _eventRepository;
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<UserEvent> _userEventRepository;

        public UnitOfWork(DataContext context)
        {
            _context = context;
            _eventRepository = new GenericRepository<Event>(_context);
            _userRepository = new GenericRepository<User>(_context);
            _userEventRepository = new GenericRepository<UserEvent>(_context);
        }

        public IGenericRepository<Event> EventRepository => _eventRepository;

        public IGenericRepository<User> UserRepository => _userRepository;

        public IGenericRepository<UserEvent> UserEventRepository => _userEventRepository;

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
