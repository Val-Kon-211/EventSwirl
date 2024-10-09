using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using EwentSwirl.RabbitMQ.Commands;

namespace EventSwirl.DataAccess.Handlers
{
    public class UpdateUserCommandHandler : BaseCommandHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async override Task<UpdateUserResponse> HandleCommandAsync(UpdateUserCommand command)
        {
            await _unitOfWork.UserRepository.Update(command.User).ConfigureAwait(false);
            _unitOfWork.Save();

            return new UpdateUserResponse();
        }
    }
}
