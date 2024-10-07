using EventSwirl.Application.Data.DTOs;
using EventSwirl.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventSwirl.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [Route("create")]
        public async Task CreateEvent([FromBody] EventDTO eventInfo)
        {
            await _eventService.CreateEvent(eventInfo).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("edit")]
        public async Task EditEvent([FromBody] EventDTO eventInfo)
        {
            await _eventService.UpdateEvent(eventInfo).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<EventDTO> GetEventById(int id)
        {
            return await _eventService.GetEventById(id).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("events")]
        public async Task<IEnumerable<EventDTO>> GetEvents()
        {
            return await _eventService.GetAllEvents().ConfigureAwait(false);
        }

        [HttpGet]
        [Route("get/user/{id}")]
        public async Task<IEnumerable<EventDTO>> GetEventsByUser(int userId)
        {
            return await _eventService.GetEventsByUserId(userId).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task Delete(int id)
        {
            await _eventService.DeleteEventById(id).ConfigureAwait(false);
        }
    }
}
