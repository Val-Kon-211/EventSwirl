using AutoMapper;
using EventSwirl.Application.Data.DTOs;
using EventSwirl.Application.Services.Interfaces;
using EventSwirl.DataAccess.Interfaces;
using EventSwirl.Domain.Entities;
using System.Data.Entity;

namespace EventSwirl.Application.Services
{
    public class EventService: IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateEvent(EventDTO newEvent)
        {
            await _unitOfWork.EventRepository.Insert(_mapper.Map<Event>(newEvent)).ConfigureAwait(false);
            _unitOfWork.Save();
        }

        public async Task<EventDTO> GetEventById(int id)
        {
            var requestedEvent = await _unitOfWork.EventRepository.GetById(id).ConfigureAwait(false);

            return requestedEvent == null 
                ? throw new ArgumentNullException() 
                : _mapper.Map<EventDTO>(requestedEvent);
        }

        public async Task<IEnumerable<EventDTO>> GetAllEvents()
        {
            var events = await _unitOfWork.EventRepository.GetAll().ConfigureAwait(false);
            return _mapper.Map<IQueryable<Event>, IQueryable<EventDTO>>(events);
        }

        public async Task<IEnumerable<EventDTO>> GetEventsByUserId(int userId)
        {
            var events = await _unitOfWork.EventRepository
                .Query()
                .Where(e => e.Creator.Id == userId)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventDTO>>(events);
        }

        public async Task UpdateEvent(EventDTO eventInfo)
        {
            await _unitOfWork.EventRepository.Update(_mapper.Map<Event>(eventInfo)).ConfigureAwait(false);
            _unitOfWork.Save();
        }

        public async Task DeleteEventById(int id)
        {
            var delEvent = await _unitOfWork.EventRepository.GetById(id).ConfigureAwait(false);

            if (delEvent == null)
                throw new ArgumentNullException(nameof(delEvent));

            await _unitOfWork.EventRepository.Delete(delEvent).ConfigureAwait(false);
            _unitOfWork.Save();
        }
    }
}
