namespace SeparateConsumer;
public class NotificationMessage
{
    public MessageBody? Message { get; set; }

}

public class MessageBody
{
    public string? Text { get; set; }
}