using EwentSwirl.RabbitMQ.Commands;
using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using System.Data.Entity;

namespace EventSwirl.DataAccess.Handlers
{
    public class GetEventsByUserIdCommandHandler : BaseCommandHandler<GetEventsByUserIdCommand, GetEventsByUserIdResponse>
    {
        public readonly IUnitOfWork _unitOfWork;

        public GetEventsByUserIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async override Task<GetEventsByUserIdResponse> HandleCommandAsync(GetEventsByUserIdCommand command)
        {
            var events = await _unitOfWork.EventRepository
                .Query()
                .Where(e => e.Creator.Id == command.UserId)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return new GetEventsByUserIdResponse { Events = events };
        }
    }
}
