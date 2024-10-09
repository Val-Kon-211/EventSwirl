using AutoMapper;
using EventSwirl.Application.Data.DTOs;
using EventSwirl.Application.Services.Interfaces;
using EventSwirl.Domain.Entities;
using EwentSwirl.RabbitMQ;
using EwentSwirl.RabbitMQ.Commands;

namespace EventSwirl.Application.Services
{
    public class EventService: IEventService
    {
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public EventService(IMapper mapper, IProducer producer)
        {
            _mapper = mapper;
            _producer = producer;
        }

        public async Task CreateEvent(EventDTO newEvent)
        {
            await _producer
                .SendCommandAsync<CreateEventCommand, CreateEventResponse>(new CreateEventCommand() { Event = _mapper.Map<Event>(newEvent) })
                .ConfigureAwait(false);
        }

        public async Task<EventDTO> GetEventById(int id)
        {
            var requestedEventResponse = await _producer
                .SendCommandAsync<GetEventByIdCommand, GetEventByIdResponse>(new GetEventByIdCommand() { EventId = id })
                .ConfigureAwait(false);

            var requestedEvent = requestedEventResponse.Event;

            return requestedEvent == null 
                ? throw new ArgumentNullException() 
                : _mapper.Map<EventDTO>(requestedEvent);
        }

        public async Task<IEnumerable<EventDTO>> GetAllEvents()
        {
            var eventsResponse = await _producer
                .SendCommandAsync<GetAllEventsCommand, GetAllEventsResponse>(new GetAllEventsCommand())
                .ConfigureAwait(false);

            var events = eventsResponse.Events;

            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventDTO>>(events);
        }

        public async Task<IEnumerable<EventDTO>> GetEventsByUserId(int userId)
        {
            var eventsResponse = await _producer
                .SendCommandAsync<GetEventsByUserIdCommand, GetEventsByUserIdResponse>(new GetEventsByUserIdCommand() { UserId = userId})
                .ConfigureAwait(false);

            var events = eventsResponse.Events;

            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventDTO>>(events);
        }

        public async Task UpdateEvent(EventDTO eventInfo)
        {
            await _producer
                .SendCommandAsync<UpdateEventCommand, UpdateEventResponse>(new UpdateEventCommand() { Event = _mapper.Map<Event>(eventInfo) })
                .ConfigureAwait(false);
        }

        public async Task DeleteEventById(int id)
        {
            await _producer
                .SendCommandAsync<DeleteEventByIdCommand, DeleteEventByIdResponse>(new DeleteEventByIdCommand() { EventId = id })
                .ConfigureAwait(false);
        }
    }
}
