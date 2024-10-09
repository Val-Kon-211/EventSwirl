using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using EwentSwirl.RabbitMQ.Commands;

namespace EventSwirl.DataAccess.Handlers
{
    public class CreateUserCommandHandler : BaseCommandHandler<CreateUserCommand, CreateUserResponse>
    {
        private IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async override Task<CreateUserResponse> HandleCommandAsync(CreateUserCommand command)
        {
            await _unitOfWork.UserRepository.Insert(command.User).ConfigureAwait(false);
            _unitOfWork.Save();

            return new CreateUserResponse();
        }
    }
}
