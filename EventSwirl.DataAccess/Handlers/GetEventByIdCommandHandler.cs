using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using EwentSwirl.RabbitMQ.Commands;

namespace EventSwirl.DataAccess.Handlers
{
    public class GetEventByIdCommandHandler : BaseCommandHandler<GetEventByIdCommand, GetEventByIdResponse>
    {
        public readonly IUnitOfWork _unitOfWork;

        public GetEventByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        protected async override Task<GetEventByIdResponse> HandleCommandAsync(GetEventByIdCommand command)
        {
            var requestedEvent = await _unitOfWork.EventRepository.GetById(command.EventId).ConfigureAwait(false);

            return new GetEventByIdResponse() { Event = requestedEvent };
        }
    }
}
