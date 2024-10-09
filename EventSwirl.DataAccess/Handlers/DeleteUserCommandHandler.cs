using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using EwentSwirl.RabbitMQ.Commands;

namespace EventSwirl.DataAccess.Handlers
{
    public class DeleteUserCommandHandler : BaseCommandHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async override Task<DeleteUserResponse> HandleCommandAsync(DeleteUserCommand command)
        {
            await _unitOfWork.UserRepository.Delete(command.User).ConfigureAwait(false);
            _unitOfWork.Save();

            return new DeleteUserResponse();
        }
    }
}
