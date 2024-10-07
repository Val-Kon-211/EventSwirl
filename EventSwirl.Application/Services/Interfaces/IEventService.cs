using EventSwirl.Application.Data.DTOs;

namespace EventSwirl.Application.Services.Interfaces
{
    public interface IEventService
    {
        public Task CreateEvent(EventDTO newEvent);

        public Task<EventDTO> GetEventById(int id);

        public Task<IEnumerable<EventDTO>> GetAllEvents();

        public Task<IEnumerable<EventDTO>> GetEventsByUserId(int userId);

        public Task UpdateEvent(EventDTO newEvent);

        public Task DeleteEventById(int id);
    }
}
