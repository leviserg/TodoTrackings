namespace Application.Exceptions
{
    public class UpdateDbContextException : Exception
    {
        public UpdateDbContextException(string message) : base(message) { }
        public UpdateDbContextException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
