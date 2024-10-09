using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using EwentSwirl.RabbitMQ.Commands;

namespace EventSwirl.DataAccess.Handlers
{
    public class UpdateEventCommandHandler : BaseCommandHandler<UpdateEventCommand, UpdateEventResponse>
    {
        private IUnitOfWork _unitOfWork;

        public UpdateEventCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async override Task<UpdateEventResponse> HandleCommandAsync(UpdateEventCommand command)
        {
            await _unitOfWork.EventRepository.Update(command.Event).ConfigureAwait(false);
            _unitOfWork.Save();

            return new UpdateEventResponse();
        }
    }
}
