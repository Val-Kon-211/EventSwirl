using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using EwentSwirl.RabbitMQ.Commands;
using System.Data.Entity;

namespace EventSwirl.DataAccess.Handlers
{
    public class GetUserByLoginCommandHandler : BaseCommandHandler<GetUserByLoginCommand, GetUserByLoginResponse>
    {
        private IUnitOfWork _unitOfWork;

        public GetUserByLoginCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async override Task<GetUserByLoginResponse> HandleCommandAsync(GetUserByLoginCommand command)
        {
            var user = await _unitOfWork
                .UserRepository
                .Query()
                .Where(u => u.Login == command.Login)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return new GetUserByLoginResponse() { User = user };
        }
    }
}
