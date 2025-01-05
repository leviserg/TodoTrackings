using Domain.Events;
using Infrastructure.Messaging;
using MassTransit;
using MediatR;

namespace Application.Handlers.EventHandlers
{
    public class TaskCreatedEventHandler(IPublishEndpoint publishEndpoint) : INotificationHandler<TodoTaskCreated>
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        public async Task Handle(TodoTaskCreated notification, CancellationToken cancellationToken)
        {
            var message = $"ToDo task with Id={notification.TodoTaskItem.Id} has been CREATED: details : CONTENT:\t{notification.TodoTaskItem.Content};\tCREATED:{notification.TodoTaskItem.CreatedBy}\t at {notification.TodoTaskItem.CreatedAt.ToString("HH:mm:ss.fff")}";
            await _publishEndpoint.Publish(new TodoTaskChangedMessage(message), cancellationToken);
        }
    }
}
