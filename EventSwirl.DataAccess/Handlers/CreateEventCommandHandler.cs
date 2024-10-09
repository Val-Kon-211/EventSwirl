using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using EwentSwirl.RabbitMQ.Commands;

namespace EventSwirl.DataAccess.Handlers
{
    public class CreateEventCommandHandler : BaseCommandHandler<CreateEventCommand, CreateEventResponse>
    {
        private IUnitOfWork _unitOfWork;

        public CreateEventCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async override Task<CreateEventResponse> HandleCommandAsync(CreateEventCommand command)
        {
            await _unitOfWork.EventRepository.Insert(command.Event).ConfigureAwait(false);
            _unitOfWork.Save();

            return new CreateEventResponse();
        }
    }
}
