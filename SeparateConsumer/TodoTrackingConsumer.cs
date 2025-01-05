using MassTransit;

namespace SeparateConsumer
{

    public class TodoTrackingConsumer : IConsumer<NotificationMessage>
    {

        public async Task Consume(ConsumeContext<NotificationMessage> context)
        {
            string message = context.Message?.Message?.Text ?? string.Empty;
            Console.WriteLine($"LOGGER: Received message: {message}");
            await Task.CompletedTask;
        }
    }
 
}
