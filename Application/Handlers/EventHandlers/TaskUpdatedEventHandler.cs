using Domain.Events;
using Infrastructure.Messaging;
using MassTransit;
using MediatR;

namespace Application.Handlers.EventHandlers
{
    public class TaskUpdatedEventHandler(IPublishEndpoint publishEndpoint) : INotificationHandler<TodoTaskUpdated>
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        public async Task Handle(TodoTaskUpdated notification, CancellationToken cancellationToken)
        {
            var message = $"ToDo task with Id={notification.TodoTaskItem.Id} has been UPDATED: details : CONTENT:\t{notification.TodoTaskItem.Content};\nIsCompleted:{notification.TodoTaskItem.IsCompleted.ToString()}\t at {notification.TodoTaskItem.UpdatedAt.GetValueOrDefault().ToString("HH:mm:ss.fff")}";
            await _publishEndpoint.Publish(new TodoTaskChangedMessage(message), cancellationToken);
        }
    }
}
