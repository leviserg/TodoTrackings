using Domain.Events;
using Infrastructure.Messaging;
using MassTransit;
using MediatR;

namespace Application.Handlers.EventHandlers
{
    public class TaskDeletedEventHandler(IPublishEndpoint publishEndpoint) : INotificationHandler<TodoTaskDeleted>
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        public async Task Handle(TodoTaskDeleted notification, CancellationToken cancellationToken)
        {
            var message = $"ToDo task with Id={notification.TodoTaskItem.Id} has been DELETED at:\t{DateTime.UtcNow.ToString("HH:mm:ss.fff")}";
            await _publishEndpoint.Publish(new TodoTaskChangedMessage(message), cancellationToken);
        }
    }
}
