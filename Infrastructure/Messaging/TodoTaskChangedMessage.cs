namespace Infrastructure.Messaging
{
    public class TodoTaskChangedMessage
    {
        public string Text { get; }

        public TodoTaskChangedMessage(string text)
        {
            Text = text;
        }
    }
}
