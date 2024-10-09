using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using EwentSwirl.RabbitMQ.Commands;

namespace EventSwirl.DataAccess.Handlers
{
    public class GetAllEventsCommandHandler : BaseCommandHandler<GetAllEventsCommand, GetAllEventsResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllEventsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async override Task<GetAllEventsResponse> HandleCommandAsync(GetAllEventsCommand command)
        {
            var events = await _unitOfWork.EventRepository.GetAll().ConfigureAwait(false);

            return new GetAllEventsResponse() { Events = events.ToArray() };
        }
    }
}
