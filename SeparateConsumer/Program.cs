using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SeparateConsumer;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory() { HostName = "localhost",
    UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? string.Empty,
    Password = Environment.GetEnvironmentVariable("RABBITMQ_USERPASS") ?? string.Empty
};

var seralizerOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

// Create a connection
using (var connection = await factory.CreateConnectionAsync())
{
    Console.WriteLine("Connected to RabbitMQ");
    // Create a channel
    using (var channel = await connection.CreateChannelAsync())
    {
        // Declare a queue
        await channel.QueueDeclareAsync(queue: "my-queue",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

        // Create a consumer
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);

            var messageBody = JsonSerializer.Deserialize<NotificationMessage>(messageJson, seralizerOptions);
            var content = messageBody?.Message?.Text;
            Console.WriteLine(" [x] Received {0}", content);
            await Task.CompletedTask;
        };

        // Consume messages from the queue
        await channel.BasicConsumeAsync(queue: "my-queue",
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}