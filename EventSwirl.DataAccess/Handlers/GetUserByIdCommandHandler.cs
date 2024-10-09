using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using EwentSwirl.RabbitMQ.Commands;

namespace EventSwirl.DataAccess.Handlers
{
    public class GetUserByIdCommandHandler : BaseCommandHandler<GetUserByIdCommand, GetUserByIdResponse>
    {
        private IUnitOfWork _unitOfWork;

        public GetUserByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async override Task<GetUserByIdResponse> HandleCommandAsync(GetUserByIdCommand command)
        {
            Console.WriteLine($"Handling GetUserByIdCommand for UserId: {command.UserId}");

            var user = await _unitOfWork.UserRepository.GetById(command.UserId).ConfigureAwait(false);

            return new GetUserByIdResponse { User = user };
        }
    }
}
