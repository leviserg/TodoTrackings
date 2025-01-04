using Application.Messages;
using Domain.Events;
using MassTransit;

namespace Application.Handlers.EventHandlers
{
    public class TaskCreatedEventHandler(IPublishEndpoint publishEndpoint)
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        public async Task Handle(TodoTaskCreated notification, CancellationToken cancellationToken)
        {
            var message = $"ToDo task with Id={notification.Id} has been created: details : CONTENT:\t{notification.Content};\tCREATED:{notification.CreatedBy}";
            await _publishEndpoint.Publish(new TodoTaskChangedMessage(message), cancellationToken);
        }
    }
}
