using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using EwentSwirl.RabbitMQ.Commands;

namespace EventSwirl.DataAccess.Handlers
{
    public class DeleteEventByIdCommandHandler : BaseCommandHandler<DeleteEventByIdCommand, DeleteEventByIdResponse>
    {
        private IUnitOfWork _unitOfWork;

        public DeleteEventByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async override Task<DeleteEventByIdResponse> HandleCommandAsync(DeleteEventByIdCommand command)
        {
            var delEvent = await _unitOfWork.EventRepository.GetById(command.EventId).ConfigureAwait(false);

            if (delEvent == null)
                throw new ArgumentNullException(nameof(delEvent));

            await _unitOfWork.EventRepository.Delete(delEvent).ConfigureAwait(false);
            _unitOfWork.Save();

            return new DeleteEventByIdResponse();
        }
    }
}
