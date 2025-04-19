namespace BuildingBlocks.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {

        }

        public BadRequestException(string messag, string details)
        {
            Details = details;
        }

        public string? Details { get; }
    }
}
